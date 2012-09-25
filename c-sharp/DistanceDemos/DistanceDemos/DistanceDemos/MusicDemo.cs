using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DistanceDemos
{
    public partial class MusicDemo : Form
    {
        private NAudio.Wave.WaveOut waveOut;
        CustomSoundProvider tone;
        private DistanceSensors sensors;

        public MusicDemo()
        {
            InitializeComponent();
        }

        private void MusicDemo_Load(object sender, EventArgs e)
        {
            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();

            tone = new CustomSoundProvider();
            tone.SetWaveFormat(16000, 1);
            tone.Frequency = 440;
            tone.Amplitude = 0.25f;

            waveOut = new NAudio.Wave.WaveOut();
            waveOut.Init(tone);
            waveOut.Play();
        }

        void sensors_DistancesChanged(double dist1, double dist2)
        {
            // normalize distance
            float x = (float)dist1 / 1024;

            // convert to frequency on a logarithmic scale (constants selected by trial and error)
            int frequency = (int)(-110 + 440 * Math.Exp(x));
            if (FixNotesCheckbox.Checked)
            {
                // conversion formula from user agargara on processing.org: http://processing.org/discourse/beta/num_1241052082.html
                float pitch = (float)Math.Round(69 + 12 * (Math.Log(frequency / 440.0) / Math.Log(2.0)));
                pitch = 440 * (float)Math.Pow(2, (pitch - 69) / 12); // Convert back
                frequency = (int)pitch;
            }
            tone.Frequency = frequency;
        }

        private void MusicDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // clean up
            waveOut.Dispose();
            sensors.Disconnect();
        }

        private void SineRadio_CheckedChanged(object sender, EventArgs e)
        {
            tone.Type = CustomSoundProvider.WaveType.Sine;
        }

        private void SquareRadio_CheckedChanged(object sender, EventArgs e)
        {
            tone.Type = CustomSoundProvider.WaveType.Square;
        }

        private void TriangleRadio_CheckedChanged(object sender, EventArgs e)
        {
            tone.Type = CustomSoundProvider.WaveType.Triangle;
        }

        private void SmoothCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            tone.SmoothTransitions = SmoothCheckbox.Checked;
        }

        // SineWaveProvider class modified from NAudio tutorial by Mark Heath: http://mark-dot-net.blogspot.com/2009/10/playback-of-sine-wave-in-naudio.html
        // Phase tracking and smooth transitions inspired by user Paul R on StackExchange: http://dsp.stackexchange.com/questions/971/how-to-create-a-sine-wave-generator-that-can-smoothly-transition-between-frequen
        internal class CustomSoundProvider : NAudio.Wave.WaveProvider32
        {
            public enum WaveType { Sine, Square, Triangle };

            // Sine Wave lookup table
            const int TABLE_SIZE = 1024;
            float[] sineTable, squareTable, triangleTable;
            private float[] Table { get { if (Type == WaveType.Sine) return sineTable; else if (Type == WaveType.Square) return squareTable; else return triangleTable; } }

            float phase_index;
            float phase_delta;
            float new_phase_delta;
            float delta_percent;
            int frequency;

            public int Frequency { get { return frequency; } set 
            { 
                frequency = value;

                if (SmoothTransitions)
                {
                    // fix the phase delta value to its current position
                    phase_delta = phase_delta + (new_phase_delta - phase_delta) * delta_percent;

                    // start a smooth transition between frequencies
                    delta_percent = 0;
                    new_phase_delta = (float)TABLE_SIZE * frequency / (float)WaveFormat.SampleRate;
                }
                else
                {
                    delta_percent = 1;
                    new_phase_delta = (float)TABLE_SIZE * frequency / (float)WaveFormat.SampleRate;
                    phase_delta = new_phase_delta;
                }
            } }
            public float Amplitude;
            public WaveType Type;
            public bool SmoothTransitions;

            public CustomSoundProvider()
            {
                // build a wave lookup tables for efficient calculations
                sineTable = new float[TABLE_SIZE];
                squareTable = new float[TABLE_SIZE];
                triangleTable = new float[TABLE_SIZE];
                for (int i = 0; i < TABLE_SIZE; i++)
                {
                    sineTable[i] = (float)Math.Sin(i * Math.PI * 2.0 / (double)TABLE_SIZE);
                    squareTable[i] = i < TABLE_SIZE / 2 ? 1 : -1;
                    triangleTable[i] = 1 - 2.0f * i / (float)TABLE_SIZE;
                }

                // initialize variables to reasonable values
                frequency = 1000;
                Amplitude = 0.25f;
                phase_delta = (float)TABLE_SIZE * 1000 / (float)WaveFormat.SampleRate;
                delta_percent = 1;
                Type = WaveType.Sine;
                SmoothTransitions = true;
            }

            public override int Read(float[] buffer, int offset, int sampleCount)
            {
                int sampleRate = WaveFormat.SampleRate;
                for (int n = 0; n < sampleCount; n++)
                {
                    // calculate the current integer phase index and loop the phase tracker
                    int sample = (int)phase_index % TABLE_SIZE;
                    if (phase_index >= TABLE_SIZE) { phase_index -= TABLE_SIZE; }
                    
                    // lookup the tone value and interpolate for improved sound quality
                    float percent = (phase_index % TABLE_SIZE) - sample;
                    float v1 = Table[sample];
                    float v2 = Table[(sample + 1) % TABLE_SIZE];
                    float v = v1 * (1 - percent) + v2 * percent;
                    buffer[n + offset] = Amplitude * v;
                    
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
                }
                return sampleCount;
            }
        }
    }
}
