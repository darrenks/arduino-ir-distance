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
    public partial class Form1 : Form
    {
        private DistanceSensors sensors;

        public Form1()
        {
            InitializeComponent();

            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();
        }

        private void sensors_DistancesChanged(double dist1, double dist2)
        {
            Invoke(new MethodInvoker(delegate
            {
                Distance1.Value = (int)dist1;
            }));
        }
    }
}
