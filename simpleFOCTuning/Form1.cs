using LiveCharts.Geared;
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

            cartesianChart1.Series.Add(new GLineSeries
            {
                Values = _viewModel.Values
            });
            cartesianChart1.DisableAnimations = true;
        }

        serialConfig sc = serialConfig.Instance;
        tuneProperty tp = new tuneProperty();
        SerialPort port = new SerialPort();



        private LiveChart _viewModel = new LiveChart();


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

        private void buttonConfigure_Click(object sender, EventArgs e)
        {
            ConfigureSerialConnect f1 = new ConfigureSerialConnect();
            f1.ShowDialog(this);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
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
                    buttonConnect.Text = "Disconnect";
                }
                else if (port.IsOpen)
                {
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                    port.Close();
                    GC.Collect();
                    buttonConnect.Text = "Connect";
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
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "E1" + "\n"));
                button1.Text = "Disable Device";
            }
            else if (button1.Text == "Disable Device")
            {
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "E0" + "\n"));
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
                int dt = 5;
                switch (e.ChangedItem.PropertyDescriptor.Name)
                {
                    case "MCT":
                        switch (e.ChangedItem.Value)
                        {
                            case tuneProperty.MotionControlType.Torque:
                                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "C0" + "\n"));
                                break;
                            case tuneProperty.MotionControlType.Velocity:
                                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "C1" + "\n"));
                                break;
                            case tuneProperty.MotionControlType.Angle:
                                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "C2" + "\n"));
                                break;
                            case tuneProperty.MotionControlType.VelocityOpenloop:
                                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "C3" + "\n"));
                                break;
                            case tuneProperty.MotionControlType.AngleOpenloop:
                                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "C4" + "\n"));
                                break;
                        }
                        break;
                    case "TCT":
                        switch (e.ChangedItem.Value)
                        {
                            case tuneProperty.TorqueControlType.Voltage:
                                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "T0" + "\n"));
                                break;
                            case tuneProperty.TorqueControlType.DCCurrent:
                                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "T1" + "\n"));
                                break;
                            case tuneProperty.TorqueControlType.FOCCurrent:
                                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "T2" + "\n"));
                                break;
                        }
                        break;
                    case "VelocityProportionalGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "MotionDownsample":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "CD" + e.ChangedItem.Value.ToString()+ "\n"));
                        break;
                    case "VelocityIntegralGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "VelocityDerivativeGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "VelocityOutputRamp":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "VelocityOutputLitmit":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "VelocityLowPassFilter":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "AngleProportionalGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "AngleIntegralGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "AngleDerivativeGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "AngleOutputRamp":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "AngleOutputLitmit":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "AngleLowPassFilter":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentqProportionalGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentqIntegralGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentqDerivativeGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentqOutputRamp":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentqOutputLitmit":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentqLowPassFilter":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentdProportionalGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentdIntegralGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentdDerivativeGain":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentdOutputRamp":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentdOutputLitmit":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentdLowPassFilter":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "VelocityLimit":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "LC" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "VoltageLimit":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "LU" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "CurrentLimit":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "LC" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "ZeroAngleOffset":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "SM" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "ElectricalZeroOffset":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "SE" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "PhaseResistance":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "R" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                    case "MotorStatus":
                        SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "E" + e.ChangedItem.Value.ToString() + "\n"));
                        break;
                }
            }
            catch { }
        }
        private void PortDataReceive(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(5);
                byte[] buffer = new byte[port.BytesToRead];
                Int32 lengh = port.Read(buffer, 0, buffer.Length);
                string str = Encoding.ASCII.GetString(buffer).Replace("\r\n", string.Empty).Replace(" ", string.Empty);
                string[] strArray = str.Split(new char[] { ':', '\t' });
                var rawBuffer = Encoding.ASCII.GetString(buffer);
                this.Invoke(new Action(() =>
                {
                    // set the current caret position to the end
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    // scroll it automatically
                    richTextBox1.ScrollToCaret();
                    richTextBox1.Text += rawBuffer;
                }));
                if(double.TryParse(strArray[0], out double n)&&strArray.Length>6)
                {
                    this.Invoke(new Action(() =>
                    {
                        _viewModel.Trend=double.Parse(strArray[6]);
                        textBoxTopAngle.Text = strArray[6];
                        textBoxTopVelocity.Text = strArray[5];
                        textBoxTopVoltage.Text = strArray[1];
                        textBoxTopTarget.Text = strArray[0];
                    }));
                }
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

        private void buttonPullParams_Click(object sender, EventArgs e)
        {
            int dt=20;
            Task.Factory.StartNew(async () =>
            {
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "C" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "T" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "CD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VP" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VI" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VR" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VL" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "VF" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AP" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AI" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AR" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AL" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "AF" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DP" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DI" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DR" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DL" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "DF" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QP" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QI" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QD" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QR" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QL" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "QF" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "LV" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "LU" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "LC" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "SM" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "SE" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "R" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "WC" + "\n"));
                await Task.Delay(dt);
                SendData(Encoding.ASCII.GetBytes(textBoxConnectCMD.Text + "E" + "\n"));
            });
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendData(Encoding.ASCII.GetBytes(textBoxCMD.Text + "\n"));
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                richTextBox1.Clear();
            }));
        }

        private void buttonGeneral_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Factory.StartNew(async () =>
                {
                    this.Invoke(new Action(() =>
                    {
                        if(buttonGeneral.Text == "Stop")buttonGeneral.Text ="Start";
                        else if(buttonGeneral.Text == "Start") buttonGeneral.Text = "Stop";
                    }));
                    while(buttonGeneral.Text == "Stop")
                    {
                        SendData(Encoding.ASCII.GetBytes(textBoxGeneralCMD.Text + textBoxPos1.Text + "\n"));
                        await Task.Delay(Int32.Parse(textBoxInterval.Text));
                        SendData(Encoding.ASCII.GetBytes(textBoxGeneralCMD.Text + textBoxPos2.Text + "\n"));
                        await Task.Delay(Int32.Parse(textBoxInterval.Text));
                    }
                });
            }
            catch (Exception ex)
            {
                ;
            }
        }

        private void buttonChartStart_Click(object sender, EventArgs e)
        {
            _viewModel.Read();
        }
    }

}
