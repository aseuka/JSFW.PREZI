namespace JSFW.PREZI
{
    partial class PreziDesignForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.preziDesignControl1 = new JSFW.PREZI.Controls.PreziDesignControl();
            this.SuspendLayout();
            // 
            // preziDesignControl1
            // 
            this.preziDesignControl1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.preziDesignControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.preziDesignControl1.Location = new System.Drawing.Point(0, 0);
            this.preziDesignControl1.MinimumSize = new System.Drawing.Size(1007, 690);
            this.preziDesignControl1.Name = "preziDesignControl1";
            this.preziDesignControl1.Size = new System.Drawing.Size(1008, 690);
            this.preziDesignControl1.TabIndex = 0;
            this.preziDesignControl1.ChangeDesignViewMode += new System.Action<JSFW.PREZI.DesignView>(this.preziDesignControl1_ChangeDesignViewMode);
            // 
            // PreziDesignForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1008, 689);
            this.Controls.Add(this.preziDesignControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(1024, 723);
            this.Name = "PreziDesignForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PreziDesignForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.PreziDesignControl preziDesignControl1;
    }
}