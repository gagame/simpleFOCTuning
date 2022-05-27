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

        private void Form1_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = tp;
            propertyGrid1.ToolbarVisible = false;
            propertyGrid1.HelpVisible = false;
            propertyGrid1.PropertySort = PropertySort.Categorized;

            port.ReadBufferSize = 50;
            port.ReadTimeout = 600;
            port.DataReceived += PortDataReceive;

            propertyGrid1.PropertyValueChanged += PropertyChange;
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
        private void PropertyChange(object sender, PropertyValueChangedEventArgs e)
        {
            try
            {
                Console.WriteLine(e.ChangedItem.PropertyDescriptor.Name);
                //Console.WriteLine(e.ChangedItem.Parent.Label);
                Console.WriteLine(e.ChangedItem.Value);
                int dt = 5;
                switch (e.ChangedItem.PropertyDescriptor.Name)
                {
                    case "MCT":
                        switch (e.ChangedItem.Value)
                        {
                            case tuneProperty.MotionControlType.Torque:
                                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "C0" + "\n"));
                                Thread.Sleep(dt);
                                break;
                            case tuneProperty.MotionControlType.Velocity:
                                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "C1" + "\n"));
                                Thread.Sleep(dt);
                                break;
                            case tuneProperty.MotionControlType.Angle:
                                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "C2" + "\n"));
                                Thread.Sleep(dt);
                                break;
                            case tuneProperty.MotionControlType.VelocityOpenloop:
                                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "C3" + "\n"));
                                Thread.Sleep(dt);
                                break;
                            case tuneProperty.MotionControlType.AngleOpenloop:
                                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "C4" + "\n"));
                                Thread.Sleep(dt);
                                break;
                        }
                        break;
                    case "TCT":
                        switch (e.ChangedItem.Value)
                        {
                            case tuneProperty.TorqueControlType.Voltage:
                                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "T0" + "\n"));
                                Thread.Sleep(dt);
                                break;
                            case tuneProperty.TorqueControlType.DCCurrent:
                                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "T1" + "\n"));
                                Thread.Sleep(dt);
                                break;
                            case tuneProperty.TorqueControlType.FOCCurrent:
                                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "T2" + "\n"));
                                Thread.Sleep(dt);
                                break;
                        }
                        break;
                    case "VelocityProportionalGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VP" + e.ChangedItem.Value.ToString()+ "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "VelocityIntegralGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "VelocityDerivativeGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "VelocityOutputRamp":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "VelocityOutputLitmit":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "VelocityLowPassFilter":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "AngleProportionalGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "AngleIntegralGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "AngleDerivativeGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "AngleOutputRamp":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "AngleOutputLitmit":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "AngleLowPassFilter":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentqProportionalGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentqIntegralGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentqDerivativeGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentqOutputRamp":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentqOutputLitmit":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentqLowPassFilter":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentdProportionalGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentdIntegralGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentdDerivativeGain":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentdOutputRamp":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentdOutputLitmit":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentdLowPassFilter":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "VelocityLimit":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "LC" + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "VoltageLimit":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "LU" + "\n"));
                        Thread.Sleep(dt);
                        break;
                    case "CurrentLimit":
                        SendData(Encoding.ASCII.GetBytes(textBox6.Text + "LC" + "\n"));
                        Thread.Sleep(dt);
                        break;
                }
            }
            catch { }
        }
        private void PortDataReceive(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] buffer = new byte[port.BytesToRead];
                Int32 lengh = port.Read(buffer, 0, buffer.Length);
                string str = Encoding.ASCII.GetString(buffer).Replace("\r\n", string.Empty).Replace(" ", string.Empty);
                string[] strArray = str.Split(':');
                switch (strArray[0])
                {
                    case "Motion":
                        switch (strArray[1])
                        {
                            case "angle":
                                //tp.MCT = (tuneProperty.MotionControlType)1;
                                //propertyGrid1?.Invoke(new EventHandler(delegate
                                //{
                                //    propertyGrid1.Refresh();
                                //}));
                                tp.MCT = tuneProperty.MotionControlType.Angle;
                                propertyGrid1?.Invoke(new Action(() =>
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                            case "velocity":
                                tp.MCT = tuneProperty.MotionControlType.Velocity;
                                propertyGrid1?.Invoke(new Action(() =>
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                            case "torque":
                                tp.MCT = tuneProperty.MotionControlType.Torque;
                                propertyGrid1?.Invoke(new Action(() =>
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                            case "VelocityOpenloop":
                                tp.MCT = tuneProperty.MotionControlType.VelocityOpenloop;
                                propertyGrid1?.Invoke(new Action(() =>
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                            case "AngleOpenloop":
                                tp.MCT = tuneProperty.MotionControlType.AngleOpenloop;
                                propertyGrid1?.Invoke(new Action(() =>
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                            case "downsample":
                                tp.MotionDownsample = Int32.Parse(strArray[2]);
                                propertyGrid1?.Invoke(new Action(() =>
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                        }
                        break;
                    case "Torque":
                        switch (strArray[1])
                        {
                            case "foccurr":
                                tp.TCT = tuneProperty.TorqueControlType.FOCCurrent;
                                propertyGrid1.Invoke(new EventHandler(delegate
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                            case "dccurr":
                                tp.TCT = tuneProperty.TorqueControlType.DCCurrent;
                                propertyGrid1.Invoke(new EventHandler(delegate
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                            case "volt":
                                tp.TCT = tuneProperty.TorqueControlType.Voltage;
                                propertyGrid1.Invoke(new EventHandler(delegate
                                {
                                    propertyGrid1.Refresh();
                                }));
                                break;
                        }
                        break;
                    case "PIDvel|P":
                        tp.VelocityProportionalGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDvel|I":
                        tp.VelocityIntegralGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDvel|D":
                        tp.VelocityDerivativeGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDvel|ramp":
                        tp.VelocityOutputRamp = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDvel|limit":
                        tp.VelocityOutputLitmit = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDvel|Tf":
                        tp.VelocityLowPassFilter = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDangle|P":
                        tp.AngleProportionalGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDangle|I":
                        tp.AngleIntegralGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDangle|D":
                        tp.AngleDerivativeGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDangle|ramp":
                        tp.AngleOutputRamp = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDangle|limit":
                        tp.AngleOutputLitmit = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDangle|Tf":
                        tp.AngleLowPassFilter = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrd|P":
                        tp.CurrentdProportionalGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrd|I":
                        tp.CurrentdIntegralGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrd|D":
                        tp.CurrentdDerivativeGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrd|ramp":
                        tp.CurrentdOutputRamp = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrd|limit":
                        tp.CurrentdOutputLitmit = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrd|Tf":
                        tp.CurrentdLowPassFilter = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrq|P":
                        tp.CurrentqProportionalGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrq|I":
                        tp.CurrentqIntegralGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrq|D":
                        tp.CurrentqDerivativeGain = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrq|ramp":
                        tp.CurrentqOutputRamp = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrq|limit":
                        tp.CurrentqOutputLitmit = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PIDcurrq|Tf":
                        tp.CurrentqLowPassFilter = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "Limits|vel":
                        tp.VelocityLimit = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "Limits|volt":
                        tp.VoltageLimit = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "Limits|curr":
                        tp.CurrentLimit = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "Sensor|offset":
                        tp.ElectricalZeroOffset = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "Sensor|el.offset":
                        tp.ZeroAngleOffset = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "Rphase":
                        tp.PhaseResistance = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                    case "PWMMod|center":
                        break;
                    case "Status":
                        tp.MotorStatus = double.Parse(strArray[1]);
                        propertyGrid1.Invoke(new EventHandler(delegate
                        {
                            propertyGrid1.Refresh();
                        }));
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int dt=10;
            Task.Factory.StartNew(async () =>
            {
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "C" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "T" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VP" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VI" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VR" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VL" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "VF" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AP" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AI" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AR" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AL" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "AF" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DP" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DI" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DR" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DL" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "DF" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QP" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QI" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QR" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QL" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "QF" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "LV" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "LU" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "LC" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "SM" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "SE" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "CD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "R" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "WC" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBox6.Text + "E" + "\n"));
            });
        }

    }

}
