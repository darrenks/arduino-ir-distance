using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DistanceDemos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartForm start = new StartForm();
            Application.Run(start);
            if (start.SelectedDemo == StartForm.DemoType.Music) Application.Run(new MusicDemo());
            else if (start.SelectedDemo == StartForm.DemoType.Pong) Application.Run(new Pong());
            else if (start.SelectedDemo == StartForm.DemoType.Pong2) Application.Run(new AlternatePongExperiment());
        }
    }
}
