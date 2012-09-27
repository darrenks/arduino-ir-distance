using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DistanceDemos
{
    public partial class CustomRadio : System.Windows.Forms.RadioButton
    {
        public CustomRadio()
        {
            InitializeComponent();
        }

        public CustomRadio(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override bool IsInputKey(System.Windows.Forms.Keys keyData)
        {
            //return base.IsInputKey(keyData);
            return true;
        }
    }
}
