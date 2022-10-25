using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSFW.PREZI
{
    public partial class Demo_Tuple : Form
    {
        public Demo_Tuple()
        {
            InitializeComponent();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tuple<int, string> t = new Tuple<int, string>(0, "00000");

            int first = t.Item1;
            string second = t.Item2;


            //t.Item2 = ""+ 111;  // 튜블은 set 이 없음!
        }

        
    }
}
