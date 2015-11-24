namespace Water_Drop_Pro
{
    partial class Options_Form
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxShowSplashScreen = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoHideCamera = new System.Windows.Forms.CheckBox();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.groupBoxStatistics = new System.Windows.Forms.GroupBox();
            this.labelProcessRunsValue = new System.Windows.Forms.Label();
            this.labelSoftwareRunsValue = new System.Windows.Forms.Label();
            this.labelProcessRunsText = new System.Windows.Forms.Label();
            this.labelSoftwareRunsText = new System.Windows.Forms.Label();
            this.labelMinimumValveTime = new System.Windows.Forms.Label();
            this.numericUpDownMinimumValveTime = new System.Windows.Forms.NumericUpDown();
            this.labelUnitMilliseconds = new System.Windows.Forms.Label();
            this.groupBoxOptions.SuspendLayout();
            this.groupBoxStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumValveTime)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.BackColor = System.Drawing.Color.OrangeRed;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Location = new System.Drawing.Point(66, 283);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackColor = System.Drawing.Color.OrangeRed;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(147, 283);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxShowSplashScreen
            // 
            this.checkBoxShowSplashScreen.AutoSize = true;
            this.checkBoxShowSplashScreen.Location = new System.Drawing.Point(15, 30);
            this.checkBoxShowSplashScreen.Name = "checkBoxShowSplashScreen";
            this.checkBoxShowSplashScreen.Size = new System.Drawing.Size(168, 17);
            this.checkBoxShowSplashScreen.TabIndex = 2;
            this.checkBoxShowSplashScreen.Text = "Show splash screen at startup";
            this.checkBoxShowSplashScreen.UseVisualStyleBackColor = true;
            this.checkBoxShowSplashScreen.CheckedChanged += new System.EventHandler(this.checkBoxShowSplashScreen_CheckedChanged);
            // 
            // checkBoxAutoHideCamera
            // 
            this.checkBoxAutoHideCamera.AutoSize = true;
            this.checkBoxAutoHideCamera.Location = new System.Drawing.Point(15, 60);
            this.checkBoxAutoHideCamera.Name = "checkBoxAutoHideCamera";
            this.checkBoxAutoHideCamera.Size = new System.Drawing.Size(188, 17);
            this.checkBoxAutoHideCamera.TabIndex = 3;
            this.checkBoxAutoHideCamera.Text = "Automatically hide camera window";
            this.checkBoxAutoHideCamera.UseVisualStyleBackColor = true;
            this.checkBoxAutoHideCamera.CheckedChanged += new System.EventHandler(this.checkBoxAutoHideCamera_CheckedChanged);
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxOptions.Controls.Add(this.labelUnitMilliseconds);
            this.groupBoxOptions.Controls.Add(this.numericUpDownMinimumValveTime);
            this.groupBoxOptions.Controls.Add(this.labelMinimumValveTime);
            this.groupBoxOptions.Controls.Add(this.checkBoxShowSplashScreen);
            this.groupBoxOptions.Controls.Add(this.checkBoxAutoHideCamera);
            this.groupBoxOptions.ForeColor = System.Drawing.Color.White;
            this.groupBoxOptions.Location = new System.Drawing.Point(12, 12);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(210, 152);
            this.groupBoxOptions.TabIndex = 4;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "Options";
            // 
            // groupBoxStatistics
            // 
            this.groupBoxStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxStatistics.Controls.Add(this.labelProcessRunsValue);
            this.groupBoxStatistics.Controls.Add(this.labelSoftwareRunsValue);
            this.groupBoxStatistics.Controls.Add(this.labelProcessRunsText);
            this.groupBoxStatistics.Controls.Add(this.labelSoftwareRunsText);
            this.groupBoxStatistics.ForeColor = System.Drawing.Color.White;
            this.groupBoxStatistics.Location = new System.Drawing.Point(12, 170);
            this.groupBoxStatistics.Name = "groupBoxStatistics";
            this.groupBoxStatistics.Size = new System.Drawing.Size(210, 100);
            this.groupBoxStatistics.TabIndex = 5;
            this.groupBoxStatistics.TabStop = false;
            this.groupBoxStatistics.Text = "Statistics";
            // 
            // labelProcessRunsValue
            // 
            this.labelProcessRunsValue.AutoSize = true;
            this.labelProcessRunsValue.Location = new System.Drawing.Point(100, 60);
            this.labelProcessRunsValue.Name = "labelProcessRunsValue";
            this.labelProcessRunsValue.Size = new System.Drawing.Size(13, 13);
            this.labelProcessRunsValue.TabIndex = 3;
            this.labelProcessRunsValue.Text = "0";
            // 
            // labelSoftwareRunsValue
            // 
            this.labelSoftwareRunsValue.AutoSize = true;
            this.labelSoftwareRunsValue.Location = new System.Drawing.Point(100, 30);
            this.labelSoftwareRunsValue.Name = "labelSoftwareRunsValue";
            this.labelSoftwareRunsValue.Size = new System.Drawing.Size(13, 13);
            this.labelSoftwareRunsValue.TabIndex = 2;
            this.labelSoftwareRunsValue.Text = "0";
            // 
            // labelProcessRunsText
            // 
            this.labelProcessRunsText.AutoSize = true;
            this.labelProcessRunsText.Location = new System.Drawing.Point(12, 60);
            this.labelProcessRunsText.Name = "labelProcessRunsText";
            this.labelProcessRunsText.Size = new System.Drawing.Size(71, 13);
            this.labelProcessRunsText.TabIndex = 1;
            this.labelProcessRunsText.Text = "Process runs:";
            // 
            // labelSoftwareRunsText
            // 
            this.labelSoftwareRunsText.AutoSize = true;
            this.labelSoftwareRunsText.Location = new System.Drawing.Point(12, 30);
            this.labelSoftwareRunsText.Name = "labelSoftwareRunsText";
            this.labelSoftwareRunsText.Size = new System.Drawing.Size(75, 13);
            this.labelSoftwareRunsText.TabIndex = 0;
            this.labelSoftwareRunsText.Text = "Software runs:";
            // 
            // labelMinimumValveTime
            // 
            this.labelMinimumValveTime.AutoSize = true;
            this.labelMinimumValveTime.Location = new System.Drawing.Point(12, 92);
            this.labelMinimumValveTime.Name = "labelMinimumValveTime";
            this.labelMinimumValveTime.Size = new System.Drawing.Size(142, 13);
            this.labelMinimumValveTime.TabIndex = 4;
            this.labelMinimumValveTime.Text = "Minimum Valve Time (Open):";
            // 
            // numericUpDownMinimumValveTime
            // 
            this.numericUpDownMinimumValveTime.Location = new System.Drawing.Point(15, 115);
            this.numericUpDownMinimumValveTime.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMinimumValveTime.Name = "numericUpDownMinimumValveTime";
            this.numericUpDownMinimumValveTime.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMinimumValveTime.TabIndex = 5;
            this.numericUpDownMinimumValveTime.ValueChanged += new System.EventHandler(this.numericUpDownMinimumValveTime_ValueChanged);
            // 
            // labelUnitMilliseconds
            // 
            this.labelUnitMilliseconds.AutoSize = true;
            this.labelUnitMilliseconds.Location = new System.Drawing.Point(141, 117);
            this.labelUnitMilliseconds.Name = "labelUnitMilliseconds";
            this.labelUnitMilliseconds.Size = new System.Drawing.Size(26, 13);
            this.labelUnitMilliseconds.TabIndex = 6;
            this.labelUnitMilliseconds.Text = "[ms]";
            // 
            // Options_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(234, 318);
            this.Controls.Add(this.groupBoxStatistics);
            this.Controls.Add(this.groupBoxOptions);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.ForeColor = System.Drawing.Color.White;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(250, 305);
            this.Name = "Options_Form";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            this.groupBoxStatistics.ResumeLayout(false);
            this.groupBoxStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumValveTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxShowSplashScreen;
        private System.Windows.Forms.CheckBox checkBoxAutoHideCamera;
        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.GroupBox groupBoxStatistics;
        private System.Windows.Forms.Label labelProcessRunsValue;
        private System.Windows.Forms.Label labelSoftwareRunsValue;
        private System.Windows.Forms.Label labelProcessRunsText;
        private System.Windows.Forms.Label labelSoftwareRunsText;
        private System.Windows.Forms.Label labelMinimumValveTime;
        private System.Windows.Forms.Label labelUnitMilliseconds;
        private System.Windows.Forms.NumericUpDown numericUpDownMinimumValveTime;
    }
}