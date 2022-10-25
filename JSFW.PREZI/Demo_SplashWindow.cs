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
    public partial class Demo_SplashWindow : Form
    {
       
        public Demo_SplashWindow()
        {
            InitializeComponent();             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // ON
            SplashForm.ShowSplashForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // OFF
            SplashForm.CloseForm();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SplashForm.SetStatus(textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 빅!
            button4.Enabled = false;
            progressBar1.Maximum = 100;
            progressBar1.Value = progressBar1.Minimum = 1;
            SplashForm.BigDataProgressing(() => {
                for (int loop = progressBar1.Minimum; loop <= progressBar1.Maximum; loop++)
                {
                    progressBar1.DoAsync(p => p.Value = loop);
                    SplashForm.SetStatus("Count=" + loop);
                    System.Threading.Thread.Sleep(150);
                }
                button4.DoAsync( b => b.Enabled = true);
            });
        }

        private void button5_Click(object sender, EventArgs e)
        { 
            button5.Enabled = false;
            progressBar1.Maximum = 100;
            progressBar1.Value = progressBar1.Minimum = 1;

            var task = SplashForm.BigDataProgressingAsync(() => {
                for (int loop = progressBar1.Minimum; loop <= progressBar1.Maximum; loop++)
                {
                    progressBar1.DoAsync(p => p.Value = loop);
                    SplashForm.SetStatus("Count=" + loop);
                    System.Threading.Thread.Sleep(150);
                     
                }

            });

            task.ContinueWith(x => {
                    SplashForm.CloseForm();
                    button5.Enabled = true;
                },
                TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
