using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JSFW.PREZI
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //Application.Run(new Demo_Transparent_PanelForm());
            //Application.Run(new Demo_Task_Progress());
            //Application.Run(new Demo_StackTrace()); 
            //Application.Run(new Demo_SplashWindow());
        }
    }
     
}
