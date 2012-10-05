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
        float analog1;
        float analog2;
        int user1Score;
        int user2Score;
        bool direction;
        int pongX;
        int pongY;
        int reAnalog1, reAnalog2;
        public Pong()
        {
            InitializeComponent();
            textToDisplay = "";
            user1Score = 0;
            user2Score = 0;
            direction = true;
            pongX = 270;
            pongY = 179;

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
            if (dists.Length > 0) analog1 = (float)dists[0];
            if (dists.Length > 1) analog2 = (float)dists[1];
            textToDisplay = analog1.ToString("0.0") + ", " + analog2.ToString("0.0");
            UpdateFrame();
            movePaddes();
            moveBall();
        }

        private void UpdateFrame()
        {
            Invoke(new MethodInvoker(delegate
            {
                textBox1.Text = textToDisplay;
                textBox1.Enabled = false;
                DisplayPanel.Refresh();
            }));
            
        }
        private void movePaddes()
        {
            Invoke(new MethodInvoker(delegate
            {
                int i = 0;
                //12,169 
                //starts from 8 ends at 30. translate it to movements between 22 steps to measure
                //8 to 19 -> 0 to 165; 20-30 -> 166 to 330
                reAnalog1 = (int)analog1;
                reAnalog2 = (int)analog2;
                reAnalog1 = (reAnalog1 - 8) * (165 / 11);
                reAnalog2 = (reAnalog2 - 8) * (165 / 11);
                if (reAnalog1 > 330) reAnalog1 = 330;
                if (reAnalog2 > 330) reAnalog2 = 330;

                //if(reAnalog1>=0 && reAnalog1<=165) 
                this.pictureBox1.Location = new System.Drawing.Point(12, reAnalog1);
                this.pictureBox2.Location = new System.Drawing.Point(503, reAnalog2);
                DisplayPanel.Refresh();
            }));
            
        }
        private void moveBall()
        {
            //270 179
            
            this.pictureBox3.Location = new System.Drawing.Point(pongX,pongY);
            pongX--;
            pongY++;
            Invoke(new MethodInvoker(delegate
            {
                if (direction = true)
                {
                    this.pictureBox3.Location = new System.Drawing.Point(pongX, pongY);
                    pongX--;
                    pongY++;
                    //if(pongX==78 && pictureBox1.Location
                    DisplayPanel.Refresh();
                }
                if (pongX == 78 && reAnalog1 <= pongY - 12)
                {
                    pongX++;
                    pongY--;
                    this.pictureBox3.Location = new System.Drawing.Point(pongX, pongY);
                    //DisplayPanel.Refresh();
                }
            }));
        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            int i = 10;
        }

    }
}
