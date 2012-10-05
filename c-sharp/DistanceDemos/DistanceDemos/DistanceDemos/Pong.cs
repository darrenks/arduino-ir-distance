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
        bool direction;
        bool user;
        int pongX;
        int pongY;
        int x;
        int y;
        int reAnalog1, reAnalog2;
        //Thread t;
        //private static System.Timers.Timer pongTimer;
        System.Threading.Timer pongTimer;
        public Pong()
        {
            InitializeComponent();
            textToDisplay = "";
            user1Score = 0;
            user2Score = 0;
            direction = true;
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
            pongTimer.Change(0, 1000); //enable
            pongTimer = new System.Threading.Timer(new TimerCallback(moveBall), null, 0, 10);
            //moveBall();
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
        private void moveBall(object source)
        {
            Invoke(new MethodInvoker(delegate
            {
            //270 179
                textBox1.Text = "abc";
                if (direction == true && pongY != 353)
                {
                    x = -1;
                    y = 1;
                    pongY += x;
                    pongY += y;
                    this.pictureBox3.Location = new System.Drawing.Point(pongX, pongY);
                }
            }));

        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            int i = 10;
        }

    }
}
