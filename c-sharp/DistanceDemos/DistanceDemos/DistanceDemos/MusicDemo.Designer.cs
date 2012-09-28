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
            this.OrganSettingsPanel = new System.Windows.Forms.Panel();
            this.TremeloAmplitudeSlider = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.OrganBar9 = new DistanceDemos.CustomTrackBar(this.components);
            this.OrganBar8 = new DistanceDemos.CustomTrackBar(this.components);
            this.OrganBar7 = new DistanceDemos.CustomTrackBar(this.components);
            this.OrganBar6 = new DistanceDemos.CustomTrackBar(this.components);
            this.OrganBar5 = new DistanceDemos.CustomTrackBar(this.components);
            this.OrganBar4 = new DistanceDemos.CustomTrackBar(this.components);
            this.OrganBar3 = new DistanceDemos.CustomTrackBar(this.components);
            this.OrganBar2 = new DistanceDemos.CustomTrackBar(this.components);
            this.OrganBar1 = new DistanceDemos.CustomTrackBar(this.components);
            this.PresetChooser = new DistanceDemos.CustomComboBox(this.components);
            this.SoundChooser = new DistanceDemos.CustomComboBox(this.components);
            this.FixNotesCheckbox = new DistanceDemos.CustomCheckbox(this.components);
            this.DisplayPanel = new DistanceDemos.DoubleBufferedPanel();
            this.TremeloFrequencySlider = new DistanceDemos.CustomTrackBar(this.components);
            this.ExtendedDisplayPanel = new DistanceDemos.DoubleBufferedPanel();
            this.OrganSettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TremeloAmplitudeSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TremeloFrequencySlider)).BeginInit();
            this.SuspendLayout();
            // 
            // FrequencyLabel
            // 
            this.FrequencyLabel.Location = new System.Drawing.Point(651, 321);
            this.FrequencyLabel.Name = "FrequencyLabel";
            this.FrequencyLabel.Size = new System.Drawing.Size(100, 23);
            this.FrequencyLabel.TabIndex = 6;
            this.FrequencyLabel.Text = "440 Hz";
            this.FrequencyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OrganSettingsPanel
            // 
            this.OrganSettingsPanel.Controls.Add(this.OrganBar9);
            this.OrganSettingsPanel.Controls.Add(this.OrganBar8);
            this.OrganSettingsPanel.Controls.Add(this.OrganBar7);
            this.OrganSettingsPanel.Controls.Add(this.OrganBar6);
            this.OrganSettingsPanel.Controls.Add(this.OrganBar5);
            this.OrganSettingsPanel.Controls.Add(this.OrganBar4);
            this.OrganSettingsPanel.Controls.Add(this.OrganBar3);
            this.OrganSettingsPanel.Controls.Add(this.OrganBar2);
            this.OrganSettingsPanel.Controls.Add(this.OrganBar1);
            this.OrganSettingsPanel.Controls.Add(this.PresetChooser);
            this.OrganSettingsPanel.Location = new System.Drawing.Point(466, 347);
            this.OrganSettingsPanel.Name = "OrganSettingsPanel";
            this.OrganSettingsPanel.Size = new System.Drawing.Size(285, 148);
            this.OrganSettingsPanel.TabIndex = 10;
            // 
            // TremeloAmplitudeSlider
            // 
            this.TremeloAmplitudeSlider.Location = new System.Drawing.Point(66, 381);
            this.TremeloAmplitudeSlider.Maximum = 100;
            this.TremeloAmplitudeSlider.Name = "TremeloAmplitudeSlider";
            this.TremeloAmplitudeSlider.Size = new System.Drawing.Size(104, 45);
            this.TremeloAmplitudeSlider.TabIndex = 11;
            this.TremeloAmplitudeSlider.TickFrequency = 10;
            this.TremeloAmplitudeSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TremeloAmplitudeSlider.Scroll += new System.EventHandler(this.TremeloAmplitudeSlider_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 387);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Tremelo:";
            // 
            // OrganBar9
            // 
            this.OrganBar9.Location = new System.Drawing.Point(235, 30);
            this.OrganBar9.Maximum = 8;
            this.OrganBar9.Name = "OrganBar9";
            this.OrganBar9.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OrganBar9.Size = new System.Drawing.Size(45, 104);
            this.OrganBar9.TabIndex = 22;
            this.OrganBar9.TickFrequency = 10;
            this.OrganBar9.TickStyle = System.Windows.Forms.TickStyle.None;
            this.OrganBar9.ValueChanged += new System.EventHandler(this.OrganBar_ValueChanged);
            // 
            // OrganBar8
            // 
            this.OrganBar8.Location = new System.Drawing.Point(206, 30);
            this.OrganBar8.Maximum = 8;
            this.OrganBar8.Name = "OrganBar8";
            this.OrganBar8.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OrganBar8.Size = new System.Drawing.Size(45, 104);
            this.OrganBar8.TabIndex = 21;
            this.OrganBar8.TickFrequency = 10;
            this.OrganBar8.TickStyle = System.Windows.Forms.TickStyle.None;
            this.OrganBar8.ValueChanged += new System.EventHandler(this.OrganBar_ValueChanged);
            // 
            // OrganBar7
            // 
            this.OrganBar7.Location = new System.Drawing.Point(177, 30);
            this.OrganBar7.Maximum = 8;
            this.OrganBar7.Name = "OrganBar7";
            this.OrganBar7.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OrganBar7.Size = new System.Drawing.Size(45, 104);
            this.OrganBar7.TabIndex = 20;
            this.OrganBar7.TickFrequency = 10;
            this.OrganBar7.TickStyle = System.Windows.Forms.TickStyle.None;
            this.OrganBar7.ValueChanged += new System.EventHandler(this.OrganBar_ValueChanged);
            // 
            // OrganBar6
            // 
            this.OrganBar6.Location = new System.Drawing.Point(148, 30);
            this.OrganBar6.Maximum = 8;
            this.OrganBar6.Name = "OrganBar6";
            this.OrganBar6.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OrganBar6.Size = new System.Drawing.Size(45, 104);
            this.OrganBar6.TabIndex = 19;
            this.OrganBar6.TickFrequency = 10;
            this.OrganBar6.TickStyle = System.Windows.Forms.TickStyle.None;
            this.OrganBar6.ValueChanged += new System.EventHandler(this.OrganBar_ValueChanged);
            // 
            // OrganBar5
            // 
            this.OrganBar5.Location = new System.Drawing.Point(119, 30);
            this.OrganBar5.Maximum = 8;
            this.OrganBar5.Name = "OrganBar5";
            this.OrganBar5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OrganBar5.Size = new System.Drawing.Size(45, 104);
            this.OrganBar5.TabIndex = 18;
            this.OrganBar5.TickFrequency = 10;
            this.OrganBar5.TickStyle = System.Windows.Forms.TickStyle.None;
            this.OrganBar5.ValueChanged += new System.EventHandler(this.OrganBar_ValueChanged);
            // 
            // OrganBar4
            // 
            this.OrganBar4.Location = new System.Drawing.Point(90, 30);
            this.OrganBar4.Maximum = 8;
            this.OrganBar4.Name = "OrganBar4";
            this.OrganBar4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OrganBar4.Size = new System.Drawing.Size(45, 104);
            this.OrganBar4.TabIndex = 17;
            this.OrganBar4.TickFrequency = 10;
            this.OrganBar4.TickStyle = System.Windows.Forms.TickStyle.None;
            this.OrganBar4.ValueChanged += new System.EventHandler(this.OrganBar_ValueChanged);
            // 
            // OrganBar3
            // 
            this.OrganBar3.Location = new System.Drawing.Point(61, 30);
            this.OrganBar3.Maximum = 8;
            this.OrganBar3.Name = "OrganBar3";
            this.OrganBar3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OrganBar3.Size = new System.Drawing.Size(45, 104);
            this.OrganBar3.TabIndex = 16;
            this.OrganBar3.TickFrequency = 10;
            this.OrganBar3.TickStyle = System.Windows.Forms.TickStyle.None;
            this.OrganBar3.ValueChanged += new System.EventHandler(this.OrganBar_ValueChanged);
            // 
            // OrganBar2
            // 
            this.OrganBar2.Location = new System.Drawing.Point(32, 30);
            this.OrganBar2.Maximum = 8;
            this.OrganBar2.Name = "OrganBar2";
            this.OrganBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OrganBar2.Size = new System.Drawing.Size(45, 104);
            this.OrganBar2.TabIndex = 15;
            this.OrganBar2.TickFrequency = 10;
            this.OrganBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.OrganBar2.ValueChanged += new System.EventHandler(this.OrganBar_ValueChanged);
            // 
            // OrganBar1
            // 
            this.OrganBar1.Location = new System.Drawing.Point(3, 30);
            this.OrganBar1.Maximum = 8;
            this.OrganBar1.Name = "OrganBar1";
            this.OrganBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.OrganBar1.Size = new System.Drawing.Size(45, 104);
            this.OrganBar1.TabIndex = 14;
            this.OrganBar1.TickFrequency = 10;
            this.OrganBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.OrganBar1.ValueChanged += new System.EventHandler(this.OrganBar_ValueChanged);
            // 
            // PresetChooser
            // 
            this.PresetChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PresetChooser.FormattingEnabled = true;
            this.PresetChooser.Location = new System.Drawing.Point(3, 3);
            this.PresetChooser.Name = "PresetChooser";
            this.PresetChooser.Size = new System.Drawing.Size(259, 21);
            this.PresetChooser.TabIndex = 9;
            this.PresetChooser.SelectedIndexChanged += new System.EventHandler(this.PresetChooser_SelectedIndexChanged);
            // 
            // SoundChooser
            // 
            this.SoundChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SoundChooser.FormattingEnabled = true;
            this.SoundChooser.Items.AddRange(new object[] {
            "Sine",
            "Square",
            "Triangle",
            "Sawtooth",
            "Flute",
            "Hammond Organ"});
            this.SoundChooser.Location = new System.Drawing.Point(12, 350);
            this.SoundChooser.Name = "SoundChooser";
            this.SoundChooser.Size = new System.Drawing.Size(219, 21);
            this.SoundChooser.TabIndex = 8;
            this.SoundChooser.SelectedIndexChanged += new System.EventHandler(this.SoundChooser_SelectedIndexChanged);
            // 
            // FixNotesCheckbox
            // 
            this.FixNotesCheckbox.AutoSize = true;
            this.FixNotesCheckbox.Checked = true;
            this.FixNotesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FixNotesCheckbox.Location = new System.Drawing.Point(15, 464);
            this.FixNotesCheckbox.Name = "FixNotesCheckbox";
            this.FixNotesCheckbox.Size = new System.Drawing.Size(143, 17);
            this.FixNotesCheckbox.TabIndex = 5;
            this.FixNotesCheckbox.Text = "Snap to Chromatic Scale";
            this.FixNotesCheckbox.UseVisualStyleBackColor = true;
            // 
            // DisplayPanel
            // 
            this.DisplayPanel.Location = new System.Drawing.Point(12, 168);
            this.DisplayPanel.Name = "DisplayPanel";
            this.DisplayPanel.Size = new System.Drawing.Size(739, 150);
            this.DisplayPanel.TabIndex = 0;
            this.DisplayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DisplayPanel_Paint);
            // 
            // TremeloFrequencySlider
            // 
            this.TremeloFrequencySlider.Location = new System.Drawing.Point(176, 381);
            this.TremeloFrequencySlider.Maximum = 90;
            this.TremeloFrequencySlider.Minimum = 3;
            this.TremeloFrequencySlider.Name = "TremeloFrequencySlider";
            this.TremeloFrequencySlider.Size = new System.Drawing.Size(104, 45);
            this.TremeloFrequencySlider.TabIndex = 12;
            this.TremeloFrequencySlider.TickFrequency = 10;
            this.TremeloFrequencySlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TremeloFrequencySlider.Value = 10;
            this.TremeloFrequencySlider.Scroll += new System.EventHandler(this.TremeloFrequencySlider_Scroll);
            // 
            // ExtendedDisplayPanel
            // 
            this.ExtendedDisplayPanel.Location = new System.Drawing.Point(12, 12);
            this.ExtendedDisplayPanel.Name = "ExtendedDisplayPanel";
            this.ExtendedDisplayPanel.Size = new System.Drawing.Size(739, 150);
            this.ExtendedDisplayPanel.TabIndex = 1;
            this.ExtendedDisplayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ExtendedDisplayPanel_Paint);
            // 
            // MusicDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 504);
            this.Controls.Add(this.ExtendedDisplayPanel);
            this.Controls.Add(this.FrequencyLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OrganSettingsPanel);
            this.Controls.Add(this.SoundChooser);
            this.Controls.Add(this.FixNotesCheckbox);
            this.Controls.Add(this.DisplayPanel);
            this.Controls.Add(this.TremeloAmplitudeSlider);
            this.Controls.Add(this.TremeloFrequencySlider);
            this.KeyPreview = true;
            this.Name = "MusicDemo";
            this.Text = "MusicDemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MusicDemo_FormClosing);
            this.Load += new System.EventHandler(this.MusicDemo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MusicDemo_KeyDown);
            this.OrganSettingsPanel.ResumeLayout(false);
            this.OrganSettingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TremeloAmplitudeSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OrganBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TremeloFrequencySlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedPanel DisplayPanel;
        private CustomCheckbox FixNotesCheckbox;
        private System.Windows.Forms.Label FrequencyLabel;
        private CustomComboBox SoundChooser;
        private CustomComboBox PresetChooser;
        private System.Windows.Forms.Panel OrganSettingsPanel;
        private System.Windows.Forms.TrackBar TremeloAmplitudeSlider;
        private System.Windows.Forms.Label label1;
        private CustomTrackBar TremeloFrequencySlider;
        private CustomTrackBar OrganBar9;
        private CustomTrackBar OrganBar8;
        private CustomTrackBar OrganBar7;
        private CustomTrackBar OrganBar6;
        private CustomTrackBar OrganBar5;
        private CustomTrackBar OrganBar4;
        private CustomTrackBar OrganBar3;
        private CustomTrackBar OrganBar2;
        private CustomTrackBar OrganBar1;
        private DoubleBufferedPanel ExtendedDisplayPanel;

    }
}