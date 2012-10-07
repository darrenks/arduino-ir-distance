using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

using NAudio.Wave;

namespace DistanceDemos
{
    public partial class MusicDemo : Form
    {
        private string[,] ORGAN_PRESETS = {{"Standard", "88 8888 888"}, {"Jimmy Smith", "88 8000 000"}, {"Gospel", "88 8000 008"}, {"Stopped Flute", "00 5320 000"}, {"Dulciana", "00 4432 000"}, {"French Horn", "00 8740 000"}, {"Salacional", "00 4544 222"}, {"Flutes 8' & 4'", "00 5403 000"}, {"Oboe Horn", "00 4675 300"}, {"Swell Diapason", "00 5644 320"}, {"Trumpet", "00 6876 540"}, {"Full Swell", "32 7645 222"}, {"Cello", "00 4545 440"}, {"Flute & String", "00 4423 220"}, {"Clarinet", "00 7373 430"}, {"Diapason, Gamba & Flute", "00 4544 220"}, {"Great, no reeds", "00 6644 322"}, {"Open Diapason", "00 5642 200"}, {"Full Great", "00 6845 433"}, {"Tibia Clausa", "00 8030 000"}, {"Full Great with 16'", "42 7866 244"}};
        private enum ScaleType {Major, Minor, Chromatic, Whole, Blues};
        private Dictionary<ScaleType, int[]> scales;
        private int[] MajorScale = { 0, 2, 4, 5, 7, 9, 11, 12 };
        private int[] MinorScale = { 0, 2, 3, 5, 7, 8, 10, 12 };
        private int[] ChromaticScale = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private int[] WholeToneScale = { 0, 2, 4, 6, 8, 10, 12 };
        private int[] BluesScale = { 0, 2, 3, 4, 5, 7, 9, 10, 11, 12 };

        private string SomewhereOverTheRainbow = "104|8|A4---A5---G#5-E5F#5G#5-A5-A4---F#5---E5-------F#4---D5---C#5-A4B4C#5-D5-B4-G#4A4B4-C#5-A4---....";
        private string Hysteria = "100|16|A2A2G3A2G3A3A2G3A2F3A2F3F3E3D3E3/E2E2D3E2D3E3E2G3E2E3E2G3G3E3G3A3/D3D3C4D3C4D4D3C4D3C4B3D3B3A#3D3A#3/A3A2G3A2G3A3A2G3A2F3A2F3F3E3D3E3/A2A2G3A2G3A3A2G3A2F3A2F3F3E3D3E3/E2E2D3E2D3E3E2G3E2E3E2G3G3E3G3A3/D3D3C4D3C4D4D3C4D3C4B3D3B3A#3D3A#3/A3A2G3A2G3A3A2G3A2F3A2F3F3E3D3E3";
        private List<Song> songs;
        private bool playingSong;

        private WaveOut waveOut;
        private CustomSoundProvider sound;
        private DistanceSensors sensors;
        private List<float> waveForm;
        private float frequency, amplitude;
        private int soundType = 0;
        private int numOctaves = 2;
        private ScaleType scaleType = ScaleType.Chromatic;
        private bool allowPartials = false;
        private int scale = 0;
        private float prevDist1 = -1, prevDist2 = -1;

        public MusicDemo()
        {
            InitializeComponent();
        }

        private void MusicDemo_Load(object sender, EventArgs e)
        {
            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();

            scales = new Dictionary<ScaleType, int[]>();
            scales[ScaleType.Major] = MajorScale;
            scales[ScaleType.Minor] = MinorScale;
            scales[ScaleType.Chromatic] = ChromaticScale;
            scales[ScaleType.Whole] = WholeToneScale;
            scales[ScaleType.Blues] = BluesScale;

            songs = new List<Song>();
            //songs.Add(new Song(Hysteria));
            songs.Add(new Song(SomewhereOverTheRainbow));

            waveForm = new List<float>();

            sound = new CustomSoundProvider();
            sound.SetWaveFormat(16000, 1);
            sound.DataReady += new CustomSoundProvider.DataReadyDelegate(sound_DataReady);
            frequency = 440;
            amplitude = 1.0f;
            SoundChooser.SelectedIndex = 0;

            OrganBar1.Tag = 0;
            OrganBar2.Tag = 1;
            OrganBar3.Tag = 2;
            OrganBar4.Tag = 3;
            OrganBar5.Tag = 4;
            OrganBar6.Tag = 5;
            OrganBar7.Tag = 6;
            OrganBar8.Tag = 7;
            OrganBar9.Tag = 8;
            OrganSettingsPanel.Visible = false;
            for (int i = 0; i < ORGAN_PRESETS.GetLength(0); i++) PresetChooser.Items.Add(ORGAN_PRESETS[i, 0]);
            PresetChooser.SelectedIndex = 0;

            ScaleChooser.SelectedIndex = 0;
            ScaleTypeChooser.SelectedIndex = 0;
            NumOctavesChooser.SelectedIndex = 1;

            waveOut = new NAudio.Wave.WaveOut();
            waveOut.Init(sound);
            waveOut.Play();

            SetTone(frequency, amplitude);
        }

        void sound_DataReady(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i++) waveForm.Add(amplitude * buffer[offset + i]);
            int toRemove = waveForm.Count - 10 * 16000;
            if (toRemove > 0) waveForm.RemoveRange(0, toRemove);
            DisplayPanel.Refresh();
            //Piano.Refresh();
        }

        void sensors_DistancesChanged(double[] dists)
        {
            float dist1 = (float)dists[0];
            float dist2 = (float)dists[1];
            if (dist1 == 80 && prevDist1 >= 0 && prevDist1 < 50 /*&& Math.Abs(dist1 - prevDist1) > 20*/) dist1 = prevDist1;
            if (dist2 == 80 && prevDist2 >= 0 && prevDist2 < 50 /*&& Math.Abs(dist2 - prevDist2) > 20*/) dist2 = prevDist2;
            prevDist1 = dist1;
            prevDist2 = dist2;

            // normalize distance
            float x = (dist1 - 5) / 45.0f;
            float y = (dist2 - 5) / 45.0f;
            
            // fix in [0, 1]
            if (x < 0) x = 0;
            else if (x > 1) x = 1;
            x = 1 - x;
            if (y < 0) y = 0;
            else if (y > 1) y = 1;
            y = 1 - y;

            // use distance percentage to compute note:
            float percentage = x;
            int lowerOctaves = Math.Min((int)Math.Floor(numOctaves / 2.0), 2);
            float midi = 60 - 12 * lowerOctaves + numOctaves * 12 * percentage;
            if (!allowPartials) midi = RoundToScale(midi);
            if(!playingSong) frequency = MidiToFrequency(midi);

            //if (frequency < 55) frequency = 55;
            //else if (frequency > 3322.4375f) frequency = 3322.4375f;

            amplitude = dists[0] == 80 && !playingSong ? 0 : y;

            //if (playingSong)
            //{
            //    Song.Note note = songs[0].Next;
            //    if (note == null)
            //        playingSong = false;
            //    else
            //    {
            //        frequency = (float)note.frequency;
            //        amplitude = amplitude * (float)note.amplitude;
            //    }
            //}
            
            if(!playingSong) SetTone(frequency, amplitude);
            //sound.TremeloAmplitude = z;

            //if(!playingSong)
                Invoke(new MethodInvoker(delegate
                {
                    FrequencyLabel.Text = frequency.ToString("0") + " Hz";
                    Piano.Refresh();
                }));
        }

        private void PlaySongButton_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            songs[0].Reset();
            playingSong = true;
            Task.Factory.StartNew(() =>
            {
                while (!IsDisposed)
                {
                    try
                    {
                        Song.Note note = songs[0].Next;
                        if (note == null) break;
                        frequency = (float)note.frequency;
                        SetTone((float)note.frequency, (float)note.amplitude * amplitude);

                        //Invoke(new MethodInvoker(delegate
                        //{
                        //    FrequencyLabel.Text = frequency.ToString("0") + " Hz";
                        //    Piano.Refresh();
                        //}));

                        //Thread.Sleep(10);
                    }
                    catch { }
                }
                playingSong = false;
                frequency = 440;
                amplitude = 0;
            });
        }

        private void AllowPartialCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            allowPartials = AllowPartialCheckbox.Checked;
        }

        private void ScaleChooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            scale = ScaleChooser.SelectedIndex;
        }

        private void ScaleTypeChooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            scaleType = (ScaleType)Enum.ToObject(typeof(ScaleType), ScaleTypeChooser.SelectedIndex);
        }

        private void NumOctavesChooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            numOctaves = NumOctavesChooser.SelectedIndex + 1;
            int numOctavesLower = Math.Min((int)Math.Floor(numOctaves / 2.0), 2);
            float maxNote = 60 + (numOctaves - numOctavesLower) * 12 - 1;
            float minNote = 60 - numOctavesLower * 12;
            float midi = FrequencyToMidi(frequency);
            if (midi < minNote) { midi = minNote; frequency = MidiToFrequency(midi); SetTone(frequency, amplitude); }
            if (midi > maxNote) { midi = maxNote; frequency = MidiToFrequency(midi); SetTone(frequency, amplitude); }
            Piano.Refresh();
        }

        private void SmoothTransitionsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Harmonic h in sound.Layers)
            {
                h.Sine.SmoothTransition = SmoothTransitionsCheckbox.Checked;
                h.Square.SmoothTransition = SmoothTransitionsCheckbox.Checked;
                h.Sawtooth.SmoothTransition = SmoothTransitionsCheckbox.Checked;
                h.Triangle.SmoothTransition = SmoothTransitionsCheckbox.Checked;
            }
        }

        // formulas for midi to frequency and back from http://www.phys.unsw.edu.au/jw/notes.html
        public static float MidiToFrequency(float midi)
        {
            float frequency = 440 * (float)Math.Pow(2, (midi - 69) / 12);
            return frequency;
        }
        public static float FrequencyToMidi(float frequency)
        {
            float midi = 69 + 12 * (float)(Math.Log(frequency / 440.0) / Math.Log(2.0));
            return midi;
        }

        private int RoundToScale(float midi)
        {
            int octave = (int)(midi / 12);
            float scaleNote = midi - octave * 12;
            int bestNote = 0;
            float minDist = float.MaxValue;
            int[] scaleNotes = scales[scaleType];
            foreach (int note in scaleNotes)
            {
                int actualNote = (note + scale) % 12;
                float dist = Math.Abs(actualNote - scaleNote);
                if (dist < minDist)
                {
                    minDist = dist;
                    bestNote = actualNote;
                }
            }
            return octave * 12 + bestNote;
        }

        private void MusicDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // clean up
            waveOut.Dispose();
            sensors.Disconnect();
        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Navy);
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, DisplayPanel.Width - 1, DisplayPanel.Height - 1);
            
            int lastY = DisplayPanel.Height / 4;
            for (int x = 0; x < DisplayPanel.Width - 2; x++)
            {
                float j = 0;
                float k = (float)x / (float)DisplayPanel.Width * (10 * 16000.0f);
                if (k < waveForm.Count) j = waveForm[(int)k];
                int y = DisplayPanel.Height / 4 + (int)((DisplayPanel.Height - 2) / 4 * j);
                e.Graphics.DrawLine(Pens.LightBlue, x == 0 ? 1 : x, lastY, x + 1, y);
                lastY = y;
            }

            lastY = 3 * DisplayPanel.Height / 4;
            for (int x = 0; x < DisplayPanel.Width - 2; x++)
            {
                float j = 0;
                int k = Math.Max(0, waveForm.Count - DisplayPanel.Width + x);
                if (k < waveForm.Count) j = waveForm[k];
                int y = 3 * DisplayPanel.Height / 4 + (int)((DisplayPanel.Height - 2) / 4 * j);
                e.Graphics.DrawLine(Pens.Red, x == 0 ? 1 : x, lastY, x + 1, y);
                lastY = y;
            }
        }

        private void Piano_Paint(object sender, PaintEventArgs e)
        {
            //int numOctaves = 6;
            int keyWidth = 15;
            int keyHeight = 60;
            int blackKeyWidth = 12;
            int blackKeyHeight = 40;
            int shiftX = (Piano.Width - keyWidth * numOctaves * 7) / 2;
            int shiftY = (Piano.Height - keyHeight) / 2;

            float currNote = FrequencyToMidi(frequency);
            int numLowerOctaves = Math.Min((int)Math.Floor(numOctaves / 2.0f), 2);
            int floorNote = (int)Math.Floor(currNote) - (60 - numLowerOctaves * 12);
            float percent = currNote - (60 - numLowerOctaves * 12) - floorNote;
            int octave = (int)(floorNote / 12);
            int k = floorNote % 12;
            int w = octave * 7 + WhiteIndex(k);

            // draw white keys
            for (int i = 0; i < 7 * numOctaves; i++)
            {
                e.Graphics.FillRectangle(Brushes.White, shiftX + keyWidth * i, shiftY, keyWidth, keyHeight);
                e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * i, shiftY, keyWidth, keyHeight);
            }

            // draw highlighted white key(s)
            if (percent < 1E-5)
            {
                if (!IsBlackNote(floorNote))
                {
                    e.Graphics.FillRectangle(Brushes.Yellow, shiftX + keyWidth * w, shiftY, keyWidth, keyHeight);
                    e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * w, shiftY, keyWidth, keyHeight);
                }
            }
            else
            {
                if (!IsBlackNote(floorNote))
                {
                    Color c1 = Color.FromArgb((int)((1 - percent) * 255), Color.Yellow);
                    e.Graphics.FillRectangle(new SolidBrush(c1), shiftX + keyWidth * w, shiftY, keyWidth, keyHeight);
                    e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * w, shiftY, keyWidth, keyHeight);

                    if (k == 4 || k == 11)
                    {
                        Color c2 = Color.FromArgb((int)(percent * 255), Color.Yellow);
                        e.Graphics.FillRectangle(new SolidBrush(c2), shiftX + keyWidth * (w + 1), shiftY, keyWidth, keyHeight);
                        e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * (w + 1), shiftY, keyWidth, keyHeight);
                    }
                }
                else
                {
                    Color c2 = Color.FromArgb((int)(percent * 255), Color.Yellow);
                    e.Graphics.FillRectangle(new SolidBrush(c2), shiftX + keyWidth * w, shiftY, keyWidth, keyHeight);
                    e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * w, shiftY, keyWidth, keyHeight);
                }
            }

            // draw black keys
            for (int i = 0; i < 7 * numOctaves; i++)
            {
                int j = i % 7;
                if(j == 1 || j == 2 || j == 4 || j == 5 || j == 6)
                    e.Graphics.FillRectangle(Brushes.Black, shiftX + keyWidth * i - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
            }

            // draw highlighted black key
            if (percent < 1E-5)
            {
                if (IsBlackNote(floorNote))
                {
                    e.Graphics.FillRectangle(Brushes.Yellow, shiftX + keyWidth * w - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
                    e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * w - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
                }
            }
            else
            {
                if (IsBlackNote(floorNote))
                {
                    Color c1 = Color.FromArgb((int)((1 - percent) * 255), Color.Yellow);
                    e.Graphics.FillRectangle(new SolidBrush(c1), shiftX + keyWidth * w - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
                    e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * w - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
                }
                else if(k != 4 && k != 11)
                {
                    Color c2 = Color.FromArgb((int)(percent * 255), Color.Yellow);
                    e.Graphics.FillRectangle(new SolidBrush(c2), shiftX + keyWidth * (w + 1) - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
                    e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * (w + 1) - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
                }
            }
        }

        // converts the chromatic index to corresponding white key index
        private int WhiteIndex(int num)
        {
            switch (num)
            {
                default:
                case 0: return 0;
                case 1: return 1;
                case 2: return 1;
                case 3: return 2;
                case 4: return 2;
                case 5: return 3;
                case 6: return 4;
                case 7: return 4;
                case 8: return 5;
                case 9: return 5;
                case 10: return 6;
                case 11: return 6;
            }
        }

        private bool IsBlackNote(int num)
        {
            switch (num % 12)
            {
                case 1: case 3: case 6: case 8: case 10: return true;
                default: return false;
            }
        }

        private void SoundChooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            sound.Layers.Clear();
            OrganSettingsPanel.Visible = false;
            soundType = SoundChooser.SelectedIndex;
            switch (SoundChooser.SelectedIndex)
            {
                case 0: // Sine Wave
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, frequency, 1.0f);
                    break;
                case 1: // Square Wave
                    sound.AddTone(Harmonic.WaveType.Square, SmoothTransitionsCheckbox.Checked, frequency, 1.0f);
                    break;
                case 2: // Triangle Wave
                    sound.AddTone(Harmonic.WaveType.Triangle, SmoothTransitionsCheckbox.Checked, frequency, 1.0f);
                    break;
                case 3: // Sawtooth Wave
                    sound.AddTone(Harmonic.WaveType.Sawtooth, SmoothTransitionsCheckbox.Checked, frequency, 1.0f);
                    break;
                case 4: // Flute
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, frequency, 1.0f);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 2.0f * frequency, 0.1f);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 3.0f * frequency, 0.4f);
                    break;
                case 5: // Organ
                    OrganSettingsPanel.Visible = true;
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 0.5f * frequency, 0.1f * OrganBar1.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, (3.0f / 2.0f) * frequency, 0.1f * OrganBar2.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 1.0f * frequency, 0.1f * OrganBar3.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 2.0f * frequency, 0.1f * OrganBar4.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 3.0f * frequency, 0.1f * OrganBar5.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 4.0f * frequency, 0.1f * OrganBar6.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 5.0f * frequency, 0.1f * OrganBar7.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 6.0f * frequency, 0.1f * OrganBar8.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, SmoothTransitionsCheckbox.Checked, 8.0f * frequency, 0.1f * OrganBar9.Value);
                    break;
            }
        }

        private void PresetChooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = PresetChooser.SelectedIndex;
            string settings = ORGAN_PRESETS[index, 1];
            OrganBar1.Value = int.Parse(settings.Substring(0, 1));
            OrganBar2.Value = int.Parse(settings.Substring(1, 1));
            OrganBar3.Value = int.Parse(settings.Substring(3, 1));
            OrganBar4.Value = int.Parse(settings.Substring(4, 1));
            OrganBar5.Value = int.Parse(settings.Substring(5, 1));
            OrganBar6.Value = int.Parse(settings.Substring(6, 1));
            OrganBar7.Value = int.Parse(settings.Substring(8, 1));
            OrganBar8.Value = int.Parse(settings.Substring(9, 1));
            OrganBar9.Value = int.Parse(settings.Substring(10, 1));
        }

        private void OrganBar_ValueChanged(object sender, EventArgs e)
        {
            if(SoundChooser.SelectedIndex == 5) // Organ selected
            {
                sound.Layers[(int)((TrackBar)sender).Tag].Amplitude = 0.1f * ((TrackBar)sender).Value;
            }
        }

        private void TremeloAmplitudeSlider_Scroll(object sender, EventArgs e)
        {
            sound.TremeloAmplitude = ((float)TremeloAmplitudeSlider.Value / 200.0f);
        }

        private void TremeloFrequencySlider_Scroll(object sender, EventArgs e)
        {
            sound.Tremelo.Frequency = TremeloFrequencySlider.Value / 3;
        }

        private void MusicDemo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                int numOctavesUpper = numOctaves - Math.Min((int)Math.Floor(numOctaves / 2.0), 2);
                float maxNote = 60 + numOctavesUpper * 12 - 1;
                if (!allowPartials)
                {
                    float midi = FrequencyToMidi(frequency);
                    float newMidi = RoundToScale(midi + 1);
                    if (midi == newMidi) newMidi = RoundToScale(midi + 2);
                    if (newMidi > maxNote) newMidi = maxNote;
                    frequency = MidiToFrequency(newMidi);
                }
                else
                {
                    frequency = frequency * 1.01f;
                    float midi= FrequencyToMidi(frequency);
                    if (midi > maxNote) { midi = maxNote; frequency = MidiToFrequency(midi); }
                }
                SetTone(frequency, amplitude);
                FrequencyLabel.Text = frequency.ToString("0") + " Hz";
            }
            else if (e.KeyCode == Keys.Left)
            {
                float numOctavesLower = Math.Min((int)Math.Floor(numOctaves / 2.0), 2);
                float minNote = 60 - numOctavesLower * 12;
                if (!allowPartials)
                {
                    float midi = FrequencyToMidi(frequency);
                    float newMidi = RoundToScale(midi - 1);
                    if (midi == newMidi) newMidi = RoundToScale(midi - 2);
                    if (newMidi < minNote) newMidi = minNote;
                    frequency = MidiToFrequency(newMidi);
                }
                else
                {
                    frequency = frequency * 0.99f;
                    float midi = FrequencyToMidi(frequency);
                    if (midi < minNote) { midi = minNote; frequency = MidiToFrequency(midi); }
                }
                SetTone(frequency, amplitude);
                FrequencyLabel.Text = frequency.ToString("0") + " Hz";
            }
            else if (e.KeyCode == Keys.Up)
            {
                amplitude += 0.01f;
                if (amplitude > 1) amplitude = 1;
                SetTone(frequency, amplitude);
            }
            else if (e.KeyCode == Keys.Down)
            {
                amplitude -= 0.01f;
                if (amplitude < 0) amplitude = 0;
                SetTone(frequency, amplitude);
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                sound.TremeloAmplitude += 0.01f;
                if (sound.TremeloAmplitude > 1) sound.TremeloAmplitude = 1;
                TremeloAmplitudeSlider.Value = (int)(sound.TremeloAmplitude * 200);
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                sound.TremeloAmplitude -= 0.01f;
                if (sound.TremeloAmplitude < 0) sound.TremeloAmplitude = 0;
                TremeloAmplitudeSlider.Value = (int)(sound.TremeloAmplitude * 200);
            }
            else if (e.KeyCode == Keys.Home)
            {
                sound.Tremelo.Frequency *= 1.01f;
                TremeloFrequencySlider.Value = (int)(sound.Tremelo.Frequency * 3);
            }
            else if (e.KeyCode == Keys.End)
            {
                sound.Tremelo.Frequency *= 0.99f;
                TremeloFrequencySlider.Value = (int)(sound.Tremelo.Frequency * 3);
            }
        }

        private void SetTone(float frequency, float amplitude)
        {
            if (sound.Layers.Count == 0) return;

            switch (soundType)
            {
                case 0: // Sine Wave
                case 1: // Square Wave
                case 2: // Triangle Wave
                case 3: // Sawtooth Wave
                    sound.Layers[0].Frequency = frequency;
                    break;
                case 4: // Flute
                    sound.Layers[0].Frequency = frequency;
                    sound.Layers[1].Frequency = 2.0f * frequency;
                    sound.Layers[2].Frequency = 3.0f * frequency;
                    break;
                case 5: // Organ
                    sound.Layers[0].Frequency = 0.5f * frequency;
                    sound.Layers[1].Frequency = (3.0f / 2.0f) * frequency;
                    sound.Layers[2].Frequency = 1.0f * frequency;
                    sound.Layers[3].Frequency = 2.0f * frequency;
                    sound.Layers[4].Frequency = 3.0f * frequency;
                    sound.Layers[5].Frequency = 4.0f * frequency;
                    sound.Layers[6].Frequency = 5.0f * frequency;
                    sound.Layers[7].Frequency = 6.0f * frequency;
                    sound.Layers[8].Frequency = 8.0f * frequency;
                    break;
            }

            if (playingSong) sound.ForceAmplitude = amplitude;
            else sound.Amplitude = amplitude;
            //waveOut.Volume = amplitude;
        }

        internal class Harmonic
        {
            public enum WaveType { Sine, Square, Triangle, Sawtooth };

            public WaveType Type = WaveType.Sine;
            public SineWave Sine;
            public SquareWave Square;
            public TriangleWave Triangle;
            public SawtoothWave Sawtooth;
            public float Amplitude;

            public float Frequency
            {
                get
                {
                    switch (Type)
                    {
                        default:
                        case WaveType.Sine: return Sine.Frequency;
                        case WaveType.Square: return Square.Frequency;
                        case WaveType.Triangle: return Triangle.Frequency;
                        case WaveType.Sawtooth: return Sawtooth.Frequency;
                    }
                }
                set
                {
                    Sine.Frequency = value;
                    Square.Frequency = value;
                    Triangle.Frequency = value;
                    Sawtooth.Frequency = value;
                }
            }

            public float NextValue
            {
                get
                {
                    switch (Type)
                    {
                        default:
                        case WaveType.Sine: return Sine.NextValue;
                        case WaveType.Square: return Square.NextValue;
                        case WaveType.Triangle: return Triangle.NextValue;
                        case WaveType.Sawtooth: return Sawtooth.NextValue;
                    }
                }
            }

            public Harmonic(CustomSoundProvider provider)
            {
                Sine = new SineWave(provider);
                Square = new SquareWave(provider);
                Triangle = new TriangleWave(provider);
                Sawtooth = new SawtoothWave(provider);
                Amplitude = 0.25f;
            }
        }

        // SineWaveProvider class modified from NAudio tutorial by Mark Heath: http://mark-dot-net.blogspot.com/2009/10/playback-of-sine-wave-in-naudio.html
        internal class CustomSoundProvider : NAudio.Wave.WaveProvider32
        {
            public List<Harmonic> Layers;
            public SineWave Tremelo;
            private float amplitude;
            private float prevAmplitude;
            private float newAmplitude;
            private float amplitudePercent;
            public float TremeloAmplitude;

            public float ForceAmplitude
            {
                set
                {
                    amplitude = value;
                    prevAmplitude = value;
                    newAmplitude = value;
                    amplitudePercent = 1.0f;
                }
            }

            public float Amplitude 
            { 
                get { return newAmplitude; } 
                set 
                {
                    prevAmplitude = amplitude;
                    newAmplitude = value;
                    amplitudePercent = 0;
                } 
            }

            public CustomSoundProvider()
            {
                // initialize variables to reasonable values
                Layers = new List<Harmonic>();
                Tremelo = new SineWave(this);
                Tremelo.Frequency = 10;
                TremeloAmplitude = 0;
                amplitude = 0;
            }

            public void AddTone(Harmonic.WaveType type, bool smooth, float frequency, float amplitude)
            {
                Harmonic tone = new Harmonic(this);
                tone.Type = type;
                tone.Sine.SmoothTransition = smooth;
                tone.Frequency = frequency;
                tone.Amplitude = amplitude;
                Layers.Add(tone);
            }

            public override int Read(float[] buffer, int offset, int sampleCount)
            {
                float normalizer = 0.0f;
                foreach (Harmonic sound in Layers) normalizer += sound.Amplitude;
                float[] bufferCopy = new float[buffer.Length];
                for (int n = 0; n < sampleCount; n++)
                {
                    buffer[n + offset] = 0;
                    bufferCopy[n + offset] = 0;
                    float t = Tremelo.NextValue;
                    if (amplitudePercent < 1)
                    {
                        amplitudePercent += 0.0005f;
                        amplitude = prevAmplitude + (newAmplitude - prevAmplitude) * amplitudePercent;
                    }
                    foreach (Harmonic sound in Layers)
                    {
                        float v = sound.NextValue;
                        buffer[n + offset] += (amplitude + TremeloAmplitude * t) * sound.Amplitude * v * (1.0f / normalizer);
                        bufferCopy[n + offset] = buffer[n + offset];
                    }
                }
                OnDataReady(bufferCopy, offset, sampleCount);
                return sampleCount;
            }

            public delegate void DataReadyDelegate(float[] buffer, int offset, int sampleCount);
            public event DataReadyDelegate DataReady;
            private void OnDataReady(float[] buffer, int offset, int sampleCount) { if (DataReady != null) DataReady(buffer, offset, sampleCount); }
        }

        // Phase tracking and smooth transitions inspired by user Paul R on StackExchange: http://dsp.stackexchange.com/questions/971/how-to-create-a-sine-wave-generator-that-can-smoothly-transition-between-frequen
        internal class SineWave
        {
            // Wave lookup table
            const int TABLE_SIZE = 1024;
            public float[] Table;

            float phase_index;
            float phase_delta;
            float new_phase_delta;
            float delta_percent;
            float frequency;
            private NAudio.Wave.WaveProvider32 provider;

            public bool SmoothTransition;

            public float Frequency
            {
                get { return frequency; }
                set
                {
                    frequency = value;

                    if (SmoothTransition)
                    {
                        // fix the phase delta value to its current position
                        phase_delta = phase_delta + (new_phase_delta - phase_delta) * delta_percent;

                        // start a smooth transition between frequencies
                        delta_percent = 0;
                        new_phase_delta = (float)TABLE_SIZE * frequency / (float)provider.WaveFormat.SampleRate;
                    }
                    else
                    {
                        delta_percent = 1;
                        new_phase_delta = (float)TABLE_SIZE * frequency / (float)provider.WaveFormat.SampleRate;
                        phase_delta = new_phase_delta;
                    }
                }
            }

            public SineWave(NAudio.Wave.WaveProvider32 provider)
            {
                // build a wave lookup tables for efficient calculations
                Table = new float[TABLE_SIZE];
                for (int i = 0; i < TABLE_SIZE; i++)
                {
                    Table[i] = (float)Math.Sin(i * Math.PI * 2.0 / (double)TABLE_SIZE);
                }

                this.provider = provider;

                // initialize variables to reasonable values
                frequency = 1000;
                phase_delta = (float)TABLE_SIZE * 1000 / (float)provider.WaveFormat.SampleRate;
                delta_percent = 1;
                SmoothTransition = true;
            }

            public float NextValue
            {
                get
                {
                    // calculate the current integer phase index and loop the phase tracker
                    int sample = (int)phase_index % TABLE_SIZE;
                    if (phase_index >= TABLE_SIZE) { phase_index -= TABLE_SIZE; }

                    // look up the tone value and interpolate for improved sound quality
                    float percent = (phase_index % TABLE_SIZE) - sample;
                    float v1 = Table[sample];
                    float v2 = Table[(sample + 1) % TABLE_SIZE];
                    float v = v1 * (1 - percent) + v2 * percent;

                    // continue smooth transition between frequencies if in progress
                    float delta = phase_delta;
                    if (delta_percent < 1)
                    {
                        delta_percent += 0.0005f;
                        delta = phase_delta + (new_phase_delta - phase_delta) * delta_percent;
                        if (delta_percent >= 1)
                            phase_delta = new_phase_delta;
                    }

                    // update phase position
                    phase_index += delta;

                    return v;
                }
            }
        }

        internal class SquareWave
        {
            // Wave lookup table
            const int TABLE_SIZE = 1024;
            public float[] Table;

            float phase_index;
            float phase_delta;
            float new_phase_delta;
            float delta_percent;
            float frequency;
            private NAudio.Wave.WaveProvider32 provider;

            public bool SmoothTransition;

            public float Frequency
            {
                get { return frequency; }
                set
                {
                    frequency = value;

                    if (SmoothTransition)
                    {
                        // fix the phase delta value to its current position
                        phase_delta = phase_delta + (new_phase_delta - phase_delta) * delta_percent;

                        // start a smooth transition between frequencies
                        delta_percent = 0;
                        new_phase_delta = (float)TABLE_SIZE * frequency / (float)provider.WaveFormat.SampleRate;
                    }
                    else
                    {
                        delta_percent = 1;
                        new_phase_delta = (float)TABLE_SIZE * frequency / (float)provider.WaveFormat.SampleRate;
                        phase_delta = new_phase_delta;
                    }
                }
            }

            public SquareWave(NAudio.Wave.WaveProvider32 provider)
            {
                // build a wave lookup tables for efficient calculations
                Table = new float[TABLE_SIZE];
                for (int i = 0; i < TABLE_SIZE; i++)
                {
                    Table[i] = i < TABLE_SIZE / 2 ? 1 : -1;
                }

                this.provider = provider;

                // initialize variables to reasonable values
                frequency = 1000;
                phase_delta = (float)TABLE_SIZE * 1000 / (float)provider.WaveFormat.SampleRate;
                delta_percent = 1;
                SmoothTransition = false;
            }

            public float NextValue
            {
                get
                {
                    // calculate the current integer phase index and loop the phase tracker
                    int sample = (int)phase_index % TABLE_SIZE;
                    if (phase_index >= TABLE_SIZE) { phase_index -= TABLE_SIZE; }

                    // look up the tone value and interpolate for improved sound quality
                    float percent = (phase_index % TABLE_SIZE) - sample;
                    float v1 = Table[sample];
                    float v2 = Table[(sample + 1) % TABLE_SIZE];
                    float v = v1 * (1 - percent) + v2 * percent;

                    // continue smooth transition between frequencies if in progress
                    float delta = phase_delta;
                    if (delta_percent < 1)
                    {
                        delta_percent += 0.0005f;
                        delta = phase_delta + (new_phase_delta - phase_delta) * delta_percent;
                        if (delta_percent >= 1)
                            phase_delta = new_phase_delta;
                    }

                    // update phase position
                    phase_index += delta;

                    return v;
                }
            }
        }

        internal class SawtoothWave
        {
            // Wave lookup table
            const int TABLE_SIZE = 1024;
            public float[] Table;

            float phase_index;
            float phase_delta;
            float new_phase_delta;
            float delta_percent;
            float frequency;
            private NAudio.Wave.WaveProvider32 provider;

            public bool SmoothTransition;

            public float Frequency
            {
                get { return frequency; }
                set
                {
                    frequency = value;

                    if (SmoothTransition)
                    {
                        // fix the phase delta value to its current position
                        phase_delta = phase_delta + (new_phase_delta - phase_delta) * delta_percent;

                        // start a smooth transition between frequencies
                        delta_percent = 0;
                        new_phase_delta = (float)TABLE_SIZE * frequency / (float)provider.WaveFormat.SampleRate;
                    }
                    else
                    {
                        delta_percent = 1;
                        new_phase_delta = (float)TABLE_SIZE * frequency / (float)provider.WaveFormat.SampleRate;
                        phase_delta = new_phase_delta;
                    }
                }
            }

            public SawtoothWave(NAudio.Wave.WaveProvider32 provider)
            {
                // build a wave lookup tables for efficient calculations
                Table = new float[TABLE_SIZE];
                for (int i = 0; i < TABLE_SIZE; i++)
                {
                    Table[i] = 1 - 2 * (float)i / (float)TABLE_SIZE;
                }

                this.provider = provider;

                // initialize variables to reasonable values
                frequency = 1000;
                phase_delta = (float)TABLE_SIZE * 1000 / (float)provider.WaveFormat.SampleRate;
                delta_percent = 1;
                SmoothTransition = false;
            }

            public float NextValue
            {
                get
                {
                    // calculate the current integer phase index and loop the phase tracker
                    int sample = (int)phase_index % TABLE_SIZE;
                    if (phase_index >= TABLE_SIZE) { phase_index -= TABLE_SIZE; }

                    // look up the tone value and interpolate for improved sound quality
                    float percent = (phase_index % TABLE_SIZE) - sample;
                    float v1 = Table[sample];
                    float v2 = Table[(sample + 1) % TABLE_SIZE];
                    float v = v1 * (1 - percent) + v2 * percent;

                    // continue smooth transition between frequencies if in progress
                    float delta = phase_delta;
                    if (delta_percent < 1)
                    {
                        delta_percent += 0.0005f;
                        delta = phase_delta + (new_phase_delta - phase_delta) * delta_percent;
                        if (delta_percent >= 1)
                            phase_delta = new_phase_delta;
                    }

                    // update phase position
                    phase_index += delta;

                    return v;
                }
            }
        }

        internal class TriangleWave
        {
            // Wave lookup table
            const int TABLE_SIZE = 1024;
            public float[] Table;

            float phase_index;
            float phase_delta;
            float new_phase_delta;
            float delta_percent;
            float frequency;
            private NAudio.Wave.WaveProvider32 provider;

            public bool SmoothTransition;

            public float Frequency
            {
                get { return frequency; }
                set
                {
                    frequency = value;

                    if (SmoothTransition)
                    {
                        // fix the phase delta value to its current position
                        phase_delta = phase_delta + (new_phase_delta - phase_delta) * delta_percent;

                        // start a smooth transition between frequencies
                        delta_percent = 0;
                        new_phase_delta = (float)TABLE_SIZE * frequency / (float)provider.WaveFormat.SampleRate;
                    }
                    else
                    {
                        delta_percent = 1;
                        new_phase_delta = (float)TABLE_SIZE * frequency / (float)provider.WaveFormat.SampleRate;
                        phase_delta = new_phase_delta;
                    }
                }
            }

            public TriangleWave(NAudio.Wave.WaveProvider32 provider)
            {
                // build a wave lookup tables for efficient calculations
                Table = new float[TABLE_SIZE];
                for (int i = 0; i < TABLE_SIZE; i++)
                {
                    Table[i] = i < TABLE_SIZE / 2 ? 1.0f - 2.0f * i / (TABLE_SIZE / 2.0f) : -1.0f + 2.0f * (i - TABLE_SIZE / 2.0f) / (TABLE_SIZE / 2.0f);
                }

                this.provider = provider;

                // initialize variables to reasonable values
                frequency = 1000;
                phase_delta = (float)TABLE_SIZE * 1000 / (float)provider.WaveFormat.SampleRate;
                delta_percent = 1;
                SmoothTransition = false;
            }

            public float NextValue
            {
                get
                {
                    // calculate the current integer phase index and loop the phase tracker
                    int sample = (int)phase_index % TABLE_SIZE;
                    if (phase_index >= TABLE_SIZE) { phase_index -= TABLE_SIZE; }

                    // look up the tone value and interpolate for improved sound quality
                    float percent = (phase_index % TABLE_SIZE) - sample;
                    float v1 = Table[sample];
                    float v2 = Table[(sample + 1) % TABLE_SIZE];
                    float v = v1 * (1 - percent) + v2 * percent;

                    // continue smooth transition between frequencies if in progress
                    float delta = phase_delta;
                    if (delta_percent < 1)
                    {
                        delta_percent += 0.0005f;
                        delta = phase_delta + (new_phase_delta - phase_delta) * delta_percent;
                        if (delta_percent >= 1)
                            phase_delta = new_phase_delta;
                    }

                    // update phase position
                    phase_index += delta;

                    return v;
                }
            }
        }

        internal class Song
        {
            private List<Note> notes;
            private int index = -1;
            private Stopwatch timer;

            public Song(string songString)
            {
                notes = new List<Note>();
                timer = new Stopwatch();

                string[] songinfo = songString.Split('|');
                int bpm = int.Parse(songinfo[0]);
                double beatlength = (1000.0 * 60.0) / (double)bpm;
                int noteType = int.Parse(songinfo[1]);
                double noteLength = beatlength * (1.0 / (double)noteType) * 4.0;
                double cumulative = 0;

                MatchCollection matches = Regex.Matches(songinfo[2].Replace("/", ""), "([A-G][#b]?[1-9]-*)|(\\.*)");
                foreach (Match match in matches)
                {
                    string noteString = match.Value;
                    if (noteString.StartsWith(".")) // rest
                    {
                        double duration = noteString.Length * noteLength;
                        Note note = new Note(55, cumulative, duration, 0);
                        cumulative += duration;
                        notes.Add(note);
                    }
                    else if(noteString.Length > 0) // note
                    {
                        int beats = 1;
                        while (noteString.EndsWith("-")) { beats++; noteString = noteString.Substring(0, noteString.Length - 1); }
                        double duration = beats * noteLength;

                        int octave = int.Parse(noteString.Substring(noteString.Length - 1));
                        int noteIndex = Note.NoteToIndex(noteString.Substring(0, noteString.Length - 1));
                        int midi = 12 * (octave + 1) + noteIndex;
                        float frequency = MidiToFrequency(midi);

                        Note note = new Note(frequency, cumulative, duration, 1.0);
                        cumulative += duration;
                        notes.Add(note);
                    }
                }
            }

            public void Reset()
            {
                index = -1;
            }

            public Note Next
            {
                get
                {
                    if (index >= notes.Count) return null;

                    if (index < 0) { timer.Restart(); index = 0; }
                    
                    DateTime curr = DateTime.Now;
                    double elapsed = timer.ElapsedMilliseconds;

                    //Console.WriteLine(elapsed / 150);

                    Note currNote = notes[index];
                    while (currNote != null && elapsed > currNote.cumulative + currNote.duration)
                    {
                        index++;
                        currNote = index < notes.Count ? notes[index] : null;
                    }
                    if (index >= notes.Count) return null;

                    return currNote;
                }
            }

            internal class Note
            {
                public static int NoteToIndex(string note)
                {
                    switch (note)
                    {
                        default:
                        case "B#":
                        case "C": return 0;
                        case "C#":
                        case "Db": return 1;
                        case "D": return 2;
                        case "D#":
                        case "Eb": return 3;
                        case "E":
                        case "Fb": return 4;
                        case "F":
                        case "E#": return 5;
                        case "F#":
                        case "Gb": return 6;
                        case "G": return 7;
                        case "G#":
                        case "Ab": return 8;
                        case "A": return 9;
                        case "A#":
                        case "Bb": return 10;
                        case "B":
                        case "Cb": return 11;
                    }
                }

                public double frequency;
                public double duration;
                public double amplitude;
                public double cumulative;

                public Note(double frequency, double cumulative, double duration, double amplitude)
                {
                    this.frequency = frequency;
                    this.cumulative = cumulative;
                    this.duration = duration;
                    this.amplitude = amplitude;
                }
            }
        }
    }
}
