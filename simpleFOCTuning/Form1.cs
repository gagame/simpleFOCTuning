using LiveCharts.Geared;
using LiveCharts.Wpf;
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
using Brushes = System.Windows.Media.Brushes;

namespace simpleFOCTuning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //cartesianChart1.Series.Add(new LineSeries
            //{
            //    Values = _viewModelTarget.Values,
            //    Fill = Brushes.Transparent,
            //    StrokeThickness = 1,
            //    PointGeometry = null
            //});
            //cartesianChart1.Series.Add(new LineSeries
            //{
            //    Values = _viewModelAngle.Values,
            //    Fill = Brushes.Transparent,
            //    StrokeThickness = 1,
            //});
            //cartesianChart1.DisableAnimations = true;
        }

        private tuneProperty tp = new tuneProperty();
        private RS485Helper rs485Helper = new RS485Helper();
        private CancellationTokenSource ctsRS485;
        private CancellationTokenSource ctsMain;
        Task task1;
        bool isCancel = false;
        int sampleRate = 100;
        int delayTime = 10;
        private LiveChart _viewModelAngle = new LiveChart();
        private LiveChart _viewModelTarget = new LiveChart();


        private void Form1_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = tp;
            propertyGrid1.ToolbarVisible = false;
            propertyGrid1.HelpVisible = false;
            propertyGrid1.PropertySort = PropertySort.Categorized;
            propertyGrid1.PropertyValueChanged += PropertyChange;

            rs485Helper.portReceiveEventHandler += UpdateUI;
        }

        private void buttonConfigure_Click(object sender, EventArgs e)
        {
            ConfigureSerialConnect f1 = new ConfigureSerialConnect();
            f1.ShowDialog(this);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            rs485Helper.PortOpenClose();
            ctsRS485 = new CancellationTokenSource();
            ctsRS485.Token.Register(() =>
            {
                //when rs485Helper task cancel
            });
            if (rs485Helper.IsOpen)
            {
                this.Invoke(new Action(() =>
                {
                    rs485Helper.startTask(ctsRS485.Token);
                    buttonConnect.Text = "Disconnect";
                    createTask();
                }));
            }
            else if (!rs485Helper.IsOpen)
            {
                this.Invoke(new Action(() =>
                {
                    buttonConnect.Text = "Connect";
                    cancelTask();
                }));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Enable Device")
            {
                SendData(textBoxConnectCMD.Text + "E1" + "\n");
                button1.Text = "Disable Device";
            }
            else if (button1.Text == "Disable Device")
            {
                SendData(textBoxConnectCMD.Text + "E0" + "\n");
                button1.Text = "Enable Device";
            }
        }
        private void SendData(string sendBuffer)
        {
            if (sendBuffer != null)
            {
                try
                {
                    rs485Helper.PortSendData(sendBuffer);
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
                switch (e.ChangedItem.PropertyDescriptor.Name)
                {
                    case "MCT":
                        switch (e.ChangedItem.Value)
                        {
                            case tuneProperty.MotionControlType.Torque:
                                SendData(textBoxConnectCMD.Text + "C0" + "\n");
                                break;
                            case tuneProperty.MotionControlType.Velocity:
                                SendData(textBoxConnectCMD.Text + "C1" + "\n");
                                break;
                            case tuneProperty.MotionControlType.Angle:
                                SendData(textBoxConnectCMD.Text + "C2" + "\n");
                                break;
                            case tuneProperty.MotionControlType.VelocityOpenloop:
                                SendData(textBoxConnectCMD.Text + "C3" + "\n");
                                break;
                            case tuneProperty.MotionControlType.AngleOpenloop:
                                SendData(textBoxConnectCMD.Text + "C4" + "\n");
                                break;
                        }
                        break;
                    case "TCT":
                        switch (e.ChangedItem.Value)
                        {
                            case tuneProperty.TorqueControlType.Voltage:
                                SendData(textBoxConnectCMD.Text + "T0" + "\n");
                                break;
                            case tuneProperty.TorqueControlType.DCCurrent:
                                SendData(textBoxConnectCMD.Text + "T1" + "\n");
                                break;
                            case tuneProperty.TorqueControlType.FOCCurrent:
                                SendData(textBoxConnectCMD.Text + "T2" + "\n");
                                break;
                        }
                        break;
                    case "VelocityProportionalGain":
                        SendData(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "MotionDownsample":
                        SendData(textBoxConnectCMD.Text + "CD" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "VelocityIntegralGain":
                        SendData(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "VelocityDerivativeGain":
                        SendData(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "VelocityOutputRamp":
                        SendData(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "VelocityOutputLitmit":
                        SendData(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "VelocityLowPassFilter":
                        SendData(textBoxConnectCMD.Text + "VP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "AngleProportionalGain":
                        SendData(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "AngleIntegralGain":
                        SendData(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "AngleDerivativeGain":
                        SendData(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "AngleOutputRamp":
                        SendData(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "AngleOutputLitmit":
                        SendData(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "AngleLowPassFilter":
                        SendData(textBoxConnectCMD.Text + "AP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentqProportionalGain":
                        SendData(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentqIntegralGain":
                        SendData(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentqDerivativeGain":
                        SendData(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentqOutputRamp":
                        SendData(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentqOutputLitmit":
                        SendData(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentqLowPassFilter":
                        SendData(textBoxConnectCMD.Text + "QP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentdProportionalGain":
                        SendData(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentdIntegralGain":
                        SendData(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentdDerivativeGain":
                        SendData(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentdOutputRamp":
                        SendData(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentdOutputLitmit":
                        SendData(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentdLowPassFilter":
                        SendData(textBoxConnectCMD.Text + "DP" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "VelocityLimit":
                        SendData(textBoxConnectCMD.Text + "LC" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "VoltageLimit":
                        SendData(textBoxConnectCMD.Text + "LU" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "CurrentLimit":
                        SendData(textBoxConnectCMD.Text + "LC" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "ZeroAngleOffset":
                        SendData(textBoxConnectCMD.Text + "SM" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "ElectricalZeroOffset":
                        SendData(textBoxConnectCMD.Text + "SE" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "PhaseResistance":
                        SendData(textBoxConnectCMD.Text + "R" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                    case "MotorStatus":
                        SendData(textBoxConnectCMD.Text + "E" + e.ChangedItem.Value.ToString() + "\n");
                        break;
                }
            }
            catch { }
        }
        private void UpdateUI(object sender, PortReceiveDataEventArgs e)
        {
            try
            {
                //string str = Encoding.ASCII.GetString(buffer).Replace("\r\n", string.Empty).Replace(" ", string.Empty);
                string[] strArray = new string[] { };
                this.Invoke(new Action(() =>
                {
                    string str = e.ReceiveData.Replace("\r\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);
                    strArray = str.Split(new char[] { ':', '\t' });
                    // set the current caret position to the end
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    // scroll it automatically
                    richTextBox1.ScrollToCaret();
                    //richTextBox1.Text += e.ReceiveData;
                }));
                switch (strArray[0])
                {
                    case "Monitor|target":
                        this.Invoke(new Action(() =>
                        {
                            try
                            {
                                _viewModelTarget.Trend = double.Parse(strArray[1]);
                                textBoxTopTarget.Text = strArray[1];
                            }
                            catch
                            {

                            }
                        }));
                        break;
                    case "Monitor|Vq":
                        this.Invoke(new Action(() =>
                        {
                            textBoxTopVoltage.Text = strArray[1];
                        }));
                        break;
                    case "Monitor|Vd":
                        break;
                    case "Monitor|Cq":
                        break;
                    case "Monitor|Cd":
                        break;
                    case "Monitor|vel":
                        this.Invoke(new Action(() =>
                        {
                            textBoxTopVelocity.Text = strArray[1];
                        }));
                        break;
                    case "Monitor|angle":
                        this.Invoke(new Action(() =>
                        {
                            try
                            {
                                _viewModelAngle.Trend = double.Parse(strArray[1]);
                                textBoxTopAngle.Text = strArray[1];
                            }
                            catch
                            {

                            }
                        }));
                        break;
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
        private void createTask()
        {
            if (task1?.Status != TaskStatus.Running)
            {
                isCancel = false;
                ctsMain = new CancellationTokenSource();
                var token1 = ctsMain.Token;
                task1 = new Task(() =>
                {
                    try
                    {
                        token1.ThrowIfCancellationRequested();
                        while (!isCancel)
                        {
                            Console.WriteLine(rs485Helper.SendBufferSize);
                            if (rs485Helper.SendBufferSize > 30)
                            {
                                rs485Helper.ClearSendBuffer();
                            }
                            GetStatue();
                            if (token1.IsCancellationRequested)
                                token1.ThrowIfCancellationRequested();
                            Thread.Sleep(sampleRate);
                        }
                    }
                    catch
                    {

                    }
                }, ctsMain.Token);
                task1.Start();
            }
        }

        private void cancelTask()
        {
            if (ctsMain != null)
            {
                isCancel = true;
                ctsMain.Cancel();
            }
        }
        public void GetStatue()
        {
            SendData(textBoxConnectCMD.Text + "MG0" + "\n");
            Thread.Sleep(delayTime);
            SendData(textBoxConnectCMD.Text + "MG1" + "\n");
            Thread.Sleep(delayTime);
            SendData(textBoxConnectCMD.Text + "MG2" + "\n");
            Thread.Sleep(delayTime);
            SendData(textBoxConnectCMD.Text + "MG3" + "\n");
            Thread.Sleep(delayTime);
            SendData(textBoxConnectCMD.Text + "MG4" + "\n");
            Thread.Sleep(delayTime);
            SendData(textBoxConnectCMD.Text + "MG5" + "\n");
            Thread.Sleep(delayTime);
            SendData(textBoxConnectCMD.Text + "MG6" + "\n");
            Thread.Sleep(delayTime);
        }
        private void buttonPullParams_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(async () =>
            {
                SendData(textBoxConnectCMD.Text + "C" + "\n");
                SendData(textBoxConnectCMD.Text + "T" + "\n");
                SendData(textBoxConnectCMD.Text + "CD" + "\n");
                SendData(textBoxConnectCMD.Text + "VP" + "\n");
                SendData(textBoxConnectCMD.Text + "VI" + "\n");
                SendData(textBoxConnectCMD.Text + "VD" + "\n");
                SendData(textBoxConnectCMD.Text + "VR" + "\n");
                SendData(textBoxConnectCMD.Text + "VL" + "\n");
                SendData(textBoxConnectCMD.Text + "VF" + "\n");
                SendData(textBoxConnectCMD.Text + "AP" + "\n");
                SendData(textBoxConnectCMD.Text + "AI" + "\n");
                SendData(textBoxConnectCMD.Text + "AD" + "\n");
                SendData(textBoxConnectCMD.Text + "AR" + "\n");
                SendData(textBoxConnectCMD.Text + "AL" + "\n");
                SendData(textBoxConnectCMD.Text + "AF" + "\n");
                SendData(textBoxConnectCMD.Text + "DP" + "\n");
                SendData(textBoxConnectCMD.Text + "DI" + "\n");
                SendData(textBoxConnectCMD.Text + "DD" + "\n");
                SendData(textBoxConnectCMD.Text + "DR" + "\n");
                SendData(textBoxConnectCMD.Text + "DL" + "\n");
                SendData(textBoxConnectCMD.Text + "DF" + "\n");
                SendData(textBoxConnectCMD.Text + "QP" + "\n");
                SendData(textBoxConnectCMD.Text + "QI" + "\n");
                SendData(textBoxConnectCMD.Text + "QD" + "\n");
                SendData(textBoxConnectCMD.Text + "QR" + "\n");
                SendData(textBoxConnectCMD.Text + "QL" + "\n");
                SendData(textBoxConnectCMD.Text + "QF" + "\n");
                SendData(textBoxConnectCMD.Text + "LV" + "\n");
                SendData(textBoxConnectCMD.Text + "LU" + "\n");
                SendData(textBoxConnectCMD.Text + "LC" + "\n");
                SendData(textBoxConnectCMD.Text + "SM" + "\n");
                SendData(textBoxConnectCMD.Text + "SE" + "\n");
                SendData(textBoxConnectCMD.Text + "R" + "\n");
                SendData(textBoxConnectCMD.Text + "WC" + "\n");
                //await Task.Delay(dt);
                SendData(textBoxConnectCMD.Text + "E" + "\n");
            });
        }
        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendData(textBoxCMD.Text + "\n");
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
                        if (buttonGeneral.Text == "Stop") buttonGeneral.Text = "Start";
                        else if (buttonGeneral.Text == "Start") buttonGeneral.Text = "Stop";
                    }));
                    while (buttonGeneral.Text == "Stop")
                    {
                        SendData(textBoxGeneralCMD.Text + textBoxPos1.Text + "\n");
                        await Task.Delay(Int32.Parse(textBoxInterval.Text));
                        SendData(textBoxGeneralCMD.Text + textBoxPos2.Text + "\n");
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
            _viewModelTarget.Read();
            _viewModelAngle.Read();
        }
        private void buttonChartPause_Click(object sender, EventArgs e)
        {
            _viewModelTarget.Stop();
            _viewModelAngle.Stop();
        }
        private void buttonChartClear_Click(object sender, EventArgs e)
        {
            _viewModelTarget.Values.Clear();
            _viewModelAngle.Values.Clear();
        }

    }

}
