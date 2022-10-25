using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSFW.PREZI
{
    public partial class Demo_StackTrace : Form
    {
        public Demo_StackTrace()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TraceMessage();
        }


        public void TraceMessage(
            [CallerMemberName]string mem = "", 
            [CallerFilePath]string path = "", 
            [CallerLineNumber]int line = 0)        
        {

            if (!string.IsNullOrEmpty((""+mem).Trim()))
            {
                textBox1.AppendText(mem + Environment.NewLine);
                textBox1.AppendText(path + Environment.NewLine);
                textBox1.AppendText(line + Environment.NewLine);
                textBox1.AppendText(Environment.NewLine);
            }
        }
    }
}
