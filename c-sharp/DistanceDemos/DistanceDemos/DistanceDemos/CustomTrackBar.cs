using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DistanceDemos
{
    public partial class CustomTrackBar : System.Windows.Forms.TrackBar
    {
        public CustomTrackBar()
        {
            InitializeComponent();
        }

        public CustomTrackBar(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override bool IsInputKey(System.Windows.Forms.Keys keyData)
        {
            //return base.IsInputKey(keyData);
            return true;
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
