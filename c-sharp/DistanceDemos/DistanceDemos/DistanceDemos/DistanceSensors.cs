using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace DistanceDemos
{
    class DistanceSensors
    {
        public const int DEFAULT_BAUD_RATE = 9600;

        private SerialPort sp;
        
        public DistanceSensors()
        {
        }

        #region Events

        public delegate void DistancesChangedHandler(double dist1, double dist2);
        public event DistancesChangedHandler DistancesChanged;
        public virtual void OnDistancesChanged(double dist1, double dist2)
        {
            DistancesChanged(dist1, dist2);
        }

        #endregion

        #region Public Functions

        public bool Connect()
        {
            return OpenPort(Properties.Settings.Default.ComPort, DEFAULT_BAUD_RATE);
        }

        public bool Disconnect()
        {
            return ClosePort();
        }

        #endregion

        #region Private Functions

        private bool OpenPort(string port, int baudRate)
        {
            try
            {
                sp = new SerialPort(port, baudRate);
                if (!sp.IsOpen)
                    sp.Open();
                sp.DataReceived += new SerialDataReceivedEventHandler(ArduinoDataReceived);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ClosePort()
        {
            try
            {
                sp.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //private void SendData(string data)
        //{
        //    lock (sp)
        //    {
        //        try
        //        {
        //            sp.Write(data + "\n");
        //        }
        //        catch { }
        //    }
        //}

        #endregion

        #region Event Handlers

        private void ArduinoDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = sp.ReadLine();
                while (data.Length == 0 || data[0] != '=')
                    data = sp.ReadLine();

                int dist1, dist2;
                string[] vals = data.Substring(1).Split(',');
                dist1 = int.Parse(vals[0]);
                dist2 = int.Parse(vals[1]);
                OnDistancesChanged(dist1, dist2);
            }
            catch { }
        }

        #endregion
    }
}
