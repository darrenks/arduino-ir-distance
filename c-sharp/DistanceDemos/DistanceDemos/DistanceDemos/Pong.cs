using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace DistanceDemos
{
    public partial class Pong : Form
    {
        DistanceSensors sensors;
        String textToDisplay;
        //PictureBox paddle1;
        //PictureBox paddle2;
        public Pong()
        {
            InitializeComponent();
            textToDisplay = "";
            //paddle1 = pictureBox1;
            //paddle2 = pictureBox2;
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
            float analog1 = (float)dists[0];
            float analog2 = (float)dists[1];
            //String textToDisplay = "";
            textToDisplay = Convert.ToString(analog1) + ", " + Convert.ToString(analog2);
            //textBox1.Text = textToDisplay;
            //textBox1.Enabled = false;
            //DisplayPanel.Refresh();
        }

        private void Update()
        {
            Invoke(new MethodInvoker(delegate
            {
                textBox1.Text = textToDisplay;
                textBox1.Enabled = false;
                DisplayPanel.Refresh();
            }));
            
        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            int i = 10;
            
            /*for (; i < 210; i++)
            {
                e.Graphics.FillRectangle(Brushes.Blue, 10, i, 50, 50);
                //Task.Factory.StartNew(()=> {});
                //Thread.Sleep(500);
                //e.Graphics.Clear
                
            }*/
           //e.Graphics.FillEllipse(Brushes.Green, 100, 100, 50, 50);
        }

    }
}
