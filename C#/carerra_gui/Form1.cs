using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Timers;

namespace carerra_gui
{
    public partial class Form1 : Form
    {
        #region DECLARATION
        SerialPort _myport = new SerialPort();

        int counter = 1;       //counter: port geschlossen wenn ungerade
        int[,] werte = new int[120,120]; //array zum einlesen der werte für einen plot 
        double zaehler = 0;
        string Cal_Phrase = "CAL";           
        String SFactor_Phrase = "SFACTOR";
        String Control_Phrase = "CONTROL";
        String VMax_Phrase = "VMAX";
        String Start_Phrase = "START";
        String End_Phrase = "END";
        String StringTemp = "";

        //Serial
        String Data_Phrase = "DATA";
       

        #endregion
       

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;

        }

        void Form1_Load(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames();
            comboBox1.DataSource = ports;           
        }


        #region CONNECT/DISCONNECT
        void Connect(string portName)
        {
            _myport = new SerialPort(portName);
                if (!_myport.IsOpen)
                {
                    _myport.BaudRate = 9600;
                    _myport.Parity = Parity.None;
                    _myport.DataBits = 8;
                    _myport.Handshake = Handshake.None;
                    _myport.RtsEnable = true;
                    _myport.Open();
                    _myport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                if (_myport.IsOpen == true)
                {
                    MessageBox.Show("Port ist offen");
                }

            }  
        }
        
        void disconnect()
        {
            timer.Stop();
            if (_myport.IsOpen)
            {
                _myport.Close();
                
            }
            
            if (_myport.IsOpen == false)
            {
                MessageBox.Show("Port ist geschlossen");
            }

        }
        #endregion


        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string Received = string.Empty;

            try
            {
                Received = _myport.ReadTo(";");
                // Clear the buffer
                _myport.DiscardInBuffer();
                _myport.DiscardOutBuffer();
            }
            catch (Exception)
            {
                Received = string.Empty;
              

            }

            if (Received.StartsWith(Data_Phrase))
            {
                string[] Split_Seperator = new string[] { "|" };
                string[] Split_Buffer = Received.Split(Split_Seperator, StringSplitOptions.None);
                if (Split_Buffer[0] == "DATA")
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        richTextBox1.AppendText(Split_Buffer[1]);
                        chart_kraftverlauf.Series["V1"].Points.AddXY(zaehler / 2, Split_Buffer[1]);

                    });
                    
                }
            }   

        }

        private void serialOutput(string Parameter_Data)
        {
            try
            {
                _myport.Write(Parameter_Data + ";");
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
        }


       



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                      
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
        

        private void button_connect_Click(object sender, EventArgs e)
        {
            
            if (counter %2 != 0)        //Port öffnen wenn ungerade
            {
                Connect(comboBox1.SelectedItem.ToString());
                button_connect.Text = "Disconnect";
                counter++;
            }
            else
            {
                disconnect();
                button_connect.Text = "Connect";
                counter++;
                
            }
        }

        private void button_datenaufnahme_Click(object sender, EventArgs e)
        {         
            if (radioButton_kind.Checked)
            {
                serialOutput(SFactor_Phrase + "|3");
            }
            else if (radioButton_frau.Checked)
            {
                serialOutput(SFactor_Phrase + "|2");
            }
            else if (radioButton_mann.Checked)
            {
                serialOutput(SFactor_Phrase + "|1");
            }







            timer.Start();
            zaehler = 0;

        }

       private void timer_Tick(object sender, EventArgs e)
        {
            zaehler++;

            serialOutput("DATA");
            
            if (zaehler > 120)
            {
                timer.Stop();
            }
        }

        private void hScrollBar_Nullpunkt_Scroll(object sender, ScrollEventArgs e)
        {
            serialOutput(Cal_Phrase + "|" + hScrollBar_Nullpunkt.Value);
        }

        private void button_Bahn_aktiviert_Click(object sender, EventArgs e)
        {
            if(button_Bahn_aktiviert.Text =="Bahn aktiv")
            {
                button_Bahn_aktiviert.Text = "Bahn INKATIV";
                serialOutput(Control_Phrase + "|" + End_Phrase);
            }
            else if (button_Bahn_aktiviert.Text == "Bahn INAKTIV")
            {
                button_Bahn_aktiviert.Text = "Bahn aktiv";
                serialOutput(Control_Phrase + "|" + Start_Phrase);
            }
        }

        private void numericUpDown_VMAX_ValueChanged(object sender, EventArgs e)
        {
            serialOutput(VMax_Phrase + "|" + numericUpDown_VMAX.Value);
        }
    }
}
