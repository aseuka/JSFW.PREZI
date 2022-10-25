namespace JSFW.PREZI
{
    partial class Demo_Transparent_PanelForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new JSFW.PREZI.Controls.Label();
            this.transparent_Panel1 = new JSFW.PREZI.Controls.Transparent_Panel();
            this.label2 = new JSFW.PREZI.Controls.Label();
            this.label3 = new JSFW.PREZI.Controls.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 85);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(42, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "드랍 프레임";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // transparent_Panel1
            // 
            this.transparent_Panel1.AllowDrop = true;
            this.transparent_Panel1.BackColor = System.Drawing.Color.Transparent;
            this.transparent_Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.transparent_Panel1.Location = new System.Drawing.Point(67, 12);
            this.transparent_Panel1.Name = "transparent_Panel1";
            this.transparent_Panel1.Size = new System.Drawing.Size(567, 306);
            this.transparent_Panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(148, 321);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "드랍 컨트롤";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label2_MouseDown);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(506, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "버튼을 클릭하려면 투명판넬이 뒤로 가야 함. \r\n";
            // 
            // Demo_Transparent_PanelForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(646, 439);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.transparent_Panel1);
            this.Name = "Demo_Transparent_PanelForm";
            this.Text = "Demo_Transparent_PanelForm";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Demo_Transparent_PanelForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Demo_Transparent_PanelForm_DragEnter);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Transparent_Panel transparent_Panel1;
        private System.Windows.Forms.Button button1;
        private Controls.Label label1;
        private Controls.Label label2;
        private Controls.Label label3;
    }
}