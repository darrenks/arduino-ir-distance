using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

using NAudio.Wave;
using System.Threading;
using System.Threading.Tasks;

namespace DistanceDemos
{
    public partial class DJDemo : Form
    {
        private DistanceSensors sensors;
        private float volume;
        
        private WaveOut music;
        private Mp3FileReader song;
        private WaveStream wavStream;
        private BlockAlignReductionStream baStream;

        public DJDemo()
        {
            InitializeComponent();
        }

        private void DJDemo_Load(object sender, EventArgs e)
        {
            volume = 0.0f;

            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();

            song = new Mp3FileReader("sounds/gangam style.mp3");
            wavStream = WaveFormatConversionStream.CreatePcmStream(song);
            baStream = new BlockAlignReductionStream(wavStream);
            music = new WaveOut(WaveCallbackInfo.FunctionCallback());
            music.Init(baStream);
            music.Volume = 0;
        }

        ~DJDemo()
        {
            
        }

        private void sensors_DistancesChanged(double[] dists)
        {
            float dist1 = (float)dists[0];
            float dist2 = (float)dists[1];
            
            // normalize distance
            float x = (dist1 - 5) / 30.0f;
            float y = (dist2 - 5) / 30.0f;

            // fix in [0, 1]
            if (x < 0) x = 0;
            else if (x > 1) x = 1;
            x = 1 - x;
            if (y < 0) y = 0;
            else if (y > 1) y = 1;
            y = 1 - y;

            volume = Math.Max(x, y);
            if (volume > 0)
                music.Play();

            music.Volume = volume;
            Invoke(new MethodInvoker(delegate { progressBar1.Value = (int)(volume * 100); }));
        }

        private void DJDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            sensors.Disconnect();
            music.Volume = 1;
            music.Stop();
            music.Dispose();
            baStream.Dispose();
            wavStream.Close();
            song.Close();
        }
    }
}
