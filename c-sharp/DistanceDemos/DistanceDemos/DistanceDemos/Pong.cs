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
    public partial class Pong : Form
    {
        DistanceSensors sensors;
        public Pong()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Pong_Load(object sender, EventArgs e)
        {
            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();
        }
        void sensors_DistancesChanged(double[] dists)
        {
            // normalize distance
            float x = (float)dists[0];
            float y = (float)dists[1];
            float z = (float)dists[2];
            label1.Text = x + ", " + y + ", "+ z;
            Invoke(new MethodInvoker(delegate
                {
                    label1.Text = x + ", " + y + ", " + z;
                }));
        }
    }
}
