using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleFOCTuning
{
    public class PortReceiveDataEventArgs : EventArgs
    {
        public PortReceiveDataEventArgs(string data)
        {
            ReceiveData = data;
        }
        public string ReceiveData { get; set; }
    }
}
