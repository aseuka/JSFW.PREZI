using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//https://www.codeproject.com/Articles/21062/Splash-Screen-Control

namespace JSFW.PREZI
{
    
    public partial class SplashForm : Form
    { 
        static SplashForm splashForm = null;
        static Thread splashThread = null;

        public string Status
        {
            get { return label1.Text; }
            set { label1.DoAsync(l => l.Text = value); }
        }

        public SplashForm()
        {
            InitializeComponent();
        }

        //https://msdn.microsoft.com/en-us/library/windows/desktop/ff700543(v=vs.85).aspx
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_TOPMOST = 0x00000008;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOOLWINDOW | WS_EX_TOPMOST;
                cp.Parent = IntPtr.Zero;
                return cp;
            }
        }
         
        protected override void OnHandleCreated(EventArgs e)
        {
            if (this.Handle != IntPtr.Zero)
            {
                IntPtr hWndDeskTop = GetDesktopWindow();
                SetParent(this.Handle, hWndDeskTop);
                //Process currentProcess = Process.GetCurrentProcess();
                //Control MainForm = Control.FromHandle(currentProcess.MainWindowHandle);
                //SetParent(this.Handle, MainForm.Handle);
            }
            base.OnHandleCreated(e);
        }

        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();[DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
         
        public static void ShowSplashForm()
        {
            if (splashForm != null) return;

            splashThread = new Thread(new ThreadStart(SplashForm.ShowForm));
            splashThread.IsBackground = true;
            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.Start();
            while (splashForm == null || splashForm.IsHandleCreated == false)
            {
                Thread.Sleep(50);
            }
        }

        static private void ShowForm()
        {
            splashForm = new SplashForm();            
            Application.Run(splashForm);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            splashForm = null;
            splashThread = null;
        }

        public static void CloseForm()
        {
            if (splashForm != null && splashForm.IsDisposed == false)
            {
                // ...
                splashForm.DoAsync(s => s.Close());
            }
            splashThread = null;
            splashForm = null;
        }

        public static void SetStatus(string msg)
        {
            SetStatus(msg, true);
        }

        public static void SetStatus(string msg, bool setReference)
        {
            if (splashForm == null) return;

            splashForm.Status = msg;

            if (setReference)
                splashForm.SetReferenceInternal();
        }

        private static void SetReferencePoint()
        {
            if (splashForm == null) return;

            splashForm.SetReferenceInternal();
        }

        private void SetReferenceInternal()
        {
            // 뭔가??? 진행율 같은걸.. 표시하는 것인가?
        }

        public static SplashForm GetSplashForm()
        {
            return splashForm;
        }

        public static void BigDataProgressing(Action act)
        {
            ShowSplashForm();
            var task = Task.Run(() =>
            {
                act();
            });
            task.ContinueWith(x => { CloseForm(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static async Task BigDataProgressingAsync(Action act)
        {
            ShowSplashForm();
            var task = Task.Run(() =>
            {
                act();
            });            
            await task;
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {

        }
    }
}
