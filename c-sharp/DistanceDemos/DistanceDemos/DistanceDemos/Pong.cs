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
        bool user;
        int pongX;
        int pongY;
        int x;
        int y;
        int reAnalog1, reAnalog2;
        //Thread t;
        int xMax;
        int xMin;
        int yMax;
        int yMin;
        public Pong()
        {
            InitializeComponent();
            textToDisplay = "";
            user1Score = 0;
            user2Score = 0;
            user = true;
            pongX = 270;
            pongY = 179;
            x = -1;
            y = 1;
            xMin = 0;
            xMax = 540;
            yMin = 0;
            yMax = 353;
        }

        private void Pong_Load(object sender, EventArgs e)
        {
            
            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();
            timer1.Start();
        }

        void sensors_DistancesChanged(double[] dists)
        {
            // normalize distance
            if (dists.Length > 0) analog1 = (float)dists[0];
            if (dists.Length > 1) analog2 = (float)dists[1];
            textToDisplay = analog1.ToString("0.0") + ", " + analog2.ToString("0.0");
            UpdateFrame();
            movePaddes();
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
                //Process input from the IR sensor in the range 8 to 30
                //Translate it to movement from of the paddles
                //8 to 19   -> translate to y positions of 0   to 165 
                //20 to 30  -> translate to y positions of 166 to 330
                reAnalog1 = (int)analog1;
                reAnalog2 = (int)analog2;
                reAnalog1 = (reAnalog1 - 8) * (165 / 11);
                reAnalog2 = (reAnalog2 - 8) * (165 / 11);
                if (reAnalog1 > 330) reAnalog1 = 330;
                if (reAnalog2 > 330) reAnalog2 = 330;
 
                this.pictureBox1.Location = new System.Drawing.Point(12, reAnalog1);
                this.pictureBox2.Location = new System.Drawing.Point(503, reAnalog2);
                DisplayPanel.Refresh();
            }));
            
        }
        
        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            //int i = 10;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pongY == yMax) y = -1;
            if (pongY == yMin) y = 1;
            if (pongX == xMax) x = -1;
            if (pongX == xMin) x = 1;
            textBox1.Text = pongX.ToString() + ", " + pongY.ToString();
            pongX+=x;
            pongY+=y;
            pictureBox3.Location = new System.Drawing.Point(pongX, pongY);
            DisplayPanel.Refresh();
        }
    }
}
