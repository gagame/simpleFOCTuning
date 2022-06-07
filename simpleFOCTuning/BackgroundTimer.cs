using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleFOCTuning
{
    public class BackgroundTimer
    {
        public delegate void TimeArrivalEvent(object sender, EventArgs e);
        /// <summary>發生於計時到達時</summary>
        public event TimeArrivalEvent TimeArrival;

        System.Diagnostics.Stopwatch SW = new System.Diagnostics.Stopwatch();

        BackgroundWorker TimeWorker = new BackgroundWorker() { WorkerSupportsCancellation = true };

        /// <summary>計時亳秒數</summary>
        public long Timeout { get { return m_Timeout; } }
        /// <summary>是否時間到</summary>
        public bool IsArrival { get { return m_IsArrival; } set { m_IsArrival = value; } }
        /// <summary>已秏用時間(亳秒)</summary>
        public long ElapsedMilliseconds { get { return SW.ElapsedMilliseconds; } }
        /// <summary>是否計時中</summary>
        public bool IsBusy { get { return m_Is_Busy; } }

        /// <summary>計時亳秒數(隱)</summary>
        long m_Timeout;
        /// <summary>是否時間到(隱)</summary>
        bool m_IsArrival = false;
        /// <summary>是否計時中(隱)</summary>
        bool m_Is_Busy = false;

        public BackgroundTimer()
        {
            m_IsArrival = false;
            TimeWorker.DoWork += new DoWorkEventHandler(TimeWorker_DoWork);
        }

        public BackgroundTimer(int Timeout)
        {
            m_IsArrival = false;
            m_Is_Busy = false;
            this.m_Timeout = Timeout;
            TimeWorker.DoWork += new DoWorkEventHandler(TimeWorker_DoWork);
        }

        public BackgroundTimer(decimal Timeout)
        {
            m_IsArrival = false;
            m_Is_Busy = false;
            this.m_Timeout = (long)Timeout;
            TimeWorker.DoWork += new DoWorkEventHandler(TimeWorker_DoWork);
        }

        private void TimeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (m_Is_Busy)
            {
                if (SW.ElapsedMilliseconds >= this.m_Timeout)
                {
                    m_IsArrival = true;
                    Stop();
                    if (TimeArrival != null) TimeArrival(this, null);
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        /// <summary>開始計時</summary>
        public void Start()
        {
            if (!m_Is_Busy)
            {
                m_Is_Busy = true;
                SW.Start();
                if (!TimeWorker.IsBusy)
                    TimeWorker.RunWorkerAsync();
            }
        }
        /// <summary>開始計時</summary>
        public void Start(long Timeout)
        {
            if (!m_Is_Busy)
            {

                this.m_Timeout = Timeout;
                Start();
            }
        }

        public void Start(decimal Timeout)
        {
            if (!m_Is_Busy)
            {
                this.m_Timeout = (long)Timeout;
                Start();
            }
        }

        /// <summary>停止計時並將計時器歸零</summary>
        public void Rest()
        {
            m_IsArrival = false;
            m_Is_Busy = false;
            SW.Reset();
        }
        /// <summary>停止計時並將計時器歸零，然後開始重新計時。</summary>
        public void Restart()
        {
            m_IsArrival = false;
            m_Is_Busy = false;
            SW.Reset();
            Start();
            SW.Restart();
        }
        /// <summary>停止計時並將計時器歸零，然後依Timeout設定開始重新計時。</summary>
        public void Restart(long Timeout)
        {
            this.m_Timeout = Timeout;
            Restart();
        }
        public void Restart(decimal Timeout)
        {
            this.m_Timeout = (long)Timeout;
            Restart();
        }
        /// <summary>停止計時。</summary>
        public void Stop()
        {
            m_Is_Busy = false;
            SW.Stop();
        }
    }
}
