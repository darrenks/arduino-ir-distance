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
        CustomSoundProvider sound;
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

            sound = new CustomSoundProvider();
            sound.SetWaveFormat(16000, 1);
            sound.AddTone(Harmonic.WaveType.Sine, 440, 0.25f, 10, 0);

            waveOut = new NAudio.Wave.WaveOut();
            waveOut.Init(sound);
            waveOut.Play();
        }

        void sensors_DistancesChanged(double dist1, double dist2)
        {
            // normalize distance
            float x = (float)dist1 / 1024;

            // convert to frequency on a logarithmic scale (constants selected by trial and error)
            int frequency = (int)(-110 + 440 * Math.Exp(x));
            //if (FixNotesCheckbox.Checked)
            //{
            //    // conversion formula from user agargara on processing.org: http://processing.org/discourse/beta/num_1241052082.html
            //    float pitch = (float)Math.Round(69 + 12 * (Math.Log(frequency / 440.0) / Math.Log(2.0)));
            //    pitch = 440 * (float)Math.Pow(2, (pitch - 69) / 12); // Convert back
            //    frequency = (int)pitch;
            //}
            sound.Layers[0].Frequency = frequency;
        }

        private void MusicDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // clean up
            waveOut.Dispose();
            sensors.Disconnect();
        }

        private void SineRadio_CheckedChanged(object sender, EventArgs e)
        {
            //sound.Type = CustomSoundProvider.WaveType.Sine;
        }

        private void SquareRadio_CheckedChanged(object sender, EventArgs e)
        {
            //sound.Type = CustomSoundProvider.WaveType.Square;
        }

        private void TriangleRadio_CheckedChanged(object sender, EventArgs e)
        {
            //sound.Type = CustomSoundProvider.WaveType.Triangle;
        }

        private void SmoothCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //tone.Sine.SmoothTransition = SmoothCheckbox.Checked;
        }

        private void MusicDemo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                sound.Layers[0].Frequency *= 1.01f;
            }
            else if (e.KeyCode == Keys.Down)
            {
                sound.Layers[0].Frequency *= 0.99f;
            }
        }

        internal class Harmonic
        {
            public enum WaveType { Sine, Square, Triangle };

            public WaveType Type = WaveType.Sine;
            public SineWave Sine;
            public SquareWave Square;
            public TriangleWave Triangle;
            public SineWave Tremelo;
            public float Amplitude;
            public float TremeloAmplitude;

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
                    }
                }
                set
                {
                    Sine.Frequency = value;
                    Square.Frequency = value;
                    Triangle.Frequency = value;
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
                    }
                }
            }

            public Harmonic(CustomSoundProvider provider)
            {
                Sine = new SineWave(provider);
                Square = new SquareWave(provider);
                Triangle = new TriangleWave(provider);
                Tremelo = new SineWave(provider);
                Amplitude = 0.25f;
                TremeloAmplitude = 0;
            }
        }

        // SineWaveProvider class modified from NAudio tutorial by Mark Heath: http://mark-dot-net.blogspot.com/2009/10/playback-of-sine-wave-in-naudio.html
        internal class CustomSoundProvider : NAudio.Wave.WaveProvider32
        {
            public List<Harmonic> Layers;

            public CustomSoundProvider()
            {
                // initialize variables to reasonable values
                Layers = new List<Harmonic>();
            }

            public void AddTone(Harmonic.WaveType type, float frequency, float amplitude, float tremeloFrequency, float tremeloAmplitude)
            {
                Harmonic tone = new Harmonic(this);
                tone.Type = type;
                tone.Frequency = frequency;
                tone.Amplitude = amplitude;
                tone.Tremelo.Frequency = tremeloFrequency;
                tone.TremeloAmplitude = tremeloAmplitude;
                Layers.Add(tone);
            }

            public override int Read(float[] buffer, int offset, int sampleCount)
            {
                for (int n = 0; n < sampleCount; n++)
                {
                    buffer[n + offset] = 0;
                    foreach (Harmonic sound in Layers)
                    {
                        float v = sound.NextValue;
                        float t = sound.Tremelo.NextValue;
                        buffer[n + offset] += (sound.Amplitude + sound.TremeloAmplitude * t) * v;
                    }
                }
                return sampleCount;
            }
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
    }
}
