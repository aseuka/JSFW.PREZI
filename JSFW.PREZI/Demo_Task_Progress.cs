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
    public partial class Demo_Task_Progress : Form
    {
        public Demo_Task_Progress()
        {
            InitializeComponent();
        }


        bool IsRunning1 = false;
        private void button1_Click(object sender, EventArgs e)
        {
            // 테스트.. 
            IsRunning1 = true;
            progressBar1.Maximum = 100;
            progressBar1.Value = progressBar1.Minimum =  1;
            button1.Enabled = false;
            Action doAction = Progressing1;
            doAction.BeginInvoke(ir => { doAction.EndInvoke(ir); button1.DoAsync(b => b.Enabled = true); }, doAction);
        }

        private void Progressing1()
        {
            for (int loop = progressBar1.Minimum; IsRunning1 && loop <= progressBar1.Maximum; loop++)
            {
                System.Threading.Thread.Sleep(333);
                progressBar1.DoAsync(p => p.Value = loop);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IsRunning1 = false;
        }



        bool IsRunning2 = false;
        private void button2_Click(object sender, EventArgs e)
        {
            // 테스트
            IsRunning2 = true;            
            progressBar2.Maximum = 100;
            progressBar2.Value = progressBar2.Minimum = 1;
            button2.Enabled = false;
            //Progressing2_1();
            Progressing2_2();
        }

        private async void Progressing2_1()
        {
            await Task.Run(() =>
            {
                for (int loop = progressBar2.Minimum; IsRunning2 && loop <= progressBar2.Maximum; loop++)
                {
                    System.Threading.Thread.Sleep(333);
                    progressBar2.DoAsync(p => p.Value = loop);
                }
            });

            button2.Enabled = true;
        }

        private void Progressing2_2()
        { 
            var task = Task.Run(() =>
            {
                for (int loop = progressBar2.Minimum; IsRunning2 && loop <= progressBar2.Maximum; loop++)
                {
                    System.Threading.Thread.Sleep(333);
                    progressBar2.DoAsync(p => p.Value = loop);
                }
            });

            task.ContinueWith((x) => { button2.Enabled = true; }, TaskScheduler.FromCurrentSynchronizationContext());
        }
         
        private void button4_Click(object sender, EventArgs e)
        {
            IsRunning2 = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 혹시나 해서... 구핸해봄.  ( 이건 UI상에서 비동기로 동작하지 않음. : 꼼짝못함. )
            // http://www.google.co.kr/url?sa=i&rct=j&q=&esrc=s&source=images&cd=&cad=rja&uact=8&ved=0ahUKEwjk4-Gj5fTVAhVTv5QKHaZ_CqkQjhwIBQ&url=http%3A%2F%2Fwww.dotnetcurry.com%2FShowArticle.aspx%3FID%3D642&psig=AFQjCNHN-m5xAJEaOGTEgpgKBHtgWmnGqg&ust=1503829696553944
            Func<int, int> sumAction = Sum;
            textBox1.Text = "시작!";
            IAsyncResult async = sumAction.BeginInvoke(10, null, null);

            while (async.IsCompleted == false)
            {
                //textBox1.Text += " .";
                System.Threading.Thread.Sleep(200);
            }

            int result = sumAction.EndInvoke(async);

            textBox1.Text = "" + result;
        }

        public int Sum(int num)
        {
            System.Threading.Thread.Sleep(2000);
            return num * num;
        }
    }
}
