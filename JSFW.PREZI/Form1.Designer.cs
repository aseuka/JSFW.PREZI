namespace JSFW.PREZI
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.testlistctrl1 = new JSFW.PREZI.TESTLISTCTRL();
            this.SuspendLayout();
            // 
            // testlistctrl1
            // 
            this.testlistctrl1.BackColor = System.Drawing.Color.White;
            this.testlistctrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testlistctrl1.Location = new System.Drawing.Point(0, 0);
            this.testlistctrl1.Name = "testlistctrl1";
            this.testlistctrl1.Size = new System.Drawing.Size(699, 433);
            this.testlistctrl1.TabIndex = 0;
            this.testlistctrl1.ItemAddClicked += new System.Func<JSFW.PREZI.Controls.ItemCtrl>(this.testlistctrl1_Added);
            this.testlistctrl1.ItemRemoved += new System.Action<bool, JSFW.PREZI.Controls.ItemCtrl>(this.testlistctrl1_ItemRemoved);
            this.testlistctrl1.Saved += new System.Action(this.testlistctrl1_Saved);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(699, 433);
            this.Controls.Add(this.testlistctrl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "프레지";
            this.ResumeLayout(false);

        }

        #endregion

        private TESTLISTCTRL testlistctrl1;
    }
}

