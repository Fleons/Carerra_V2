﻿namespace carerra_gui
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton_mann = new System.Windows.Forms.RadioButton();
            this.radioButton_frau = new System.Windows.Forms.RadioButton();
            this.radioButton_kind = new System.Windows.Forms.RadioButton();
            this.button_datenaufnahme = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.chart_kraftverlauf = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.hScrollBar_Nullpunkt = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Bahn_aktiviert = new System.Windows.Forms.Button();
            this.Label_Maxgeschw = new System.Windows.Forms.Label();
            this.numericUpDown_VMAX = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_kraftverlauf)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VMAX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button_connect
            // 
            resources.ApplyResources(this.button_connect, "button_connect");
            this.button_connect.Name = "button_connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton_mann);
            this.panel1.Controls.Add(this.radioButton_frau);
            this.panel1.Controls.Add(this.radioButton_kind);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // radioButton_mann
            // 
            resources.ApplyResources(this.radioButton_mann, "radioButton_mann");
            this.radioButton_mann.Checked = true;
            this.radioButton_mann.Name = "radioButton_mann";
            this.radioButton_mann.TabStop = true;
            this.radioButton_mann.UseVisualStyleBackColor = true;
            // 
            // radioButton_frau
            // 
            resources.ApplyResources(this.radioButton_frau, "radioButton_frau");
            this.radioButton_frau.Name = "radioButton_frau";
            this.radioButton_frau.UseVisualStyleBackColor = true;
            // 
            // radioButton_kind
            // 
            resources.ApplyResources(this.radioButton_kind, "radioButton_kind");
            this.radioButton_kind.Name = "radioButton_kind";
            this.radioButton_kind.UseVisualStyleBackColor = true;
            // 
            // button_datenaufnahme
            // 
            resources.ApplyResources(this.button_datenaufnahme, "button_datenaufnahme");
            this.button_datenaufnahme.Name = "button_datenaufnahme";
            this.button_datenaufnahme.UseVisualStyleBackColor = true;
            this.button_datenaufnahme.Click += new System.EventHandler(this.button_datenaufnahme_Click);
            // 
            // timer
            // 
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // chart_kraftverlauf
            // 
            this.chart_kraftverlauf.BackColor = System.Drawing.SystemColors.Control;
            chartArea1.Name = "ChartArea1";
            this.chart_kraftverlauf.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_kraftverlauf.Legends.Add(legend1);
            resources.ApplyResources(this.chart_kraftverlauf, "chart_kraftverlauf");
            this.chart_kraftverlauf.Name = "chart_kraftverlauf";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Red;
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            series1.Legend = "Legend1";
            series1.Name = "V1";
            this.chart_kraftverlauf.Series.Add(series1);
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.hScrollBar_Nullpunkt);
            this.panel2.Controls.Add(this.label1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // hScrollBar_Nullpunkt
            // 
            resources.ApplyResources(this.hScrollBar_Nullpunkt, "hScrollBar_Nullpunkt");
            this.hScrollBar_Nullpunkt.Maximum = 255;
            this.hScrollBar_Nullpunkt.Name = "hScrollBar_Nullpunkt";
            this.hScrollBar_Nullpunkt.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Nullpunkt_Scroll);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // button_Bahn_aktiviert
            // 
            resources.ApplyResources(this.button_Bahn_aktiviert, "button_Bahn_aktiviert");
            this.button_Bahn_aktiviert.Name = "button_Bahn_aktiviert";
            this.button_Bahn_aktiviert.UseVisualStyleBackColor = true;
            this.button_Bahn_aktiviert.Click += new System.EventHandler(this.button_Bahn_aktiviert_Click);
            // 
            // Label_Maxgeschw
            // 
            resources.ApplyResources(this.Label_Maxgeschw, "Label_Maxgeschw");
            this.Label_Maxgeschw.Name = "Label_Maxgeschw";
            // 
            // numericUpDown_VMAX
            // 
            resources.ApplyResources(this.numericUpDown_VMAX, "numericUpDown_VMAX");
            this.numericUpDown_VMAX.Name = "numericUpDown_VMAX";
            this.numericUpDown_VMAX.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown_VMAX.ValueChanged += new System.EventHandler(this.numericUpDown_VMAX_ValueChanged);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.numericUpDown_VMAX);
            this.Controls.Add(this.Label_Maxgeschw);
            this.Controls.Add(this.button_Bahn_aktiviert);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.chart_kraftverlauf);
            this.Controls.Add(this.button_datenaufnahme);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.comboBox1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_kraftverlauf)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VMAX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton_mann;
        private System.Windows.Forms.RadioButton radioButton_frau;
        private System.Windows.Forms.RadioButton radioButton_kind;
        private System.Windows.Forms.Button button_datenaufnahme;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_kraftverlauf;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.HScrollBar hScrollBar_Nullpunkt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Bahn_aktiviert;
        private System.Windows.Forms.Label Label_Maxgeschw;
        private System.Windows.Forms.NumericUpDown numericUpDown_VMAX;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

