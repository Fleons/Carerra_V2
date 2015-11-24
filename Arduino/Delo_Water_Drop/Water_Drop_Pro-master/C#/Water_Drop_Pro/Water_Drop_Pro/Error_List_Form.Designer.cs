namespace Water_Drop_Pro
{
    partial class Error_List_Form
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
            this.textBoxErrorList = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.BackColor = System.Drawing.Color.OrangeRed;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Location = new System.Drawing.Point(197, 226);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxErrorList
            // 
            this.textBoxErrorList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxErrorList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxErrorList.ForeColor = System.Drawing.Color.White;
            this.textBoxErrorList.Location = new System.Drawing.Point(12, 12);
            this.textBoxErrorList.Multiline = true;
            this.textBoxErrorList.Name = "textBoxErrorList";
            this.textBoxErrorList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxErrorList.Size = new System.Drawing.Size(260, 208);
            this.textBoxErrorList.TabIndex = 1;
            // 
            // Error_List_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBoxErrorList);
            this.Controls.Add(this.buttonOK);
            this.ForeColor = System.Drawing.Color.White;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Error_List_Form";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error List";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxErrorList;
    }
}