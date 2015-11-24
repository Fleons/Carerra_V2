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
        
        SerialPort _myport = new SerialPort();
        int counter = 1;            //port geschlossen wenn ungerade
        int[,] werte = new int[120,120]; //array zum einlesen der werte für einen plot 
        double zaehler = 0;
        double previousValue = 0;
        double currentValue = 0;
        int runde = 0;
        
        //---------------------------------------------------------------------------------------------


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

    /*   void DataReceiveHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            Byte[] data = new Byte[5];
            int datalength = 5;
            sp.Read(data,0,datalength);
            textBox1.Text = Convert.ToString(data[4]);
        }
        */

        //------------------------------------------------------------------------------------------------------------------



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                      
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }


        //--------------------------------------------------------------------------------------------------------------

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
            timer.Start();
            zaehler = 0;
            
            
               
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            zaehler++;

            _myport.DiscardInBuffer();

            string Data = _myport.ReadLine();





            //    if (Data != "" || Data != "\r" )
            //   {
            try { 
                currentValue = Convert.ToDouble(Data);
                
                richTextBox1.AppendText(Data);
                chart_kraftverlauf.Series["V1"].Points.AddXY(zaehler/2, currentValue);

                zaehler++;
            }    
            catch
            {
                zaehler++;
            }        
            
            if (zaehler > 120)
            {
                timer.Stop();
            }
        }
    }
}
