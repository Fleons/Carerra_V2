using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Water_Drop_Pro
{
    public partial class Options_Form : Form
    {
        #region VARIABLES

        bool Show_Splash_Screen = Main_Form.Show_Splash_Screen;
        bool Auto_Hide_Camera = Main_Form.Auto_Hide_Camera;
        int Minimum_Valve_Time = Main_Form.Minimum_Valve_Time;

        #endregion

        #region START/END FORM

        // BASIC STARZUP
        public Options_Form()
        {
            InitializeComponent();

            // SET CHECKBOXES
            checkBoxShowSplashScreen.Checked = Show_Splash_Screen;
            checkBoxAutoHideCamera.Checked = Auto_Hide_Camera;

            // SET NUMERIC UP-DOWN
            numericUpDownMinimumValveTime.Value = Minimum_Valve_Time;

            // SET VALUES
            labelSoftwareRunsValue.Text = Main_Form.Software_Runs.ToString();
            labelProcessRunsValue.Text = Main_Form.Process_Runs.ToString();

            // SET DESIGN
            switch (Main_Form.Design_Sheme)
            {
                case 0:
                    // Error
                    break;
                case 1:
                    this.BackColor = Color.FromArgb(50, 50, 50);
                    this.ForeColor = Color.White;
                    groupBoxOptions.ForeColor = Color.White;
                    groupBoxStatistics.ForeColor = Color.White;
                    break;
                case 2:
                    this.BackColor = Color.White;
                    this.ForeColor = Color.Black;
                    groupBoxOptions.ForeColor = Color.Black;
                    groupBoxStatistics.ForeColor = Color.Black;
                    break;
                case 3:
                    // Default
                    break;
            }
        }

        #endregion

        #region BUTTONS

        // BUTTON OK
        private void buttonOK_Click(object sender, EventArgs e)
        {
            Main_Form.Show_Splash_Screen = Show_Splash_Screen;
            Main_Form.Auto_Hide_Camera = Auto_Hide_Camera;
            Main_Form.Minimum_Valve_Time = Minimum_Valve_Time;
            this.Close();
        }

        // BUTTON CANCEL
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region CHECKBOX & NUMERIC-UP-DOWN WATCHER

        // SHOW SPASH SCREEN
        private void checkBoxShowSplashScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowSplashScreen.Checked)
                Show_Splash_Screen = true;
            else
                Show_Splash_Screen = false;
        }
        
        // AUTO HIDE CAMERA
        private void checkBoxAutoHideCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoHideCamera.Checked)
                Auto_Hide_Camera = true;
            else
                Auto_Hide_Camera = false;
        }

        // MINIMUM VALVE TIME
        private void numericUpDownMinimumValveTime_ValueChanged(object sender, EventArgs e)
        {
            Minimum_Valve_Time = Convert.ToInt32(numericUpDownMinimumValveTime.Value);
        }

        #endregion
    }
}
