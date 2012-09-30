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

        private void Pong_Load(object sender, EventArgs e)
        {
            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();

            Update();
        }
        void sensors_DistancesChanged(double[] dists)
        {
            // normalize distance
            float x = (float)dists[0];
            float y = (float)dists[1];
            float z = (float)dists[2];

            DisplayPanel.Refresh();
        }

        private void Update()
        {

        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Blue, 10, 10, 50, 50);
        }
    }
}
