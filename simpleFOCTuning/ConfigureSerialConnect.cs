using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simpleFOCTuning
{
    public partial class ConfigureSerialConnect : Form
    {
        public ConfigureSerialConnect()
        {
            InitializeComponent();
        }
        private serialConfig sc = serialConfig.Instance;
        private void button1_Click(object sender, EventArgs e)
        {
            this.sc.myConnID = this.textBox2.Text;
            this.sc.myBaud = Int32.Parse(this.textBox1.Text);
            this.sc.myParity = this.comboBox2.Text;
            this.sc.myStopbits = this.comboBox3.Text;
            this.sc.myBytebits = Int32.Parse(this.comboBox4.Text);
            this.sc.myPortName = this.comboBox1.Text;
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            string[] portName = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(portName);
        }

    }
}
