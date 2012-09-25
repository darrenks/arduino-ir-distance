namespace DistanceDemos
{
    partial class MusicDemo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SineRadio = new System.Windows.Forms.RadioButton();
            this.TriangleRadio = new System.Windows.Forms.RadioButton();
            this.SquareRadio = new System.Windows.Forms.RadioButton();
            this.SmoothCheckbox = new System.Windows.Forms.CheckBox();
            this.FixNotesCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SineRadio
            // 
            this.SineRadio.AutoSize = true;
            this.SineRadio.Checked = true;
            this.SineRadio.Location = new System.Drawing.Point(12, 12);
            this.SineRadio.Name = "SineRadio";
            this.SineRadio.Size = new System.Drawing.Size(46, 17);
            this.SineRadio.TabIndex = 0;
            this.SineRadio.TabStop = true;
            this.SineRadio.Text = "Sine";
            this.SineRadio.UseVisualStyleBackColor = true;
            this.SineRadio.CheckedChanged += new System.EventHandler(this.SineRadio_CheckedChanged);
            // 
            // TriangleRadio
            // 
            this.TriangleRadio.AutoSize = true;
            this.TriangleRadio.Location = new System.Drawing.Point(129, 12);
            this.TriangleRadio.Name = "TriangleRadio";
            this.TriangleRadio.Size = new System.Drawing.Size(63, 17);
            this.TriangleRadio.TabIndex = 1;
            this.TriangleRadio.Text = "Triangle";
            this.TriangleRadio.UseVisualStyleBackColor = true;
            this.TriangleRadio.CheckedChanged += new System.EventHandler(this.TriangleRadio_CheckedChanged);
            // 
            // SquareRadio
            // 
            this.SquareRadio.AutoSize = true;
            this.SquareRadio.Location = new System.Drawing.Point(64, 12);
            this.SquareRadio.Name = "SquareRadio";
            this.SquareRadio.Size = new System.Drawing.Size(59, 17);
            this.SquareRadio.TabIndex = 2;
            this.SquareRadio.Text = "Square";
            this.SquareRadio.UseVisualStyleBackColor = true;
            this.SquareRadio.CheckedChanged += new System.EventHandler(this.SquareRadio_CheckedChanged);
            // 
            // SmoothCheckbox
            // 
            this.SmoothCheckbox.AutoSize = true;
            this.SmoothCheckbox.Checked = true;
            this.SmoothCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SmoothCheckbox.Location = new System.Drawing.Point(12, 49);
            this.SmoothCheckbox.Name = "SmoothCheckbox";
            this.SmoothCheckbox.Size = new System.Drawing.Size(116, 17);
            this.SmoothCheckbox.TabIndex = 3;
            this.SmoothCheckbox.Text = "Smooth Transitions";
            this.SmoothCheckbox.UseVisualStyleBackColor = true;
            this.SmoothCheckbox.CheckedChanged += new System.EventHandler(this.SmoothCheckbox_CheckedChanged);
            // 
            // FixNotesCheckbox
            // 
            this.FixNotesCheckbox.AutoSize = true;
            this.FixNotesCheckbox.Location = new System.Drawing.Point(12, 82);
            this.FixNotesCheckbox.Name = "FixNotesCheckbox";
            this.FixNotesCheckbox.Size = new System.Drawing.Size(131, 17);
            this.FixNotesCheckbox.TabIndex = 4;
            this.FixNotesCheckbox.Text = "Fix to Chromatic Scale";
            this.FixNotesCheckbox.UseVisualStyleBackColor = true;
            // 
            // MusicDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 377);
            this.Controls.Add(this.FixNotesCheckbox);
            this.Controls.Add(this.SmoothCheckbox);
            this.Controls.Add(this.SquareRadio);
            this.Controls.Add(this.TriangleRadio);
            this.Controls.Add(this.SineRadio);
            this.KeyPreview = true;
            this.Name = "MusicDemo";
            this.Text = "MusicDemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MusicDemo_FormClosing);
            this.Load += new System.EventHandler(this.MusicDemo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton SineRadio;
        private System.Windows.Forms.RadioButton TriangleRadio;
        private System.Windows.Forms.RadioButton SquareRadio;
        private System.Windows.Forms.CheckBox SmoothCheckbox;
        private System.Windows.Forms.CheckBox FixNotesCheckbox;
    }
}