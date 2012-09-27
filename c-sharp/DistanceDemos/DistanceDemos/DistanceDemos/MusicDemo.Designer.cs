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
            this.components = new System.ComponentModel.Container();
            this.FrequencyLabel = new System.Windows.Forms.Label();
            this.FixNotesCheckbox = new DistanceDemos.CustomCheckbox(this.components);
            this.SmoothCheckbox = new DistanceDemos.CustomCheckbox(this.components);
            this.TriangleRadio = new DistanceDemos.CustomRadio(this.components);
            this.SquareRadio = new DistanceDemos.CustomRadio(this.components);
            this.SineRadio = new DistanceDemos.CustomRadio(this.components);
            this.DisplayPanel = new DistanceDemos.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // FrequencyLabel
            // 
            this.FrequencyLabel.Location = new System.Drawing.Point(651, 186);
            this.FrequencyLabel.Name = "FrequencyLabel";
            this.FrequencyLabel.Size = new System.Drawing.Size(100, 23);
            this.FrequencyLabel.TabIndex = 6;
            this.FrequencyLabel.Text = "440 Hz";
            this.FrequencyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FixNotesCheckbox
            // 
            this.FixNotesCheckbox.AutoSize = true;
            this.FixNotesCheckbox.Location = new System.Drawing.Point(430, 190);
            this.FixNotesCheckbox.Name = "FixNotesCheckbox";
            this.FixNotesCheckbox.Size = new System.Drawing.Size(143, 17);
            this.FixNotesCheckbox.TabIndex = 5;
            this.FixNotesCheckbox.Text = "Snap to Chromatic Scale";
            this.FixNotesCheckbox.UseVisualStyleBackColor = true;
            // 
            // SmoothCheckbox
            // 
            this.SmoothCheckbox.AutoSize = true;
            this.SmoothCheckbox.Checked = true;
            this.SmoothCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SmoothCheckbox.Location = new System.Drawing.Point(288, 190);
            this.SmoothCheckbox.Name = "SmoothCheckbox";
            this.SmoothCheckbox.Size = new System.Drawing.Size(116, 17);
            this.SmoothCheckbox.TabIndex = 4;
            this.SmoothCheckbox.Text = "Smooth Transitions";
            this.SmoothCheckbox.UseVisualStyleBackColor = true;
            this.SmoothCheckbox.CheckedChanged += new System.EventHandler(this.SmoothCheckbox_CheckedChanged);
            // 
            // TriangleRadio
            // 
            this.TriangleRadio.AutoSize = true;
            this.TriangleRadio.Location = new System.Drawing.Point(147, 189);
            this.TriangleRadio.Name = "TriangleRadio";
            this.TriangleRadio.Size = new System.Drawing.Size(70, 17);
            this.TriangleRadio.TabIndex = 3;
            this.TriangleRadio.Text = "Sawtooth";
            this.TriangleRadio.UseVisualStyleBackColor = true;
            this.TriangleRadio.CheckedChanged += new System.EventHandler(this.TriangleRadio_CheckedChanged);
            // 
            // SquareRadio
            // 
            this.SquareRadio.AutoSize = true;
            this.SquareRadio.Location = new System.Drawing.Point(73, 189);
            this.SquareRadio.Name = "SquareRadio";
            this.SquareRadio.Size = new System.Drawing.Size(59, 17);
            this.SquareRadio.TabIndex = 2;
            this.SquareRadio.Text = "Square";
            this.SquareRadio.UseVisualStyleBackColor = true;
            this.SquareRadio.CheckedChanged += new System.EventHandler(this.SquareRadio_CheckedChanged);
            // 
            // SineRadio
            // 
            this.SineRadio.AutoSize = true;
            this.SineRadio.Checked = true;
            this.SineRadio.Location = new System.Drawing.Point(12, 189);
            this.SineRadio.Name = "SineRadio";
            this.SineRadio.Size = new System.Drawing.Size(46, 17);
            this.SineRadio.TabIndex = 1;
            this.SineRadio.TabStop = true;
            this.SineRadio.Text = "Sine";
            this.SineRadio.UseVisualStyleBackColor = true;
            this.SineRadio.CheckedChanged += new System.EventHandler(this.SineRadio_CheckedChanged);
            // 
            // DisplayPanel
            // 
            this.DisplayPanel.Location = new System.Drawing.Point(12, 12);
            this.DisplayPanel.Name = "DisplayPanel";
            this.DisplayPanel.Size = new System.Drawing.Size(739, 157);
            this.DisplayPanel.TabIndex = 0;
            this.DisplayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DisplayPanel_Paint);
            // 
            // MusicDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 225);
            this.Controls.Add(this.FrequencyLabel);
            this.Controls.Add(this.FixNotesCheckbox);
            this.Controls.Add(this.SmoothCheckbox);
            this.Controls.Add(this.TriangleRadio);
            this.Controls.Add(this.SquareRadio);
            this.Controls.Add(this.SineRadio);
            this.Controls.Add(this.DisplayPanel);
            this.KeyPreview = true;
            this.Name = "MusicDemo";
            this.Text = "MusicDemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MusicDemo_FormClosing);
            this.Load += new System.EventHandler(this.MusicDemo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MusicDemo_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedPanel DisplayPanel;
        private CustomRadio SineRadio;
        private CustomRadio SquareRadio;
        private CustomRadio TriangleRadio;
        private CustomCheckbox SmoothCheckbox;
        private CustomCheckbox FixNotesCheckbox;
        private System.Windows.Forms.Label FrequencyLabel;

    }
}