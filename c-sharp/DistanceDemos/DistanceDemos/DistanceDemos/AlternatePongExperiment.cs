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
    public partial class AlternatePongExperiment : Form
    {
        private const int PADDLE_HEIGHT = 100;
        private const int PADDLE_WIDTH = 10;
        private const int BALL_RADIUS = 5;
        private const int BALL_SPEED_FACTOR = 300;
        private const int BALL_START_TIMER = 3000;

        private float prevDist1 = -1, prevDist2 = -1;
        private bool goodDist1, goodDist2;
        private float paddle1, paddle2;
        private PointF ball, ballSpeed;
        private int score1, score2;
        private float ballStartTimer;

        private DistanceSensors sensors;
        private Random rand;

        public AlternatePongExperiment()
        {
            InitializeComponent();
            
            sensors = new DistanceSensors();
            sensors.DistancesChanged += new DistanceSensors.DistancesChangedHandler(sensors_DistancesChanged);
            sensors.Connect();

            paddle1 = 0.5f; paddle2 = 0.5f;
            ball = new PointF(DisplayPanel.Width / 2, DisplayPanel.Height / 2);
            rand = new Random();
            ballSpeed = new PointF((rand.NextDouble() <= 0.5 ? -1 : 1) * BALL_SPEED_FACTOR, (float)(rand.NextDouble() * 2 * BALL_SPEED_FACTOR) - BALL_SPEED_FACTOR);

            Task.Factory.StartNew(() =>
            {
                DateTime startTime = DateTime.Now;
                ballStartTimer = BALL_START_TIMER;
                while (!IsDisposed)
                {
                    try
                    {
                        DateTime currTime = DateTime.Now;
                        UpdatePositions((float)(currTime - startTime).TotalSeconds);
                        Invoke(new MethodInvoker(delegate { DisplayPanel.Refresh(); }));
                        startTime = currTime;
                        Thread.Sleep(20);
                    }
                    catch { }
                }
            });
        }

        ~AlternatePongExperiment()
        {
            sensors.Disconnect();
        }

        private void sensors_DistancesChanged(double[] dists)
        {
            float dist1 = (float)dists[0];
            float dist2 = (float)dists[1];
            goodDist1 = true; goodDist2 = true;
            if (dist1 == 80 && prevDist1 >= 0 && prevDist1 <= 30 /*&& Math.Abs(dist1 - prevDist1) > 20*/) { dist1 = prevDist1; goodDist1 = false; }
            if (dist2 == 80 && prevDist2 >= 0 && prevDist2 <= 30 /*&& Math.Abs(dist2 - prevDist2) > 20*/) { dist2 = prevDist2; goodDist2 = false; }
            prevDist1 = dist1;
            prevDist2 = dist2;

            // normalize distance
            float x = (dist1 - 10) / 20.0f;
            float y = (dist2 - 10) / 20.0f;

            // fix in [0, 1]
            if (x < 0) x = 0;
            else if (x > 1) x = 1;
            x = 1 - x;
            if (y < 0) y = 0;
            else if (y > 1) y = 1;
            y = 1 - y;

            paddle1 = x;
            paddle2 = y;
        }

        private void UpdatePositions(float elapsedSeconds)
        {
            ballStartTimer -= elapsedSeconds * 1000;
            if (ballStartTimer > 0) return;

            PointF prev = new PointF(ball.X, ball.Y);
            ball.X += ballSpeed.X * elapsedSeconds;
            ball.Y += ballSpeed.Y * elapsedSeconds;

            // collisions with side walls
            if (ball.Y - BALL_RADIUS < 0) { ball.Y = 2 * BALL_RADIUS - ball.Y; ballSpeed.Y = -ballSpeed.Y; }
            else if (ball.Y + BALL_RADIUS >= DisplayPanel.Height) { ball.Y = 2 * DisplayPanel.Height - ball.Y - 2 * BALL_RADIUS; ballSpeed.Y = -ballSpeed.Y; }

            // collisions with paddles
            if (ball.X - BALL_RADIUS < 10 + PADDLE_WIDTH && prev.X - BALL_RADIUS > 10 + PADDLE_WIDTH)
            {
                // solve for point where ball trajectory intersects paddle plane
                float x1 = prev.X; float y1 = prev.Y;
                float x2 = ball.X; float y2 = ball.Y;
                float m = (y2 - y1) / (x2 - x1);
                float b = y1 - m * x1;
                float x = 10 + PADDLE_WIDTH + BALL_RADIUS;
                float y = m * x + b;

                // check if ball hit paddle
                float paddleY = (DisplayPanel.Height - PADDLE_HEIGHT) * paddle1;
                if (y >= paddleY - BALL_RADIUS && y < paddleY + PADDLE_HEIGHT + BALL_RADIUS)
                {
                    ball.X = x + (x - x2);
                    ballSpeed.X = -ballSpeed.X;

                    // increase ball speed
                    ballSpeed.X *= 1.05f;
                }

                // did we hit the edge of the paddle?
                if (ballSpeed.Y > 0)
                {
                    y = paddleY;
                    x = (y - b) / m;
                    if (x >= 10 - BALL_RADIUS && x < 10 + PADDLE_WIDTH + BALL_RADIUS)
                    {
                        ball.Y = y - (y2 - y);
                        ballSpeed.Y = -ballSpeed.Y;
                    }
                }
                else if (ballSpeed.Y < 0)
                {
                    y = paddleY + PADDLE_HEIGHT;
                    x = (y - b) / m;
                    if (x >= 10 - BALL_RADIUS && x < 10 + PADDLE_WIDTH + BALL_RADIUS)
                    {
                        ball.Y = y + (y - y2);
                        ballSpeed.Y = -ballSpeed.Y;
                    }
                }
            }
            else if (ball.X + BALL_RADIUS > DisplayPanel.Width - 10 - PADDLE_WIDTH && prev.X + BALL_RADIUS < DisplayPanel.Width - 10 - PADDLE_WIDTH)
            {
                // solve for point where ball trajectory intersects paddle plane
                float x1 = prev.X; float y1 = prev.Y;
                float x2 = ball.X; float y2 = ball.Y;
                float m = (y2 - y1) / (x2 - x1);
                float b = y1 - m * x1;
                float x = DisplayPanel.Width - 10 - PADDLE_WIDTH - BALL_RADIUS;
                float y = m * x + b;

                // check if ball hit paddle
                float paddleY = (DisplayPanel.Height - PADDLE_HEIGHT) * paddle2;
                if (y >= paddleY - BALL_RADIUS && y < paddleY + PADDLE_HEIGHT + BALL_RADIUS)
                {
                    ball.X = x - (x2 - x);
                    ballSpeed.X = -ballSpeed.X;
                    
                    // increase ball speed
                    ballSpeed.X *= 1.05f;
                }

                // did we hit the edge of the paddle?
                if (ballSpeed.Y > 0)
                {
                    y = paddleY;
                    x = (y - b) / m;
                    if (x >= DisplayPanel.Width - 10 - PADDLE_WIDTH - BALL_RADIUS && x < DisplayPanel.Width - 10 + BALL_RADIUS)
                    {
                        ball.Y = y - (y2 - y);
                        ballSpeed.Y = -ballSpeed.Y;
                    }
                }
                else if (ballSpeed.Y < 0)
                {
                    y = paddleY + PADDLE_HEIGHT;
                    x = (y - b) / m;
                    if (x >= DisplayPanel.Width - 10 - PADDLE_WIDTH - BALL_RADIUS && x < DisplayPanel.Width - 10 + BALL_RADIUS)
                    {
                        ball.Y = y + (y - y2);
                        ballSpeed.Y = -ballSpeed.Y;
                    }
                }
            }

            // collisions with goal walls (point!)
            if (ball.X - BALL_RADIUS < 0)
            {
                score2++;
                ballStartTimer = BALL_START_TIMER;
                ball = new PointF(DisplayPanel.Width / 2, DisplayPanel.Height / 2);
                ballSpeed = new PointF((rand.NextDouble() <= 0.5 ? -1 : 1) * BALL_SPEED_FACTOR, (float)(rand.NextDouble() * 2 * BALL_SPEED_FACTOR) - BALL_SPEED_FACTOR);
            }
            else if (ball.X + BALL_RADIUS > DisplayPanel.Width)
            {
                score1++;
                ballStartTimer = BALL_START_TIMER;
                ball = new PointF(DisplayPanel.Width / 2, DisplayPanel.Height / 2);
                ballSpeed = new PointF((rand.NextDouble() <= 0.5 ? -1 : 1) * BALL_SPEED_FACTOR, (float)(rand.NextDouble() * 2 * BALL_SPEED_FACTOR) - BALL_SPEED_FACTOR);
            }
        }

        private void DisplayPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);

            for (int i = 0; i < DisplayPanel.Height / 30 + 1; i++)
                e.Graphics.FillRectangle(Brushes.White, DisplayPanel.Width / 2 - 2, i * 30, 4, 20);

            if (ballStartTimer > 0)
            {
                string timerString = "Service in: " + (ballStartTimer / 1000).ToString("0.00") + "s";
                SizeF timerStringSize = e.Graphics.MeasureString(timerString, Font);
                e.Graphics.FillRectangle(Brushes.Black, DisplayPanel.Width / 2 - timerStringSize.Width / 2 - 5, DisplayPanel.Height / 2 - BALL_RADIUS - 10, timerStringSize.Width + 10, timerStringSize.Height + 10 + 2 * BALL_RADIUS + 10);
                e.Graphics.DrawRectangle(Pens.White, DisplayPanel.Width / 2 - timerStringSize.Width / 2 - 5, DisplayPanel.Height / 2 - BALL_RADIUS - 10, timerStringSize.Width + 10, timerStringSize.Height + 10 + 2 * BALL_RADIUS + 10);
                e.Graphics.DrawString(timerString, Font, Brushes.White, DisplayPanel.Width / 2 - timerStringSize.Width / 2, DisplayPanel.Height / 2 + BALL_RADIUS + 10);
            }

            e.Graphics.DrawString(score1.ToString(), Font, Brushes.White, DisplayPanel.Width / 2 - 10 - e.Graphics.MeasureString(score1.ToString(), Font).Width, 10);
            e.Graphics.DrawString(score2.ToString(), Font, Brushes.White, DisplayPanel.Width / 2 + 10, 10);

            e.Graphics.FillRectangle(goodDist1 ? Brushes.White : Brushes.Red, 10, (DisplayPanel.Height - PADDLE_HEIGHT) * paddle1, PADDLE_WIDTH, PADDLE_HEIGHT);
            e.Graphics.FillRectangle(goodDist2 ? Brushes.White : Brushes.Red, DisplayPanel.Width - 10 - PADDLE_WIDTH, (DisplayPanel.Height - PADDLE_HEIGHT) * paddle2, PADDLE_WIDTH, PADDLE_HEIGHT);

            e.Graphics.FillRectangle(Brushes.White, ball.X - BALL_RADIUS, ball.Y - BALL_RADIUS, 2 * BALL_RADIUS, 2 * BALL_RADIUS);
        }

        private void AlternatePongExperiment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void AlternatePongExperiment_Resize(object sender, EventArgs e)
        {
            if (ballStartTimer > 0)
                ball = new PointF(DisplayPanel.Width / 2, DisplayPanel.Height / 2);
        }
    }
}
