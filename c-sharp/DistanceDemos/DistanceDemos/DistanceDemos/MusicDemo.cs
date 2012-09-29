using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NAudio.Wave;

namespace DistanceDemos
{
    public partial class MusicDemo : Form
    {
        private string[,] ORGAN_PRESETS = {{"Standard", "88 8888 888"},
                                           {"Jimmy Smith", "88 8000 000"},
                                           {"Gospel", "88 8000 008"},
                                           {"Stopped Flute", "00 5320 000"},
                                           {"Dulciana", "00 4432 000"},
                                           {"French Horn", "00 8740 000"},
                                           {"Salacional", "00 4544 222"},
                                           {"Flutes 8' & 4'", "00 5403 000"},
                                           {"Oboe Horn", "00 4675 300"},
                                           {"Swell Diapason", "00 5644 320"},
                                           {"Trumpet", "00 6876 540"},
                                           {"Full Swell", "32 7645 222"},
                                           {"Cello", "00 4545 440"},
                                           {"Flute & String", "00 4423 220"},
                                           {"Clarinet", "00 7373 430"},
                                           {"Diapason, Gamba & Flute", "00 4544 220"},
                                           {"Great, no reeds", "00 6644 322"},
                                           {"Open Diapason", "00 5642 200"},
                                           {"Full Great", "00 6845 433"},
                                           {"Tibia Clausa", "00 8030 000"},
                                           {"Full Great with 16'", "42 7866 244"}};

        private WaveOut waveOut;
        private CustomSoundProvider sound;
        private DistanceSensors sensors;
        private List<float> waveForm;
        private float frequency, amplitude;

        public MusicDemo()
        {
            InitializeComponent();
        }

        private void MusicDemo_Load(object sender, EventArgs e)
        {
            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();

            waveForm = new List<float>();

            sound = new CustomSoundProvider();
            sound.SetWaveFormat(16000, 1);
            sound.DataReady += new CustomSoundProvider.DataReadyDelegate(sound_DataReady);
            frequency = 440;
            amplitude = 0.25f;
            SetTone(frequency, amplitude);
            SoundChooser.SelectedIndex = 0;

            waveOut = new NAudio.Wave.WaveOut();
            waveOut.Init(sound);
            waveOut.Play();

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
        }

        void sound_DataReady(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i++) waveForm.Add(buffer[offset + i]);
            int toRemove = waveForm.Count - 10 * 16000;
            if (toRemove > 0) waveForm.RemoveRange(0, toRemove);
            DisplayPanel.Refresh();
            Piano.Refresh();
        }

        void sensors_DistancesChanged(double[] dists)
        {
            // normalize distance
            float x = (float)dists[0] / 1024;
            float y = (float)dists[1] / 1024;
            float z = (float)dists[2] / 1024;
            //float w = 2 * (float)dists[3] / 1024;

            // convert to frequency on a logarithmic scale (constants selected by trial and error)
            frequency = -110 + 440 * (float)Math.Exp(x);
            if (FixNotesCheckbox.Checked) frequency = FixNote(frequency);
            amplitude = y / 2.0f;
            SetTone(frequency, amplitude);
            sound.TremeloAmplitude = z;

            FrequencyLabel.Text = frequency.ToString("0") + " Hz";
        }

        private float FixNote(float frequency)
        {
            // conversion formula from user agargara on processing.org: http://processing.org/discourse/beta/num_1241052082.html
            float pitch = (float)Math.Round(69 + 12 * (Math.Log(frequency / 440.0) / Math.Log(2.0)));
            pitch = 440 * (float)Math.Pow(2, (pitch - 69) / 12); // Convert back
            frequency = (int)pitch;
            return frequency;
        }

        private float GetNote(float frequency)
        {
            float note = 69 + 12 * (float)(Math.Log(frequency / 440.0) / Math.Log(2.0));
            return note;
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
            int numOctaves = 6;
            int keyWidth = 15;
            int keyHeight = 60;
            int blackKeyWidth = 12;
            int blackKeyHeight = 40;
            int shiftX = (Piano.Width - keyWidth * numOctaves * 7) / 2;
            int shiftY = (Piano.Height - keyHeight) / 2;

            float currNote = GetNote(frequency);
            int floorNote = (int)Math.Round(currNote) - (69 - 36);
            float percent = currNote - (69 - 36) - floorNote;
            
            for (int i = 0; i < 7 * numOctaves; i++) // draw white keys
            {
                e.Graphics.FillRectangle(Brushes.White, shiftX + keyWidth * i, shiftY, keyWidth, keyHeight);
                e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * i, shiftY, keyWidth, keyHeight);
            }
            if (!IsBlackNote(floorNote))
            {
                int octave = (int)(floorNote / 12);
                int k = floorNote % 12;
                switch (k)
                {
                    default: case 0: break;
                    case 2: k -= 1; break;
                    case 4: case 5: k -= 2; break;
                    case 7: k -= 3; break;
                    case 9: k -= 4; break;
                    case 11: k -= 5; break;
                }
                int i = octave * 7 + k;
                if (Math.Abs(percent) < 1) // integer note
                {
                    e.Graphics.FillRectangle(Brushes.Yellow, shiftX + keyWidth * i, shiftY, keyWidth, keyHeight);
                    e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * i, shiftY, keyWidth, keyHeight);
                }
                else
                {
                    e.Graphics.DrawLine(Pens.Red, shiftX + keyWidth * i + percent * keyWidth, shiftY + 1, shiftX + keyWidth * i + percent * keyWidth, shiftY + keyHeight - 1);
                }
            }
            for (int i = 0; i < 7 * numOctaves; i++) // draw black keys
            {
                int k = i % 7;
                if(k == 1 || k == 2 || k == 4 || k == 5 || k == 6)
                    e.Graphics.FillRectangle(Brushes.Black, shiftX + keyWidth * i - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
            }
            if (IsBlackNote(floorNote))
            {
                int octave = (int)(floorNote / 12);
                int k = floorNote % 12;
                switch (k)
                {
                    default:
                    case 1: break;
                    case 3: k -= 1; break;
                    case 6: k -= 2; break;
                    case 8: k -= 3; break;
                    case 10: k -= 4; break;
                }
                int i = octave * 7 + k;
                if (Math.Abs(percent) < 1) // integer note
                {
                    e.Graphics.FillRectangle(Brushes.Yellow, shiftX + keyWidth * i - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
                    e.Graphics.DrawRectangle(Pens.Black, shiftX + keyWidth * i - blackKeyWidth / 2, shiftY, blackKeyWidth, blackKeyHeight);
                }
                else
                {
                    e.Graphics.DrawLine(Pens.Red, shiftX + keyWidth * i  - blackKeyWidth / 2 + percent * blackKeyWidth, shiftY + 1, shiftX + keyWidth * i - blackKeyWidth / 2 + percent * blackKeyWidth, shiftY + blackKeyHeight - 1);
                }
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
            switch (SoundChooser.SelectedIndex)
            {
                case 0: // Sine Wave
                    sound.AddTone(Harmonic.WaveType.Sine, true, frequency, 1.0f);
                    break;
                case 1: // Square Wave
                    sound.AddTone(Harmonic.WaveType.Square, false, frequency, 1.0f);
                    break;
                case 2: // Triangle Wave
                    sound.AddTone(Harmonic.WaveType.Triangle, false, frequency, 1.0f);
                    break;
                case 3: // Sawtooth Wave
                    sound.AddTone(Harmonic.WaveType.Sawtooth, false, frequency, 1.0f);
                    break;
                case 4: // Flute
                    sound.AddTone(Harmonic.WaveType.Sine, false, frequency, 1.0f);
                    sound.AddTone(Harmonic.WaveType.Sine, false, 2.0f * frequency, 0.1f);
                    sound.AddTone(Harmonic.WaveType.Sine, false, 3.0f * frequency, 0.4f);
                    break;
                case 5: // Organ
                    OrganSettingsPanel.Visible = true;
                    sound.AddTone(Harmonic.WaveType.Sine, false, 0.5f * frequency, 0.1f * OrganBar1.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, false, (3.0f / 2.0f) * frequency, 0.1f * OrganBar2.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, false, 1.0f * frequency, 0.1f * OrganBar3.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, false, 2.0f * frequency, 0.1f * OrganBar4.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, false, 3.0f * frequency, 0.1f * OrganBar5.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, false, 4.0f * frequency, 0.1f * OrganBar6.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, false, 5.0f * frequency, 0.1f * OrganBar7.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, false, 6.0f * frequency, 0.1f * OrganBar8.Value);
                    sound.AddTone(Harmonic.WaveType.Sine, false, 8.0f * frequency, 0.1f * OrganBar9.Value);
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
                frequency = frequency * (1.0f + 1.0f / 13.0f);
                if (FixNotesCheckbox.Checked) frequency = FixNote(frequency);
                if (frequency > 3322) frequency = 3322;
                SetTone(frequency, amplitude);
                FrequencyLabel.Text = frequency.ToString("0") + " Hz";
            }
            else if (e.KeyCode == Keys.Left)
            {
                frequency = frequency * (1.0f - 1.0f / 13.0f);
                if (FixNotesCheckbox.Checked) frequency = FixNote(frequency);
                if (frequency < 55) frequency = 55;
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
            switch (SoundChooser.SelectedIndex)
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

            sound.Amplitude = amplitude;
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
            public float Amplitude;
            public float TremeloAmplitude;


            public CustomSoundProvider()
            {
                // initialize variables to reasonable values
                Layers = new List<Harmonic>();
                Tremelo = new SineWave(this);
                Tremelo.Frequency = 10;
                TremeloAmplitude = 0;
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
                    foreach (Harmonic sound in Layers)
                    {
                        float v = sound.NextValue;
                        buffer[n + offset] += (Amplitude + TremeloAmplitude * t) * sound.Amplitude * v * (1.0f / normalizer);
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
    }
}
