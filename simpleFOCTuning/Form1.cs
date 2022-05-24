using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simpleFOCTuning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        serialConfig sc = serialConfig.Instance;
        tuneProperty tp = new tuneProperty();
        SerialPort port = new SerialPort();

        public delegate void tpDelegate(tuneProperty tp);
        public event tpDelegate tpChangeEvent;


        private void Form1_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = tp;
            propertyGrid1.ToolbarVisible = false;
            propertyGrid1.HelpVisible = false;
            propertyGrid1.PropertySort = PropertySort.Categorized;

            port.ReadBufferSize = 50;
            port.ReadTimeout = 600;
            port.DataReceived += PortDataReceive;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ConfigureSerialConnect f1 = new ConfigureSerialConnect();
            f1.ShowDialog(this);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (!port.IsOpen)
                {
                    GC.Collect();
                    port.PortName = sc.myPortName;
                    port.BaudRate = sc.myBaud;
                    port.StopBits = (sc.myStopbits == "1") ? StopBits.One : StopBits.Two;
                    if (sc.myParity == "None") port.Parity = Parity.None;
                    else if (sc.myParity == "Odd") port.Parity = Parity.Odd;
                    else if (sc.myParity == "Even") port.Parity = Parity.Even;
                    else if (sc.myParity == "Space") port.Parity = Parity.Space;
                    port.DataBits = sc.myBytebits;
                    port.Open();
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                    button8.Text = "Disconnect";
                }
                else if (port.IsOpen)
                {
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                    port.Close();
                    GC.Collect();
                    button8.Text = "Connect";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Enable Device")
            {
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "E1" + "\n"));
                button1.Text = "Disable Device";
            }
            else if (button1.Text == "Disable Device")
            {
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "E0" + "\n"));
                button1.Text = "Enable Device";
            }
        }

        private void SendData(byte[] sendBuffer)
        {
            if (sendBuffer != null)
            {
                try
                {
                    port.Write(sendBuffer, 0, sendBuffer.Length);
                }
                catch
                {

                }
            }
        }
        private void PortDataReceive(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] buffer = new byte[port.BytesToRead];
                Int32 lengh = port.Read(buffer, 0, buffer.Length);
                string str = Encoding.ASCII.GetString(buffer);
                string[] strArray = str.Split(':');
                switch (strArray[0])
                {
                    case "Motion":
                        switch (strArray[1])
                        {
                            case "angle\r\n":
                                //tp.MCT = (tuneProperty.MotionControlType)1;
                                tp.MCT = tuneProperty.MotionControlType.Angle;
                                propertyGrid1.Invoke(new EventHandler(delegate
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                            case "velocity\r\n":
                                tp.MCT = tuneProperty.MotionControlType.Velocity;
                                propertyGrid1.Invoke(new Action(() =>
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                            case "torque\r\n":
                                tp.MCT = tuneProperty.MotionControlType.Torque;
                                break;
                            case "VelocityOpenloop\r\n":
                                tp.MCT = tuneProperty.MotionControlType.VelocityOpenloop;
                                break;
                            case "AngleOpenloop\r\n":
                                tp.MCT = tuneProperty.MotionControlType.AngleOpenloop;
                                break;
                            //case " downsample\r\n":
                            //    tp.MotionDownsample = Int32.Parse(strArray[2]);
                            //    break;
                        }
                        break;
                    case "Torque":
                        switch (strArray[1])
                        {
                            case "angle\r\n":
                                //tp.MCT = (tuneProperty.MotionControlType)1;
                                tp.MCT = tuneProperty.MotionControlType.Angle;
                                propertyGrid1.Invoke(new EventHandler(delegate
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                        }
                        break;
                    case "PID vel| P":
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int dt=5;
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "C" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "T" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VP" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VI" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VD" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VR" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VL" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VF" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AP" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AI" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AD" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AR" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AL" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AF" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DP" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DI" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DD" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DR" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DL" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DF" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QP" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QI" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QD" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QR" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QL" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QF" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "LV" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "LU" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "LC" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "SM" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "SE" + "\n"));
            Thread.Sleep(dt);
            //SendData(Encoding.ASCII.GetBytes(textBox6.Text + "CD" + "\n"));
            //Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "R" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "WC" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "E" + "\n"));
            Thread.Sleep(dt);
            SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AAAAAATEST6" + "\n"));
        }
    }

}
