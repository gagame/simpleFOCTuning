using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace simpleFOCTuning
{
    public class RS485Helper
    {
        SerialPort port = new SerialPort();
        serialConfig sc = serialConfig.Instance;

        public EventHandler<PortReceiveDataEventArgs> portReceiveEventHandler;

        public Queue<string> sendBuffer = new Queue<string>();
        private bool isBusy = false;
        private bool isCancel = false;
        private int timeout = 10;
        private BackgroundTimer timer;
        private Task task;

        public RS485Helper()
        {
            init();
        }
        private void init()
        {
            port.DataReceived += PortReceiveData;
            timer = new BackgroundTimer();
        }
        public bool IsOpen
        {
            get
            {
                return port.IsOpen;
            }
        }

        public void PortOpenClose()
        {
            try
            {
                if (!port.IsOpen)
                {
                    port.PortName = sc.myPortName;
                    port.BaudRate = sc.myBaud;
                    port.StopBits = (sc.myStopbits == "1") ? StopBits.One : StopBits.Two;
                    if (sc.myParity == "None") port.Parity = Parity.None;
                    else if (sc.myParity == "Odd") port.Parity = Parity.Odd;
                    else if (sc.myParity == "Even") port.Parity = Parity.Even;
                    else if (sc.myParity == "Space") port.Parity = Parity.Space;
                    port.DataBits = sc.myBytebits;
                    port.Open();
                }
                else if (port.IsOpen)
                {
                    port.Close();
                    isCancel = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int SendBufferSize
        {
            get
            {
                return sendBuffer.Count;
            }
        }
        public void ClearSendBuffer()
        {
            sendBuffer.Clear();
        }
        public void PortSendData(string send)
        {
            sendBuffer.Enqueue(send);
        }

        public void startTask(CancellationToken token)
        {
            isCancel = false;
            task = new Task(async () =>
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    while (!isCancel)
                    {
                        if (token.IsCancellationRequested)
                        {
                            token.ThrowIfCancellationRequested();
                            return;
                        }

                        if (!isBusy || !timer.IsBusy || timer.IsArrival)
                        {
                            if (sendBuffer.Count > 0)
                            {
                                try
                                {
                                    //byte[] send = Encoding.ASCII.GetBytes(sendBuffer.Dequeue());
                                    //if(send!=null)port.Write(send, 0, send.Length);
                                    port.Write(sendBuffer.Dequeue());
                                    isBusy = true;
                                    timer.Start(timeout);
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                isBusy = false;
                                timer.Stop();
                            }
                        }

                        await Task.Delay(1);
                    }
                }
                catch
                {

                }
            }, token);
            task.Start();
        }


        private void PortReceiveData(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string tmp = port.ReadLine();
                portReceiveEventHandler?.Invoke(this,new PortReceiveDataEventArgs(tmp));
                isBusy = false;
            }
            catch
            {

            }
        }
    }
}
