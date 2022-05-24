using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleFOCTuning
{
    public sealed class serialConfig
    {
        private static readonly object padlock =new object();
        private static serialConfig _instance = null;
        public static serialConfig Instance { 
            get 
            {
                if(_instance == null)
                {
                    lock (padlock)
                    {
                        if(_instance == null)
                        {
                            _instance = new serialConfig();
                        }
                    }
                }
                return _instance; 
            } 
        }
        private string _portName, _parity, _connID, _stopbits;
        private int _baud, _bytebits;
        public int myBaud
        {
            get { return _baud; }
            set { _baud = value; }
        }
        public int myBytebits
        {
            get { return _bytebits; }
            set { _bytebits = value; }
        }
        public string myStopbits
        {
            get { return _stopbits; }
            set { _stopbits = value; }
        }
        public string myPortName
        {
            get { return _portName; }
            set { _portName = value; }
        }
        public string myParity
        {
            get { return _parity; }
            set { _parity = value; }
        }
        public string myConnID
        {
            get { return _connID; }
            set { _connID = value; }
        }


    }
}
