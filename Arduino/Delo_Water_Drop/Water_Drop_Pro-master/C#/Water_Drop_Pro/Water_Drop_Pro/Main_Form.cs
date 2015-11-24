/*
    DELOARTS RESEARCH INC.
    WATER DROP PRO 1.2.0.0
	08.08.2015
	
	DESCRIPTION
		Water Drop (Pro) is a host-based microcontroller project, which allows you to capture a water
		drop by using electro-magnetic valves. This code is runs on Arduino devices with a host (PC, Raspberry, etc.).
        
        This is the host software! It runs currently only on Windows OS and .Net 4.5

	MAIN CHANGES TO WATER DROP ADVANCED
		- It is not standalone anymore
		- The parameters are now set with a host (via USB)
		- It supports valves

	MICROCONTROLLER
		- Arduino UNO Rev3
		- Arduino Nano v3
 
    HOST (any CPU, .NET Framework 4.5)
        - Microsoft Windows XP
        - Microsoft Windows Vista
        - Microsoft Windows 7
        - Microsoft Windows 8
        - Microsoft Windows 8.1
        - Microsoft Windows 10 
 
    CAMERA
        - Currently all Canon EOS DSLR Models (2015)

	PROTOCOL
		There are multiple keywords for the protocol

		- BEGIN:	Start connection with Arduino/PC 	(A <-> PC)
		- END:		End the connection 					(A <-> PC)
		- DATA: 	A data block is following 			(A <-> PC)
        - COMMAND:  A command block is following        (A <-  PC)
		- COMPLETE:	The Arduino received all data 		(A  -> PC)
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Xml;
using EDSDKLib;

namespace Water_Drop_Pro
{
    public partial class Main_Form : Form
    {
        #region VARAIBLES

        // FORM VARAIBLES
        int[] Maximum_Size = new int[] { 831, 495 };
        int[] Minimum_Size = new int[] { 637, 495 };

        public static int Design_Sheme = 1; // 0: Error, 1: Dark, 2: Bright, 3: Default 

        bool Camera_List_Visible = true;
        bool Maximized = false;

        // RUNTIME
        bool Startup = true;
        bool Saved = true;
        bool Connected_Arduino = false;
        bool Connected_Camera = false;
        bool Live_View_Active = false;

        // SERIAL
        int Port_Amount = 0;
        bool Allow_To_Connect = false;
        bool Working = false;
        string Connection_Phrase = "BEGIN";
        string Disconnection_Phrase = "END";
        string Data_Phrase = "DATA";
        string Complete_Phrase = "COMPLETE";
        string Command_Phrase = "COMMAND";
        string Command_Toggle_Valve = "TOGGLE";

        // ARDUINO
        int[] Arduino_Software_Version = new int[3];
        bool Valve_1_Open = false;
        bool Valve_2_Open = false;
        bool Valve_3_Open = false;

        // PATH (DIRECTORIES)
        string Settings_Path = "Settings.wds";
        string Error_Log_Path = "ErrorLog.wds";
        const string DLL_Path = "EDSDK.dll";
        string SaveToHost_Path = string.Empty;
        string Save_Form_Path = string.Empty;

        // SETTINGS
        List<string> Settings = new List<string>();
        public static int Software_Runs = 10;
        public static int Process_Runs = 0;

        // OPTIONS
        public static bool Show_Splash_Screen = false;
        public static bool Auto_Hide_Camera = false;
        public static int Minimum_Valve_Time = 0;

        // SUBROUTINES
        public static List<string> Error_Log = new List<string>();

        // CANON
        SDKHandler CameraHandler;
        List<int> AvList;
        List<int> TvList;
        List<int> ISOList;
        List<Camera> Camera_List;
        Bitmap Evf_Bmp;
        int LVBw, LVBh, w, h;
        float LVBratio, LVration;
        

        #endregion

        #region START/END FORM

        // BASIC STARTUP
        public Main_Form()
        {
            InitializeComponent();

            getSettings();
            initializeCamera();

            groupBoxLiveView.Location = new Point(12, 35);

            if (Show_Splash_Screen)
            {
                Form SplashScreen = new Splash_Screen();
                SplashScreen.Show();
                Thread.Sleep(1000);
                SplashScreen.Close();
            }

            Startup = false;
            Saved = true;

            statusLabel.Text = "Ready.";
        }

        // GET SETTINGS
        private void getSettings()
        {
            if (File.Exists(Settings_Path))
            {   
                try
                {
                    File.SetAttributes(Settings_Path, FileAttributes.Normal);

                    Settings = File.ReadAllLines(Settings_Path).ToList();

                    // MAIN FORM SETTINGS
                    Software_Runs = Convert.ToInt32(Settings[0]) + 1;
                    Process_Runs = Convert.ToInt32(Settings[1]);

                    numericUpDownTriggerHeight.Value = Convert.ToInt32(Settings[2]);
                    numericUpDownTriggerDelay.Value = Convert.ToInt32(Settings[3]);

                    numericUpDownRepeatsAmount.Value = Convert.ToInt32(Settings[4]);
                    numericUpDownRepeatsPause.Value = Convert.ToInt32(Settings[5]);

                    checkBoxCameraSettingsOfflineAutofocus.Checked = Convert.ToBoolean(Settings[6]);
                    checkBoxCameraSettingsOfflineMirrorLockup.Checked = Convert.ToBoolean(Settings[7]);

                    checkBoxSaveToHost.Checked = Convert.ToBoolean(Settings[8]);
                    SaveToHost_Path = Convert.ToString(Settings[9]);

                    Design_Sheme = Convert.ToInt32(Settings[10]);
                    switch (Design_Sheme)
                    {
                        case 0:
                            // Error
                            break;
                        case 1:
                            darkToolStripMenuItem_Click(null, null);
                            break;
                        case 2:
                            brightToolStripMenuItem_Click(null, null);
                            break;
                        case 3:
                            // Default
                            break;
                    }
                    Camera_List_Visible = !Convert.ToBoolean(Settings[11]);
                    hideShowCameraListToolStripMenuItem_Click(null, null);

                    // OPTIONS FORM SETTINGS
                    Show_Splash_Screen = Convert.ToBoolean(Settings[12]);
                    Auto_Hide_Camera = Convert.ToBoolean(Settings[13]);
                    Minimum_Valve_Time = Convert.ToInt32(Settings[14]);

                    setMinimumValveTime();

                    if (Auto_Hide_Camera)
                    {
                        this.MaximumSize = new Size(Minimum_Size[0], Minimum_Size[1]);
                        this.Size = new Size(Minimum_Size[0], Minimum_Size[1]);
                    }

                }
                catch (Exception)
                {
                    reportError("An error accured by attempting to load the settings.", true);
                }
            }
        }

        // GET CAMERA FILES
        private void initializeCamera()
        {
            /* The camera will be initialized, when the neccessary dll's are available!
             * Otherwise the camera function will be disabled.
             */
            if (File.Exists(DLL_Path))
            {
                try
                {
                    CameraHandler = new SDKHandler();
                    CameraHandler.CameraAdded += new SDKHandler.CameraAddedHandler(SDK_CameraAdded);
                    CameraHandler.LiveViewUpdated += new SDKHandler.StreamUpdate(SDK_LiveViewUpdated);
                    LVBw = pictureBoxLiveView.Width;
                    LVBh = pictureBoxLiveView.Height;
                    refreshCamera();
                }
                catch (DllNotFoundException)
                {
                    reportError("Canon DLLs not found!", false);
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
        }

        // CLOSE FORM
        private void formClosingEvent(object sender, FormClosingEventArgs e)
        {
            // Close all open connections
            try
            {
                // End session with Arduino
                if (serialPort.IsOpen)
                {
                    serialOutput(Disconnection_Phrase);
                    serialPort.Close();
                }
                // End session with camera
                if (CameraHandler != null)
                {
                    CameraHandler.Dispose();
                }
            }
            catch (Exception ex)
            {
                reportError(ex.Message, false);
            }

            while (Settings.Count < 15)
                Settings.Add(string.Empty);
            
            Settings[0] = Software_Runs.ToString();
            Settings[1] = Process_Runs.ToString();
            Settings[2] = numericUpDownTriggerHeight.Value.ToString();
            Settings[3] = numericUpDownTriggerDelay.Value.ToString();
            Settings[4] = numericUpDownRepeatsAmount.Value.ToString();
            Settings[5] = numericUpDownRepeatsPause.Value.ToString();
            Settings[6] = checkBoxCameraSettingsOfflineAutofocus.Checked.ToString();
            Settings[7] = checkBoxCameraSettingsOfflineMirrorLockup.Checked.ToString();
            Settings[8] = checkBoxSaveToHost.Checked.ToString();
            Settings[9] = SaveToHost_Path.ToString();
            Settings[10] = Design_Sheme.ToString();
            Settings[11] = Camera_List_Visible.ToString();
            Settings[12] = Show_Splash_Screen.ToString();
            Settings[13] = Auto_Hide_Camera.ToString();
            Settings[14] = Minimum_Valve_Time.ToString();
            
            try
            {
                File.WriteAllLines(Settings_Path, Settings);
                File.SetAttributes(Settings_Path, FileAttributes.ReadOnly);
            }
            catch (Exception)
            {
                reportError("An error accured by attempting to save the settings.", true);
            }

            if (!Saved)
            {
                DialogResult Result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButtons.YesNoCancel);
                if (Result == DialogResult.Yes)
                    saveToolStripMenuItem_Click(null, null);
            }
        }

        #endregion

        #region SERIAL

        // RECEIVE SERIAL DATA
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string Received = string.Empty;

            try
            {
                Received = serialPort.ReadTo(";");
                // Clear the buffer
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
            catch (Exception ex)
            {
                Received = string.Empty;
                // Error log
                reportError(ex.Message, false);
            }

            #region STARTUP PHRASE
            if (Received.StartsWith(Connection_Phrase))
            {
                try
                {
                    if (Allow_To_Connect)
                    {
                        timerConnectionTimeout.Stop();
                        // Update button, combo box & group box
                        buttonConnectArduino.Invoke(new Action(() => buttonConnectArduino.Text = "DISCONNECT"));
                        buttonConnectArduino.Invoke(new Action(() => buttonConnectArduino.Enabled = true));
                        listBoxArduino.Invoke(new Action(() => listBoxArduino.Enabled = false));
                        // Status for user
                        statusLabel.Text = "Connect to " + serialPort.PortName + ". Software Version "
                            + Arduino_Software_Version[0] + "." + Arduino_Software_Version[1] + "." + Arduino_Software_Version[2] + ".";
                    }
                    Allow_To_Connect = false;
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
            #endregion

            #region END PHRASE
            if (Received.StartsWith(Disconnection_Phrase))
            {
                try
                {
                    // Delay a short time
                    Thread.Sleep(250);
                    // Close the port
                    serialPort.Close();
                    // Update button, combo box and group boxes
                    buttonConnectArduino.Invoke(new Action(() => buttonConnectArduino.Text = "CONNECT"));
                    buttonConnectArduino.Invoke(new Action(() => buttonConnectArduino.Enabled = true));
                    listBoxArduino.Invoke(new Action(() => listBoxArduino.Enabled = true));
                    groupBoxValve1.Invoke(new Action(() => groupBoxValve1.Enabled = true));
                    groupBoxValve2.Invoke(new Action(() => groupBoxValve2.Enabled = true));
                    groupBoxValve3.Invoke(new Action(() => groupBoxValve3.Enabled = true));
                    // Status for user
                    statusLabel.Text = "Ready.";
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
            #endregion

            #region DATA PHRASE
            if (Received.StartsWith(Data_Phrase))
            {
                try
                {
                    // Status for user
                    statusLabel.Text = "Incomming data from " + serialPort.PortName + ".";
                    // Get the data from the string
                    string[] Split_Seperator = new string[] { "|" };
                    string[] Split_Buffer = Received.Split(Split_Seperator, StringSplitOptions.None);
                    // Process every data block
                    // VERSION: The software version from the arduino
                    if (Split_Buffer[1] == "VERSION")
                    {
                        string Version = Split_Buffer[2];
                        // Get single parts of software version
                        Split_Seperator = new string[] { "." };
                        Split_Buffer = Version.Split(Split_Seperator, StringSplitOptions.RemoveEmptyEntries);
                        for (var x = 0; x < Split_Buffer.Length; x++)
                            Arduino_Software_Version[x] = Convert.ToInt32(Split_Buffer[x]);
                        // Only allow to connect, when major software is > 0
                        if (Arduino_Software_Version[0] > 0)
                        {
                            Allow_To_Connect = true;
                            Connected_Arduino = true;
                        }
                        else
                        {
                            Allow_To_Connect = false;
                            // Sleep, for the arduino will send another data block
                            Thread.Sleep(250);
                            // Send a new connection request
                            serialOutput(Connection_Phrase);
                        }
                    }
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
            #endregion

            #region COMPLETE PHRASE
            if (Received.StartsWith(Complete_Phrase))
            {
                try
                {
                    // Get the data from the string
                    string[] Split_Seperator = new string[] { "|" };
                    string[] Split_Buffer = Received.Split(Split_Seperator, StringSplitOptions.None);
                    // START: Everything is set up, working process is no in action
                    if (Split_Buffer[1] == "START")
                    {
                        statusLabel.Text = "Working in progress.";
                    }
                    // END: Working process has been ended.
                    if (Split_Buffer[1] == "END")
                    {
                        // Enable buttons
                        buttonConnectArduino.Invoke(new Action(() => buttonConnectArduino.Enabled = true));
                        buttonConnectCamera.Invoke(new Action(() => buttonConnectCamera.Enabled = true));
                        buttonTrigger.Invoke(new Action(() => buttonTrigger.Enabled = true));
                        buttonLiveView.Invoke(new Action(() => buttonLiveView.Enabled = true));
                        buttonStartProcess.Invoke(new Action(() => buttonStartProcess.Enabled = true));
                        buttonToggleValve1.Invoke(new Action(() => buttonToggleValve1.Enabled = true));
                        buttonToggleValve2.Invoke(new Action(() => buttonToggleValve2.Enabled = true));
                        buttonToggleValve3.Invoke(new Action(() => buttonToggleValve3.Enabled = true));
                        Working = false;
                        // Status for user
                        statusLabel.Text = "Ready.";
                    }
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
            #endregion
        }

        // SEND SERIAL DATA
        private void serialOutput(string Parameter_Data)
        {
            try
            {
                serialPort.Write(Parameter_Data + ";");
            }
            catch (Exception ex)
            {
                reportError(ex.Message, true);
            }
        }

        #endregion

        #region BUTTONS

        // CONNECT ARDUINO
        private void buttonConnectArduino_Click(object sender, EventArgs e)
        {
            if (Port_Amount > 0)
            {
                try
                {
                    // Disconnect
                    if (serialPort.IsOpen)
                    {
                        Connected_Arduino = false;
                        // Status for user
                        statusLabel.Text = "Trying to disconnect from " + serialPort.PortName;
                        // Talk to Arduino
                        serialOutput(Disconnection_Phrase);
                    }
                    // Connect
                    else if (listBoxArduino.Items.Count > 0)
                    {
                        buttonConnectArduino.Enabled = false;
                        listBoxArduino.Enabled = false;
                        // Setup serial port
                        serialPort.PortName = listBoxArduino.SelectedItem.ToString();
                        serialPort.BaudRate = 9600;
                        serialPort.Parity = Parity.None;
                        serialPort.DataBits = 8;
                        // Status for user
                        statusLabel.Text = "Trying to connect to " + serialPort.PortName;
                        // Start timeout for connection
                        timerConnectionTimeout.Start();
                        // Open the port and send the startup phrase
                        try
                        {
                            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                            serialPort.Open();
                            // Now delay a short time, because the Arduino resets itself, when it's connected to the host
                            Thread.Sleep(2000);
                            serialOutput(Connection_Phrase);
                        }
                        catch (Exception ex)
                        {
                            // Update button
                            buttonConnectArduino.Enabled = true;
                            listBoxArduino.Enabled = true;
                            // Error log
                            reportError(ex.Message, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, true);
                }
            }
        }

        // START PROCESS
        private void buttonStartProcess_Click(object sender, EventArgs e)
        {
            if (Connected_Arduino)
            {
                try
                {
                    Process_Runs++;
                    Working = true;

                    buttonConnectArduino.Invoke(new Action(() => buttonConnectArduino.Enabled = false));
                    buttonConnectCamera.Invoke(new Action(() => buttonConnectCamera.Enabled = false));
                    buttonTrigger.Invoke(new Action(() => buttonTrigger.Enabled = false));
                    buttonLiveView.Invoke(new Action(() => buttonLiveView.Enabled = false));
                    buttonStartProcess.Invoke(new Action(() => buttonStartProcess.Enabled = false));
                    buttonToggleValve1.Invoke(new Action(() => buttonToggleValve1.Enabled = false));
                    buttonToggleValve2.Invoke(new Action(() => buttonToggleValve2.Enabled = false));
                    buttonToggleValve3.Invoke(new Action(() => buttonToggleValve3.Enabled = false));

                    string[] Send_Data = new string[42];

                    // Status for user
                    statusLabel.Text = "Sending data to " + serialPort.PortName + ".";

                    // Disable buttons
                    buttonStartProcess.Enabled = false;
                    buttonConnectArduino.Enabled = false;
                    // Get the data from the user interface (depending on arduino version)
                    if (Arduino_Software_Version[0] == 1 && Arduino_Software_Version[1] > 0)
                        Send_Data = getDataList(true);
                    else
                        Send_Data = getDataList(false);
                    // Generate the protocol
                    string Send_Protocol = string.Empty;
                    for (int x = 0; x < Send_Data.Length; x++)
                    {
                        Send_Protocol += Send_Data[x];

                        if (x < Send_Data.Length - 1)
                            Send_Protocol += "|";
                    }
                    // Send the protocol
                    serialOutput(Send_Protocol);
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, true);
                }
            }
            else
            {
                statusLabel.Text = "Client is not connected.";
            }
        }

        // CONNECT CAMERA
        private void buttonConnectCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (CameraHandler.CameraSessionOpen)
                {
                    disconnectCamera();
                    statusLabel.Text = "Camera disconnected.";
                    // Disable live view
                    groupBoxLiveView.Visible = false;
                    // Set form
                    if (Maximized)
                    {
                        Maximized = false;
                        FormBorderStyle = FormBorderStyle.FixedSingle;
                        WindowState = FormWindowState.Normal;
                    }
                    // Set form
                    if (Auto_Hide_Camera)
                    {
                        this.MaximumSize = new Size(Maximum_Size[0], Maximum_Size[1]);
                        this.Size = new Size(Maximum_Size[0], Maximum_Size[1]);
                    }
                }
                else
                {
                    connectCamera();
                    statusLabel.Text = "Camera connected.";

                    if (checkBoxSaveToHost.Checked)
                    {
                        CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                        CameraHandler.SetCapacity();
                        CameraHandler.ImageSaveDirectory = SaveToHost_Path;
                    }
                    else
                    {
                        CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Camera);
                    }
                }
            }
            catch (Exception ex)
            {
                reportError(ex.Message, false);
            }
        }

        // TRIGGER CAMERA
        private void buttonTrigger_Click(object sender, EventArgs e)
        {
            if (Connected_Camera)
            {
                try
                {
                    if ((string)comboBoxTv.SelectedItem == "Bulb")
                        CameraHandler.TakePhoto((uint)numericUpDownBulb.Value);
                    else
                        CameraHandler.TakePhoto();
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
        }

        // START/END LIVE VIEW
        private void buttonLiveView_Click(object sender, EventArgs e)
        {
            if (Connected_Camera)
            {
                try
                {
                    if (!CameraHandler.IsLiveViewOn)
                    {
                        CameraHandler.StartLiveView();
                        groupBoxLiveView.Visible = true;
                        fullscreenToolStripMenuItem.Visible = true;
                        buttonLiveView.Text = "END LV";
                    }
                    else
                    {
                        CameraHandler.StopLiveView();
                        groupBoxLiveView.Visible = false;
                        fullscreenToolStripMenuItem.Visible = false;
                        buttonLiveView.Text = "LIVE VIEW";
                        if (Maximized)
                        {
                            Maximized = false;
                            this.MaximumSize = new Size(Maximum_Size[0], Maximum_Size[1]);
                            this.Size = new Size(Maximum_Size[0], Maximum_Size[1]);
                            FormBorderStyle = FormBorderStyle.FixedSingle;
                            WindowState = FormWindowState.Normal;
                        }
                    }
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
        }

        // TOGGLE VALVE 1
        private void buttonToggleValve1_Click(object sender, EventArgs e)
        {
            if (Connected_Arduino)
            {
                try
                {
                    if (Valve_1_Open)
                    {
                        serialOutput(Command_Phrase + "|" + Command_Toggle_Valve + "|10");
                        Valve_1_Open = false;
                        statusLabel.Text = "Valve 1 closed.";
                    }
                    else
                    {
                        serialOutput(Command_Phrase + "|" + Command_Toggle_Valve + "|11");
                        Valve_1_Open = true;
                        statusLabel.Text = "Valve 1 opened.";
                    }
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
            else
            {
                statusLabel.Text = "Client is not connected.";
            }
        }

        // TOGGLE VALVE 2
        private void buttonToggleValve2_Click(object sender, EventArgs e)
        {
            if (Connected_Arduino)
            {
                try
                {
                    if (Valve_2_Open)
                    {
                        serialOutput(Command_Phrase + "|" + Command_Toggle_Valve + "|20");
                        Valve_2_Open = false;
                        statusLabel.Text = "Valve 2 closed.";
                    }
                    else
                    {
                        serialOutput(Command_Phrase + "|" + Command_Toggle_Valve + "|21");
                        Valve_2_Open = true;
                        statusLabel.Text = "Valve 2 opened.";
                    }
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
            else
            {
                statusLabel.Text = "Client is not connected.";
            }
        }

        // TOGGLE VALVE 3
        private void buttonToggleValve3_Click(object sender, EventArgs e)
        {
            if (Connected_Arduino)
            {
                try
                {
                    if (Valve_3_Open)
                    {
                        serialOutput(Command_Phrase + "|" + Command_Toggle_Valve + "|30");
                        Valve_3_Open = false;
                        statusLabel.Text = "Valve 3 closed.";
                    }
                    else
                    {
                        serialOutput(Command_Phrase + "|" + Command_Toggle_Valve + "|31");
                        Valve_3_Open = true;
                        statusLabel.Text = "Valve 3 opened.";
                    }
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, false);
                }
            }
            else
            {
                statusLabel.Text = "Client is not connected.";
            }
        }

        #endregion

        #region MENU STRIP

        // FILE - NEW
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool Clear_Form = true;
            if (!Saved)
            {
                DialogResult Result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButtons.YesNoCancel);
                if (Result == DialogResult.Yes)
                {
                    saveAsToolStripMenuItem_Click(null, null);
                }
                else if (Result == DialogResult.No)
                {
                    // Nothing
                }
                else
                {
                    Clear_Form = false;
                }
            }
            if (Clear_Form)
            {
                numericUpDownTriggerDelay.Value = 0;

                numericUpDownRepeatsAmount.Value = 1;
                numericUpDownRepeatsPause.Value = 5;

                checkBoxValve1Drop1.Checked = true;
                checkBoxValve1Drop2.Checked = false;
                checkBoxValve1Drop3.Checked = false;
                checkBoxValve1Drop4.Checked = false;

                checkBoxValve2Drop1.Checked = false;
                checkBoxValve2Drop2.Checked = false;
                checkBoxValve2Drop3.Checked = false;
                checkBoxValve2Drop4.Checked = false;

                checkBoxValve3Drop1.Checked = false;
                checkBoxValve3Drop2.Checked = false;
                checkBoxValve3Drop3.Checked = false;
                checkBoxValve3Drop4.Checked = false;

                numericUpDownValve1Drop1Start.Value = numericUpDownValve1Drop1Start.Minimum;
                numericUpDownValve1Drop1Durration.Value = numericUpDownValve1Drop1Durration.Minimum;
                numericUpDownValve1Drop2Start.Value = numericUpDownValve1Drop2Start.Minimum;
                numericUpDownValve1Drop2Durration.Value = numericUpDownValve1Drop2Durration.Minimum;
                numericUpDownValve1Drop3Start.Value = numericUpDownValve1Drop3Start.Minimum;
                numericUpDownValve1Drop3Durration.Value = numericUpDownValve1Drop3Durration.Minimum;
                numericUpDownValve1Drop4Start.Value = numericUpDownValve1Drop4Start.Minimum;
                numericUpDownValve1Drop4Durration.Value = numericUpDownValve1Drop4Durration.Minimum;

                numericUpDownValve2Drop1Start.Value = numericUpDownValve2Drop1Start.Minimum;
                numericUpDownValve2Drop1Durration.Value = numericUpDownValve2Drop1Durration.Minimum;
                numericUpDownValve2Drop2Start.Value = numericUpDownValve2Drop2Start.Minimum;
                numericUpDownValve2Drop2Durration.Value = numericUpDownValve2Drop2Durration.Minimum;
                numericUpDownValve2Drop3Start.Value = numericUpDownValve2Drop3Start.Minimum;
                numericUpDownValve2Drop3Durration.Value = numericUpDownValve2Drop3Durration.Minimum;
                numericUpDownValve2Drop4Start.Value = numericUpDownValve2Drop4Start.Minimum;
                numericUpDownValve2Drop4Durration.Value = numericUpDownValve2Drop4Durration.Minimum;

                numericUpDownValve3Drop1Start.Value = numericUpDownValve3Drop1Start.Minimum;
                numericUpDownValve3Drop1Durration.Value = numericUpDownValve3Drop1Durration.Minimum;
                numericUpDownValve3Drop2Start.Value = numericUpDownValve3Drop2Start.Minimum;
                numericUpDownValve3Drop2Durration.Value = numericUpDownValve3Drop2Durration.Minimum;
                numericUpDownValve3Drop3Start.Value = numericUpDownValve3Drop3Start.Minimum;
                numericUpDownValve3Drop3Durration.Value = numericUpDownValve3Drop3Durration.Minimum;
                numericUpDownValve3Drop4Start.Value = numericUpDownValve3Drop4Start.Minimum;
                numericUpDownValve3Drop4Durration.Value = numericUpDownValve3Drop4Durration.Minimum;

                Save_Form_Path = string.Empty;
            }

            statusLabel.Text = "New file.";
        }

        // FILE - OPEN
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Open data from file.";

            string Load_Buffer = string.Empty;
            // Show the openfile dialog
            OpenFileDialog Loader = new OpenFileDialog();
            Loader.Filter = "Water Drop Pro (*.wdp)|*.wdp";
            Loader.Title = "Load data from file.";
            Loader.RestoreDirectory = true;
            Loader.ShowDialog();

            try
            {
                // Load the file to buffer
                if (Loader.FileName != "")
                {
                    Save_Form_Path = Loader.FileName;

                    using (StreamReader Reader = new StreamReader(Loader.OpenFile()))
                    {
                        Load_Buffer = Reader.ReadToEnd();
                        Reader.Close();
                    }
                }

                // Get single parts
                string[] Split_Seperator = new string[] { ";" };
                string[] Split_Buffer = Load_Buffer.Split(Split_Seperator, StringSplitOptions.RemoveEmptyEntries);

                // Set single parts
                numericUpDownTriggerHeight.Value = Convert.ToInt32(Split_Buffer[0]);
                numericUpDownTriggerDelay.Value = Convert.ToInt32(Split_Buffer[1]);

                numericUpDownRepeatsAmount.Value = Convert.ToInt32(Split_Buffer[2]);
                numericUpDownRepeatsPause.Value = Convert.ToInt32(Split_Buffer[3]);

                if (Convert.ToInt32(Split_Buffer[6]) == 1) { checkBoxValve1Drop1.Checked = true; } else { checkBoxValve1Drop1.Checked = false; }
                numericUpDownValve1Drop1Start.Value = Convert.ToInt32(Split_Buffer[7]);
                numericUpDownValve1Drop1Durration.Value = Convert.ToInt32(Split_Buffer[8]);
                if (Convert.ToInt32(Split_Buffer[9]) == 1) { checkBoxValve1Drop2.Checked = true; } else { checkBoxValve1Drop2.Checked = false; }
                numericUpDownValve1Drop2Start.Value = Convert.ToInt32(Split_Buffer[10]);
                numericUpDownValve1Drop2Durration.Value = Convert.ToInt32(Split_Buffer[11]);
                if (Convert.ToInt32(Split_Buffer[12]) == 1) { checkBoxValve1Drop3.Checked = true; } else { checkBoxValve1Drop3.Checked = false; }
                numericUpDownValve1Drop3Start.Value = Convert.ToInt32(Split_Buffer[13]);
                numericUpDownValve1Drop3Durration.Value = Convert.ToInt32(Split_Buffer[14]);
                if (Convert.ToInt32(Split_Buffer[15]) == 1) { checkBoxValve1Drop4.Checked = true; } else { checkBoxValve1Drop4.Checked = false; }
                numericUpDownValve1Drop4Start.Value = Convert.ToInt32(Split_Buffer[16]);
                numericUpDownValve1Drop4Durration.Value = Convert.ToInt32(Split_Buffer[17]);

                if (Convert.ToInt32(Split_Buffer[18]) == 1) { checkBoxValve2Drop1.Checked = true; } else { checkBoxValve2Drop1.Checked = false; }
                numericUpDownValve2Drop1Start.Value = Convert.ToInt32(Split_Buffer[19]);
                numericUpDownValve2Drop1Durration.Value = Convert.ToInt32(Split_Buffer[20]);
                if (Convert.ToInt32(Split_Buffer[21]) == 1) { checkBoxValve2Drop2.Checked = true; } else { checkBoxValve2Drop2.Checked = false; }
                numericUpDownValve2Drop2Start.Value = Convert.ToInt32(Split_Buffer[22]);
                numericUpDownValve2Drop2Durration.Value = Convert.ToInt32(Split_Buffer[23]);
                if (Convert.ToInt32(Split_Buffer[24]) == 1) { checkBoxValve2Drop3.Checked = true; } else { checkBoxValve2Drop3.Checked = false; }
                numericUpDownValve2Drop3Start.Value = Convert.ToInt32(Split_Buffer[25]);
                numericUpDownValve2Drop3Durration.Value = Convert.ToInt32(Split_Buffer[26]);
                if (Convert.ToInt32(Split_Buffer[27]) == 1) { checkBoxValve2Drop4.Checked = true; } else { checkBoxValve2Drop4.Checked = false; }
                numericUpDownValve2Drop4Start.Value = Convert.ToInt32(Split_Buffer[28]);
                numericUpDownValve2Drop4Durration.Value = Convert.ToInt32(Split_Buffer[29]);

                if (Convert.ToInt32(Split_Buffer[30]) == 1) { checkBoxValve3Drop1.Checked = true; } else { checkBoxValve3Drop1.Checked = false; }
                numericUpDownValve3Drop1Start.Value = Convert.ToInt32(Split_Buffer[31]);
                numericUpDownValve3Drop1Durration.Value = Convert.ToInt32(Split_Buffer[32]);
                if (Convert.ToInt32(Split_Buffer[33]) == 1) { checkBoxValve3Drop2.Checked = true; } else { checkBoxValve3Drop2.Checked = false; }
                numericUpDownValve3Drop2Start.Value = Convert.ToInt32(Split_Buffer[34]);
                numericUpDownValve3Drop2Durration.Value = Convert.ToInt32(Split_Buffer[35]);
                if (Convert.ToInt32(Split_Buffer[36]) == 1) { checkBoxValve3Drop3.Checked = true; } else { checkBoxValve3Drop3.Checked = false; }
                numericUpDownValve3Drop3Start.Value = Convert.ToInt32(Split_Buffer[37]);
                numericUpDownValve3Drop3Durration.Value = Convert.ToInt32(Split_Buffer[38]);
                if (Convert.ToInt32(Split_Buffer[39]) == 1) { checkBoxValve3Drop4.Checked = true; } else { checkBoxValve3Drop4.Checked = false; }
                numericUpDownValve3Drop4Start.Value = Convert.ToInt32(Split_Buffer[40]);
                numericUpDownValve3Drop4Durration.Value = Convert.ToInt32(Split_Buffer[41]);

                Saved = true;
                statusLabel.Text = "Opened file.";
            }
            catch (Exception)
            {
                reportError("Unable to read file.", true);
            }
        }

        // FILE - EXPORT
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Here is still work to do 
        }

        // FILE - SAVE
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Save_Form_Path.Length == 0)
            {
                saveAsToolStripMenuItem_Click(null, null);
            }
            else
            {
                string[] Save_Data = getDataList(true);
                string Save_Buffer = numericUpDownTriggerHeight.Value.ToString() + ";";

                for (var x = 1; x < Save_Data.Length; x++)
                {
                    Save_Buffer += Save_Data[x] + ";";
                }

                try
                {
                    using (StreamWriter Writer = new StreamWriter(Save_Form_Path))
                    {
                        Writer.Write(Save_Buffer);
                        Writer.Close();
                    }
                    Saved = true;
                    statusLabel.Text = "File saved.";
                }
                catch (Exception)
                {
                    reportError("Unable to write file.", true);
                }
            }
        }

        // FILE - SAVE AS
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the data from the user interface
            string[] Save_Data = getDataList(true);
            // Show savefile dialog
            SaveFileDialog Saver = new SaveFileDialog();
            Saver.Filter = "Water Drop Pro (*.wdp)|*.wdp";
            Saver.Title = "Save data to file";
            Saver.RestoreDirectory = true;
            Saver.ShowDialog();
            // Store the data into a buffer string
            string Save_Buffer = numericUpDownTriggerHeight.Value.ToString() + ";";

            for (var x = 1; x < Save_Data.Length; x++)
            {
                Save_Buffer += Save_Data[x] + ";";
            }

            if (Saver.FileName != "")
            {
                Save_Form_Path = Saver.FileName;

                try
                {
                    using (StreamWriter Writer = new StreamWriter(Saver.OpenFile()))
                    {
                        Writer.Write(Save_Buffer);
                        Writer.Close();
                    }
                    Saved = true;
                    statusLabel.Text = "File saved.";
                }
                catch (Exception)
                {
                    reportError("Unable to write file.", true);
                }
            }
        }

        // FILE - CLOSE
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // FILE - DROPDOWN OPENED
        private void fILEToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            fILEToolStripMenuItem.ForeColor = Color.Black;
        }

        // FILE - DROPDOWN CLOSED
        private void fILEToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            fILEToolStripMenuItem.ForeColor = Color.White;
        }

        // VIEW - DESIGN - DARK
        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(50, 50, 50);
            this.ForeColor = Color.White;
            groupBoxArduino.ForeColor = Color.White;
            groupBoxTrigger.ForeColor = Color.White;
            groupBoxRepeats.ForeColor = Color.White;
            groupBoxCameraSettingsOffline.ForeColor = Color.White;
            groupBoxValve1.ForeColor = Color.White;
            groupBoxValve2.ForeColor = Color.White;
            groupBoxValve3.ForeColor = Color.White;
            groupBoxCamera.ForeColor = Color.White;
            groupBoxCameraSettingsOnline.ForeColor = Color.White;
            groupBoxActions.ForeColor = Color.White;
            groupBoxLiveView.ForeColor = Color.White;
            Design_Sheme = 1;
        }

        // VIEW - DESIGN - BRIGHT
        private void brightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            groupBoxArduino.ForeColor = Color.Black;
            groupBoxTrigger.ForeColor = Color.Black;
            groupBoxRepeats.ForeColor = Color.Black;
            groupBoxCameraSettingsOffline.ForeColor = Color.Black;
            groupBoxValve1.ForeColor = Color.Black;
            groupBoxValve2.ForeColor = Color.Black;
            groupBoxValve3.ForeColor = Color.Black;
            groupBoxCamera.ForeColor = Color.Black;
            groupBoxCameraSettingsOnline.ForeColor = Color.Black;
            groupBoxActions.ForeColor = Color.Black;
            groupBoxLiveView.ForeColor = Color.Black;
            Design_Sheme = 2;
        }

        // VIEW - FULLSCREEN
        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Maximized)
            {
                Maximized = false;
                this.MaximumSize = new Size(Maximum_Size[0], Maximum_Size[1]);
                this.Size = new Size(Maximum_Size[0], Maximum_Size[1]);
                FormBorderStyle = FormBorderStyle.FixedSingle;
                WindowState = FormWindowState.Normal;

                int pb_w = pictureBoxLiveView.Size.Width;
                int pb_h = pictureBoxLiveView.Height;
            }
            else
            {
                Maximized = true;
                this.MaximumSize = new Size(0, 0);
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
        }

        // VIEW - ERROR LIST
        private void errorListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form ErrorList = new Error_List_Form();
            ErrorList.Show();
        }

        // VIEW - HIDE/SHOW CAMERA LIST
        private void hideShowCameraListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Auto_Hide_Camera)
            {
                if (Camera_List_Visible)
                {
                    this.MinimumSize = new System.Drawing.Size(Minimum_Size[0], Minimum_Size[1]);
                    this.Size = new System.Drawing.Size(Minimum_Size[0], Minimum_Size[1]);
                    this.MaximumSize = new System.Drawing.Size(Minimum_Size[0], Minimum_Size[1]);
                }
                else
                {
                    this.MaximumSize = new System.Drawing.Size(Maximum_Size[0], Maximum_Size[1]);
                    this.Size = new System.Drawing.Size(Maximum_Size[0], Maximum_Size[1]);
                    this.MinimumSize = new System.Drawing.Size(Maximum_Size[0], Maximum_Size[1]);
                }
            }
        }

        // VIEW - DROPDOWN OPENED
        private void vIEWToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            vIEWToolStripMenuItem.ForeColor = Color.Black;
        }

        // VIEW - DROPDOWN CLOSED
        private void vIEWToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            vIEWToolStripMenuItem.ForeColor = Color.White;
        }

        // TOOLS - OPTIONS
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Options = new Options_Form();
            Options.Show();
        }

        // TOOLS - DROPDOWN OPENED
        private void tOOLSToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            tOOLSToolStripMenuItem.ForeColor = Color.Black;
        }

        // TOOLS - DROPDOWN CLOSED
        private void tOOLSToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            tOOLSToolStripMenuItem.ForeColor = Color.White;
        }

        #endregion

        #region SUBROUTINES

        // GENERATE DATA LIST
        public string[] getDataList(bool Paramter_Get_All_Data)
        {
            string[] Data_List = new string[42];

            // INDEX 0: Data header
            Data_List[0] = Data_Phrase;
            // INDEX 1: Delay
            Data_List[1] = numericUpDownTriggerDelay.Value.ToString();
            // INDEX 2: REPEATS, AMOUNT
            Data_List[2] = numericUpDownRepeatsAmount.Value.ToString();
            // INDEX 3: REPEATS, PAUSE
            Data_List[3] = numericUpDownRepeatsPause.Value.ToString();
            // INDEX 4: SETTINGS, AUTOFOCUS
            if (checkBoxCameraSettingsOfflineAutofocus.Checked)
                Data_List[4] = "1";
            else
                Data_List[4] = "0";
            // INDEX 5: MIRROR LOCKUP
            if (checkBoxCameraSettingsOfflineMirrorLockup.Checked)
                Data_List[5] = "1";
            else
                Data_List[5] = "0";
            // INDEX 6: VALVE 1, DROP 1
            if (checkBoxValve1Drop1.Checked)
                Data_List[6] = "1";
            else
                Data_List[6] = "0";
            // INDEX 7: Valve 1, Drop 1, START
            Data_List[7] = numericUpDownValve1Drop1Start.Value.ToString();
            // INDEX 8: Valve 1, Drop 1, DURRATION
            Data_List[8] = numericUpDownValve1Drop1Durration.Value.ToString();
            // INDEX 9: VALVE 1, DROP 2
            if (checkBoxValve1Drop2.Checked)
                Data_List[9] = "1";
            else
                Data_List[9] = "0";
            // INDEX 10: Valve 1, Drop 2, START
            Data_List[10] = numericUpDownValve1Drop2Start.Value.ToString();
            // INDEX 11: Valve 1, Drop 2, DURRATION
            Data_List[11] = numericUpDownValve1Drop2Durration.Value.ToString();
            // INDEX 12: VALVE 1, DROP 3
            if (checkBoxValve1Drop3.Checked)
                Data_List[12] = "1";
            else
                Data_List[12] = "0";
            // INDEX 13: Valve 1, Drop 3, START
            Data_List[13] = numericUpDownValve1Drop3Start.Value.ToString();
            // INDEX 14: Valve 1, Drop 3, DURRATION
            Data_List[14] = numericUpDownValve1Drop3Durration.Value.ToString();
            // INDEX 15: VALVE 1, DROP 4
            if (checkBoxValve1Drop4.Checked)
                Data_List[15] = "1";
            else
                Data_List[15] = "0";
            // INDEX 16: Valve 1, Drop 4, START
            Data_List[16] = numericUpDownValve1Drop4Start.Value.ToString();
            // INDEX 17: Valve 1, Drop 4, DURRATION
            Data_List[17] = numericUpDownValve1Drop4Durration.Value.ToString();
            // INDEX 18: VALVE 2, DROP 1
            if (checkBoxValve2Drop1.Checked)
                Data_List[18] = "1";
            else
                Data_List[18] = "0";
            // INDEX 19: Valve 2, Drop 1, START
            Data_List[19] = numericUpDownValve2Drop1Start.Value.ToString();
            // INDEX 20: Valve 2, Drop 1, DURRATION
            Data_List[20] = numericUpDownValve2Drop1Durration.Value.ToString();
            // INDEX 21: VALVE 2, DROP 2
            if (checkBoxValve2Drop2.Checked)
                Data_List[21] = "1";
            else
                Data_List[21] = "0";
            // INDEX 22: Valve 2, Drop 2, START
            Data_List[22] = numericUpDownValve2Drop2Start.Value.ToString();
            // INDEX 23: Valve 2, Drop 2, DURRATION
            Data_List[23] = numericUpDownValve2Drop2Durration.Value.ToString();
            // INDEX 24: VALVE 1, DROP 3
            if (checkBoxValve2Drop3.Checked)
                Data_List[24] = "1";
            else
                Data_List[24] = "0";
            // INDEX 25: Valve 2, Drop 3, START
            Data_List[25] = numericUpDownValve2Drop3Start.Value.ToString();
            // INDEX 26: Valve 2, Drop 3, DURRATION
            Data_List[26] = numericUpDownValve2Drop3Durration.Value.ToString();
            // INDEX 27: VALVE 2, DROP 4
            if (checkBoxValve2Drop4.Checked)
                Data_List[27] = "1";
            else
                Data_List[27] = "0";
            // INDEX 28: Valve 2, Drop 4, START
            Data_List[28] = numericUpDownValve2Drop4Start.Value.ToString();
            // INDEX 29: Valve 1, Drop 4, DURRATION
            Data_List[29] = numericUpDownValve2Drop4Durration.Value.ToString();
            // INDEX 30: VALVE 3, DROP 1
            if (checkBoxValve3Drop1.Checked)
                Data_List[30] = "1";
            else
                Data_List[30] = "0";
            // INDEX 31: Valve 3, Drop 1, START
            Data_List[31] = numericUpDownValve3Drop1Start.Value.ToString();
            // INDEX 32: Valve 3, Drop 1, DURRATION
            Data_List[32] = numericUpDownValve3Drop1Durration.Value.ToString();
            // INDEX 33: VALVE 3, DROP 2
            if (checkBoxValve3Drop2.Checked)
                Data_List[33] = "1";
            else
                Data_List[33] = "0";
            // INDEX 34: Valve 3, Drop 2, START
            Data_List[34] = numericUpDownValve3Drop2Start.Value.ToString();
            // INDEX 35: Valve 3, Drop 2, DURRATION
            Data_List[35] = numericUpDownValve3Drop2Durration.Value.ToString();
            // INDEX 36: VALVE 3, DROP 3
            if (checkBoxValve3Drop3.Checked)
                Data_List[36] = "1";
            else
                Data_List[36] = "0";
            // INDEX 37: Valve 3, Drop 3, START
            Data_List[37] = numericUpDownValve3Drop3Start.Value.ToString();
            // INDEX 38: Valve 3, Drop 3, DURRATION
            Data_List[38] = numericUpDownValve3Drop3Durration.Value.ToString();
            // INDEX 39: VALVE 3, DROP 4
            if (checkBoxValve3Drop4.Checked)
                Data_List[39] = "1";
            else
                Data_List[39] = "0";
            // INDEX 40: Valve 3, Drop 4, START
            Data_List[40] = numericUpDownValve3Drop4Start.Value.ToString();
            // INDEX 41: Valve 3, Drop 4, DURRATION
            Data_List[41] = numericUpDownValve3Drop4Durration.Value.ToString();

            return Data_List;
        }

        // CONNECT CAMERA
        private void connectCamera()
        {
            if (listBoxCamera.SelectedIndex >= 0)
            {
                try
                {
                    CameraHandler.OpenSession(Camera_List[listBoxCamera.SelectedIndex]);
                    buttonConnectCamera.Text = "DISCONNECT";

                    string cameraname = CameraHandler.MainCamera.Info.szDeviceDescription;
                    //SessionLabel.Text = cameraname;
                    if (CameraHandler.GetSetting(EDSDK.PropID_AEMode) != EDSDK.AEMode_Manual)
                        MessageBox.Show("Camera is not in manual mode. Some features might not work!",
                                        "Camera",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                    AvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Av);
                    TvList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_Tv);
                    ISOList = CameraHandler.GetSettingsList((uint)EDSDK.PropID_ISOSpeed);

                    foreach (int Av in AvList)
                        comboBoxAv.Items.Add(CameraValues.AV((uint)Av));
                    foreach (int Tv in TvList)
                        comboBoxTv.Items.Add(CameraValues.TV((uint)Tv));
                    foreach (int ISO in ISOList)
                        comboBoxISO.Items.Add(CameraValues.ISO((uint)ISO));

                    comboBoxAv.SelectedIndex = comboBoxAv.Items.IndexOf(CameraValues.AV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Av)));
                    comboBoxTv.SelectedIndex = comboBoxTv.Items.IndexOf(CameraValues.TV((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_Tv)));
                    comboBoxISO.SelectedIndex = comboBoxISO.Items.IndexOf(CameraValues.ISO((uint)CameraHandler.GetSetting((uint)EDSDK.PropID_ISOSpeed)));

                    Connected_Camera = true;
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, true);
                }
            }
        }

        // CLOSE CONNECTION (CAMERA)
        private void disconnectCamera()
        {
            CameraHandler.CloseSession();
            comboBoxAv.Items.Clear();
            comboBoxTv.Items.Clear();
            comboBoxISO.Items.Clear();
            buttonConnectCamera.Text = "CONNECT";
            //Closing the session invalidates the current camera pointer
            refreshCamera();

            Connected_Camera = false;
        }

        // REFRESH CAMERA LIST
        private void refreshCamera()
        {
            listBoxCamera.Items.Clear();
            Camera_List = CameraHandler.GetCameraList();
            foreach (Camera cam in Camera_List)
                listBoxCamera.Items.Add(cam.Info.szDeviceDescription);
            if (CameraHandler.CameraSessionOpen)
                listBoxCamera.SelectedIndex = Camera_List.FindIndex(t => t.Ref == CameraHandler.MainCamera.Ref);
            else if (Camera_List.Count > 0)
            {
                listBoxCamera.SelectedIndex = 0;
                // Set form
                if (Auto_Hide_Camera)
                {
                    this.MaximumSize = new Size(Maximum_Size[0], Maximum_Size[1]);
                    this.Size = new Size(Maximum_Size[0], Maximum_Size[1]);
                }
            }
            else if (Camera_List.Count == 0)
            {
                if (Maximized)
                {
                    Maximized = false;
                    FormBorderStyle = FormBorderStyle.FixedSingle;
                    WindowState = FormWindowState.Normal;
                }
                // Set buttons
                fullscreenToolStripMenuItem.Visible = false;
                // Set form
                if (Auto_Hide_Camera)
                {
                    this.MaximumSize = new Size(Minimum_Size[0], Minimum_Size[1]);
                    this.Size = new Size(Minimum_Size[0], Minimum_Size[1]);
                }
                // Disable live view
                groupBoxLiveView.Visible = false;
            }
        }

        // SET MINIMUM VALVE TIME
        public void setMinimumValveTime()
        {
            numericUpDownValve1Drop1Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve1Drop2Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve1Drop3Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve1Drop4Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve2Drop1Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve2Drop2Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve2Drop3Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve2Drop4Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve3Drop1Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve3Drop2Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve3Drop3Durration.Minimum = Minimum_Valve_Time;
            numericUpDownValve3Drop4Durration.Minimum = Minimum_Valve_Time;
        }

        // ERROR REPORTER
        private void reportError(string Parameter_Error, bool Parameter_Show_Message)
        {
            string Error_Log_Buffer = string.Empty;
            // Get date
            Error_Log_Buffer += DateTime.Now.ToString("dd.MM.yyyy");
            Error_Log_Buffer += " ";
            // Get time
            Error_Log_Buffer += DateTime.Now.ToString("HH.mm.ss");
            Error_Log_Buffer += " ";
            // Get error
            Error_Log_Buffer += Parameter_Error;
            // Add it to log list
            Error_Log.Add(Error_Log_Buffer);
            // Show message in box
            if (Parameter_Show_Message) 
                MessageBox.Show(Parameter_Error, "Error Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Show status for user
            statusLabel.Text = "An error occured: " + Parameter_Error;
        }

        #endregion

        #region SDK EVENTS

        //AV CHANGED
        private void comboBoxAv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CameraHandler.SetSetting(EDSDK.PropID_Av, CameraValues.AV((string)comboBoxAv.SelectedItem));
            }
            catch (Exception ex)
            {
                reportError(ex.Message, false);
            }
        }

        // TV CHANGED
        private void comboBoxTv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CameraHandler.SetSetting(EDSDK.PropID_Tv, CameraValues.TV((string)comboBoxTv.SelectedItem));
                if ((string)comboBoxTv.SelectedItem == "Bulb")
                    numericUpDownBulb.Enabled = true;
                else 
                    numericUpDownBulb.Enabled = false;
            }
            catch (Exception ex)
            {
                reportError(ex.Message, false);
            }
        }

        // ISO CHANGED
        private void comboBoxISO_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CameraHandler.SetSetting(EDSDK.PropID_ISOSpeed, CameraValues.ISO((string)comboBoxISO.SelectedItem));
            }
            catch (Exception ex)
            {
                reportError(ex.Message, false);
            }
        }

        // BULB TIME CHANGED
        private void comboBoxBulb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // LIVE VIEW
        private void SDK_LiveViewUpdated(Stream img)
        {
            try
            {
                Evf_Bmp = new Bitmap(img);
                using (Graphics Camera_Picture = pictureBoxLiveView.CreateGraphics())
                {
                    LVBratio = LVBw / (float)LVBh;
                    LVration = Evf_Bmp.Width / (float)Evf_Bmp.Height;
                    if (LVBratio < LVration)
                    {
                        w = LVBw;
                        h = (int)(LVBw / LVration);
                    }
                    else
                    {
                        w = (int)(LVBh * LVration);
                        h = LVBh;
                    }
                    Camera_Picture.DrawImage(Evf_Bmp, 0, 0, w, h);
                }
                Evf_Bmp.Dispose();
            }
            catch (Exception ex)
            {
                reportError(ex.Message, true);
            }
        }

        // LIVE VIEW CHANGE SIZE
        private void pictureBoxLiveView_SizeChanged(object sender, EventArgs e)
        {
            // Set width & height for live view
            try
            {
                LVBw = pictureBoxLiveView.Width;
                LVBh = pictureBoxLiveView.Height;
            }
            catch (Exception ex)
            {
                reportError(ex.Message, true);
            }
        }

        // CAMERA ADDED
        private void SDK_CameraAdded()
        {
            try
            {
                refreshCamera();
            }
            catch (Exception ex)
            {
                reportError(ex.Message, false);
            }
        }

        #endregion

        #region TIMER

        // RUNTIME
        private void timerRuntime_Tick(object sender, EventArgs e)
        {
            if (groupBoxLiveView.Visible == true)
            {
                buttonStartProcess.Visible = false;
                hideShowCameraListToolStripMenuItem.Visible = false;
                toolStripSeparator5.Visible = false;
            }
            else
            {
                buttonStartProcess.Visible = true;
                if (Auto_Hide_Camera || Connected_Camera)
                {
                    hideShowCameraListToolStripMenuItem.Visible = false;
                    toolStripSeparator5.Visible = false;
                }
                else
                {
                    hideShowCameraListToolStripMenuItem.Visible = true;
                    toolStripSeparator5.Visible = true;
                }
            }

            if (this.Size.Width > Minimum_Size[0])
            {
                groupBoxActions.Visible = true;
                groupBoxCamera.Visible = true;
                groupBoxCameraSettingsOnline.Visible = true;
                Camera_List_Visible = true;
            }
            else
            {
                groupBoxActions.Visible = false;
                groupBoxCamera.Visible = false;
                groupBoxCameraSettingsOnline.Visible = false;
                Camera_List_Visible = false;
            }

            if (Valve_1_Open || Valve_2_Open || Valve_3_Open)
            {
                buttonStartProcess.Enabled = false;
            }
            else if (!Working)
            {
                buttonStartProcess.Enabled = true;
            }

            setMinimumValveTime();
        }

        // CONNECTION TIMEOUT
        private void timerConnectionTimeout_Tick(object sender, EventArgs e)
        {
            timerConnectionTimeout.Stop();
            serialPort.Close();
            // Update button and combo box
            buttonConnectArduino.Enabled = true;
            listBoxArduino.Enabled = true;
            // Error report
            reportError("Timeout by attenmpting to connect to " + serialPort.PortName + ".", true);
        }

        // GET PORTS
        private void timerGetPorts_Tick(object sender, EventArgs e)
        {
            // Refresh port list only if new devices are added
            if (Port_Amount != SerialPort.GetPortNames().Length)
            {
                var Ports = SerialPort.GetPortNames();
                listBoxArduino.DataSource = Ports;
                Port_Amount = SerialPort.GetPortNames().Length;
            }
        }

        // GET CAMERA
        private void timerGetCamera_Tick(object sender, EventArgs e)
        {
            try
            {
                refreshCamera();
            }
            catch (Exception ex)
            {
                reportError(ex.Message, false);
            }
        }

        #endregion

        #region NUMERIC UP DOWN WATCHER

        // TRIGGER HEIGHT
        private void numericUpDownTriggerHeight_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
            
            // Calulate the new best delay time
            float Math_Gravity_Force = 9.80665f;
            float Height_Under_Valve = (float)numericUpDownTriggerHeight.Value;
            float Height_Above_Valve = 0.0f;

            // Convert the heights into meter
            Height_Under_Valve /= 1000;
            Height_Above_Valve /= 1000;

            // Calculate the time from the valve to the surface in seconds
            float Delay_Time = (float)((2 * Height_Under_Valve) / (Math.Sqrt(2 * Math_Gravity_Force * Height_Above_Valve) + Math.Sqrt(2 * Math_Gravity_Force * (Height_Above_Valve + Height_Under_Valve))));

            // Convert the time into milliseconds
            Delay_Time *= 1000;

            // Set the new delay time
            numericUpDownTriggerDelay.Value = Convert.ToInt32(Delay_Time);
        }

        // TRIGGER DELAY
        private void numericUpDownTriggerDelay_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 1 START
        private void numericUpDownValve1Drop1Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 1 DURRATION
        private void numericUpDownValve1Drop1Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 2 START
        private void numericUpDownValve1Drop2Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 2 DURRATION
        private void numericUpDownValve1Drop2Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 3 START
        private void numericUpDownValve1Drop3Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 3 DURRATION
        private void numericUpDownValve1Drop3Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 4 START
        private void numericUpDownValve1Drop4Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 4 DURRATION
        private void numericUpDownValve1Drop4Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 1 START
        private void numericUpDownValve2Drop1Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 1 DURRATION
        private void numericUpDownValve2Drop1Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 2 START
        private void numericUpDownValve2Drop2Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 2 DURRATION
        private void numericUpDownValve2Drop2Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 3 START
        private void numericUpDownValve2Drop3Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 3 DURRATION
        private void numericUpDownValve2Drop3Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 4 START
        private void numericUpDownValve2Drop4Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 4 DURRATION
        private void numericUpDownValve2Drop4Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 1 START
        private void numericUpDownValve3Drop1Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 1 DURRATION
        private void numericUpDownValve3Drop1Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 2 START
        private void numericUpDownValve3Drop2Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 2 DURRATION
        private void numericUpDownValve3Drop2Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 3 START
        private void numericUpDownValve3Drop3Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 3 DURRATION
        private void numericUpDownValve3Drop3Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 4 START
        private void numericUpDownValve3Drop4Start_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 4 DURRATION
        private void numericUpDownValve3Drop4Durration_ValueChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        #endregion

        #region CHECKBOX WATCHER

        // SELECT SAVE PATH FOR PICTURES
        private void checkBoxSaveToHost_CheckedChanged(object sender, EventArgs e)
        {
            if (!Startup && Connected_Camera)
            {
                try
                {
                    if (checkBoxSaveToHost.Checked)
                    {
                        FolderBrowserDialog FBD = new FolderBrowserDialog();
                        FBD.Description = "Save pictures to";
                        FBD.SelectedPath = @"%userprofile%\pictures\";
                        DialogResult objResult = FBD.ShowDialog(this);
                        if (objResult == DialogResult.OK)
                        {
                            SaveToHost_Path = FBD.SelectedPath;
                            CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Both);
                            CameraHandler.SetCapacity();
                            CameraHandler.ImageSaveDirectory = SaveToHost_Path;
                        }
                        else
                        {
                            checkBoxSaveToHost.Checked = false;
                            CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Camera);
                        }
                    }
                    else
                    {
                        CameraHandler.SetSetting(EDSDK.PropID_SaveTo, (uint)EDSDK.EdsSaveTo.Camera);
                    }
                }
                catch (Exception ex)
                {
                    reportError(ex.Message, true);
                }
            }
        }

        // SHOW CURRENT PATH FOR PICTURES
        private void checkBoxSaveToHost_MouseHover(object sender, EventArgs e)
        {
            statusLabel.Text = "Current path: " + SaveToHost_Path;
        }

        // VALVE 1 DROP 1
        private void checkBoxValve1Drop1_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 2
        private void checkBoxValve1Drop2_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 3
        private void checkBoxValve1Drop3_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 1 DROP 4
        private void checkBoxValve1Drop4_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 1
        private void checkBoxValve2Drop1_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 2
        private void checkBoxValve2Drop2_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 3
        private void checkBoxValve2Drop3_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 2 DROP 4
        private void checkBoxValve2Drop4_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 1
        private void checkBoxValve3Drop1_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 2
        private void checkBoxValve3Drop2_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 3
        private void checkBoxValve3Drop3_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        // VALVE 3 DROP 4
        private void checkBoxValve3Drop4_CheckedChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        #endregion
    }
}
