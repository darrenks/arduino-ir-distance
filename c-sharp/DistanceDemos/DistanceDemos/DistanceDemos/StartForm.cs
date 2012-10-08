using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DistanceDemos
{
    public partial class StartForm : Form
    {
        public enum DemoType { None, Music, Pong, Pong2, DJ };
        public DemoType SelectedDemo = DemoType.None;

        public StartForm()
        {
            InitializeComponent();

            PortChooser.Items.AddRange(SerialPort.GetPortNames());
            if (PortChooser.Items.Contains(Properties.Settings.Default.ComPort))
                PortChooser.SelectedItem = Properties.Settings.Default.ComPort;
            else
                PortChooser.SelectedIndex = 0;
        }

        private void PortChooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ComPort = (string)PortChooser.SelectedItem;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedDemo = DemoType.Music;
            Close();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectedDemo = DemoType.Pong2;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SelectedDemo = DemoType.Pong;
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SelectedDemo = DemoType.DJ;
            Close();
        }
    }
}
