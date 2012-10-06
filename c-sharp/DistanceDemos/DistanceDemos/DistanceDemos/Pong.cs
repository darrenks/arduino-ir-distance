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
using System.Timers;


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
        bool directionX;
        bool directionY;
        bool user;
        int pongX;
        int pongY;
        int x;
        int y;
        int reAnalog1, reAnalog2;
        //Thread t;
        public Pong()
        {
            InitializeComponent();
            textToDisplay = "";
            user1Score = 0;
            user2Score = 0;
            directionX = true;
            directionY = false;
            user = true;
            pongX = 270;
            pongY = 179;
            x = -1;
            y = -1;
        }

        private void Pong_Load(object sender, EventArgs e)
        {
            
            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();
            //moveBall();
            timer1.Start();
        }

        void sensors_DistancesChanged(double[] dists)
        {
            // normalize distance
            if (dists.Length > 0) analog1 = (float)dists[0];
            if (dists.Length > 1) analog2 = (float)dists[1];
            textToDisplay = analog1.ToString("0.0") + ", " + analog2.ToString("0.0");
            //textToDisplay = pongY.ToString();
            UpdateFrame();
            movePaddes();
            //moveBall();
        }

        private void UpdateFrame()
        {
            Invoke(new MethodInvoker(delegate
            {
                //textBox1.Text = textToDisplay;
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
            Invoke(new MethodInvoker(delegate
            {
            //270 179
                //this.textBox1.Text = "abc";
                if (directionX == true && directionY==false && pongY != 353)
                {
                    x = -1;
                    y = 1;
                    pongY += x;
                    pongY += y;
                    this.pictureBox3.Location = new System.Drawing.Point(pongX, pongY);
                    DisplayPanel.Refresh();
                }
            }));

        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            int i = 10;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Invoke(new MethodInvoker(delegate
            //{
                //270 179
                //this.textBox1.Text = "abc";
                if (directionX == true && directionY == false && pongY != 353)
                {
                    //textBox1.Text = "abc";
                    x = -1;
                    y = 1;
                    pongX += x;
                    pongY += y;
                    if (true) this.pictureBox3.Location = new System.Drawing.Point(pongX, pongY);
                    if (pongY == 353)
                    {
                        x = -1;
                        y = -1;
                        directionY = true;
                    }
                    DisplayPanel.Refresh();
                }
                else if (directionX == true && directionY==true && pongX!=0)
                {
                    textBox1.Text = "abc";
                    x = -1;
                    y = -1;
                    pongX += x;
                    pongY += y;
                    if (true) this.pictureBox3.Location = new System.Drawing.Point(pongX, pongY);
                    if (pongX == 0)
                    {
                        x = 1;
                        y = -1;
                        directionX = false;
                    }
                    DisplayPanel.Refresh();
                }
                else if (directionX == false && directionY == true && pongY != 0)
                {
                    textBox1.Text = "abc";
                    x = 1;
                    y = -1;
                    pongX += x;
                    pongY += y;
                    if (true) this.pictureBox3.Location = new System.Drawing.Point(pongX, pongY);
                    if (pongY == 0)
                    {
                        x = 1;
                        y = 1;
                        directionY = false;
                    }
                    DisplayPanel.Refresh();
                }
                else if (directionX == false && directionY == false && pongX != 540)
                {
                    textBox1.Text = pongX.ToString();
                    x = 1;
                    y = 1;
                    pongX += x;
                    pongY += y;
                    if (true) this.pictureBox3.Location = new System.Drawing.Point(pongX, pongY);
                    if (pongX == 540)
                    {
                        x = -1;
                        y = 1;
                        directionX = true;
                    }
                    DisplayPanel.Refresh();
                }
            //}));
        }

    }
}
