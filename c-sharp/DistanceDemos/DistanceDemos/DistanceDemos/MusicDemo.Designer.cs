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
            this.SuspendLayout();
            // 
            // MusicDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 377);
            this.KeyPreview = true;
            this.Name = "MusicDemo";
            this.Text = "MusicDemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MusicDemo_FormClosing);
            this.Load += new System.EventHandler(this.MusicDemo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MusicDemo_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

    }
}