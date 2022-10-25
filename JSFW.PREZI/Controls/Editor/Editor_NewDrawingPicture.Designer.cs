namespace JSFW.PREZI.Controls.Editor
{
    partial class Editor_NewDrawingPicture
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbBrush_Yellow = new JSFW.PREZI.Controls.PaintLabel();
            this.lbBrush_Blue = new JSFW.PREZI.Controls.PaintLabel();
            this.lbBrush_Red = new JSFW.PREZI.Controls.PaintLabel();
            this.lbBrush_Black = new JSFW.PREZI.Controls.PaintLabel();
            this.lbPenSize_8 = new JSFW.PREZI.Controls.PaintLabel();
            this.lbPenSize_3 = new JSFW.PREZI.Controls.PaintLabel();
            this.lbErase = new JSFW.PREZI.Controls.Label();
            this.lbPen = new JSFW.PREZI.Controls.Label();
            this.label1 = new JSFW.PREZI.Controls.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(314, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.PaleGreen;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(233, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 29);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "등록";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(72, 34);
            this.textBox2.MaxLength = 100;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(145, 21);
            this.textBox2.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 9);
            this.textBox1.MaxLength = 50;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(145, 21);
            this.textBox1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "이름";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "태그";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(32, 84);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(397, 397);
            this.panel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Controls.Add(this.lbBrush_Yellow);
            this.panel2.Controls.Add(this.lbBrush_Blue);
            this.panel2.Controls.Add(this.lbBrush_Red);
            this.panel2.Controls.Add(this.lbBrush_Black);
            this.panel2.Controls.Add(this.lbPenSize_8);
            this.panel2.Controls.Add(this.lbPenSize_3);
            this.panel2.Controls.Add(this.lbErase);
            this.panel2.Controls.Add(this.lbPen);
            this.panel2.Location = new System.Drawing.Point(3, 84);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(27, 398);
            this.panel2.TabIndex = 12;
            // 
            // lbBrush_Yellow
            // 
            this.lbBrush_Yellow.BackColor = System.Drawing.Color.Ivory;
            this.lbBrush_Yellow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBrush_Yellow.BrushColor = System.Drawing.Color.Yellow;
            this.lbBrush_Yellow.BrushWidth = 16F;
            this.lbBrush_Yellow.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBrush_Yellow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBrush_Yellow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbBrush_Yellow.IsElipse = false;
            this.lbBrush_Yellow.Location = new System.Drawing.Point(0, 189);
            this.lbBrush_Yellow.Name = "lbBrush_Yellow";
            this.lbBrush_Yellow.Size = new System.Drawing.Size(27, 27);
            this.lbBrush_Yellow.TabIndex = 27;
            this.lbBrush_Yellow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBrush_Yellow.Click += new System.EventHandler(this.lbBrush_Yellow_Click);
            // 
            // lbBrush_Blue
            // 
            this.lbBrush_Blue.BackColor = System.Drawing.Color.Ivory;
            this.lbBrush_Blue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBrush_Blue.BrushColor = System.Drawing.Color.Blue;
            this.lbBrush_Blue.BrushWidth = 16F;
            this.lbBrush_Blue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBrush_Blue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBrush_Blue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbBrush_Blue.IsElipse = false;
            this.lbBrush_Blue.Location = new System.Drawing.Point(0, 162);
            this.lbBrush_Blue.Name = "lbBrush_Blue";
            this.lbBrush_Blue.Size = new System.Drawing.Size(27, 27);
            this.lbBrush_Blue.TabIndex = 26;
            this.lbBrush_Blue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBrush_Blue.Click += new System.EventHandler(this.lbBrush_Blue_Click);
            // 
            // lbBrush_Red
            // 
            this.lbBrush_Red.BackColor = System.Drawing.Color.Ivory;
            this.lbBrush_Red.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBrush_Red.BrushColor = System.Drawing.Color.Red;
            this.lbBrush_Red.BrushWidth = 16F;
            this.lbBrush_Red.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBrush_Red.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBrush_Red.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbBrush_Red.IsElipse = false;
            this.lbBrush_Red.Location = new System.Drawing.Point(0, 135);
            this.lbBrush_Red.Name = "lbBrush_Red";
            this.lbBrush_Red.Size = new System.Drawing.Size(27, 27);
            this.lbBrush_Red.TabIndex = 25;
            this.lbBrush_Red.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBrush_Red.Click += new System.EventHandler(this.lbBrush_Red_Click);
            // 
            // lbBrush_Black
            // 
            this.lbBrush_Black.BackColor = System.Drawing.Color.Ivory;
            this.lbBrush_Black.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBrush_Black.BrushColor = System.Drawing.Color.Black;
            this.lbBrush_Black.BrushWidth = 16F;
            this.lbBrush_Black.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBrush_Black.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBrush_Black.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbBrush_Black.IsElipse = false;
            this.lbBrush_Black.Location = new System.Drawing.Point(0, 108);
            this.lbBrush_Black.Name = "lbBrush_Black";
            this.lbBrush_Black.Size = new System.Drawing.Size(27, 27);
            this.lbBrush_Black.TabIndex = 24;
            this.lbBrush_Black.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBrush_Black.Click += new System.EventHandler(this.lbBrush_Black_Click);
            // 
            // lbPenSize_8
            // 
            this.lbPenSize_8.BackColor = System.Drawing.Color.Ivory;
            this.lbPenSize_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPenSize_8.BrushColor = System.Drawing.Color.Black;
            this.lbPenSize_8.BrushWidth = 10F;
            this.lbPenSize_8.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbPenSize_8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbPenSize_8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbPenSize_8.IsElipse = true;
            this.lbPenSize_8.Location = new System.Drawing.Point(0, 81);
            this.lbPenSize_8.Name = "lbPenSize_8";
            this.lbPenSize_8.Size = new System.Drawing.Size(27, 27);
            this.lbPenSize_8.TabIndex = 23;
            this.lbPenSize_8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbPenSize_8.Click += new System.EventHandler(this.lbPenSize_8_Click);
            // 
            // lbPenSize_3
            // 
            this.lbPenSize_3.BackColor = System.Drawing.Color.Ivory;
            this.lbPenSize_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPenSize_3.BrushColor = System.Drawing.Color.Black;
            this.lbPenSize_3.BrushWidth = 6F;
            this.lbPenSize_3.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbPenSize_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbPenSize_3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbPenSize_3.IsElipse = true;
            this.lbPenSize_3.Location = new System.Drawing.Point(0, 54);
            this.lbPenSize_3.Name = "lbPenSize_3";
            this.lbPenSize_3.Size = new System.Drawing.Size(27, 27);
            this.lbPenSize_3.TabIndex = 22;
            this.lbPenSize_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbPenSize_3.Click += new System.EventHandler(this.lbPenSize_3_Click);
            // 
            // lbErase
            // 
            this.lbErase.BackColor = System.Drawing.Color.Ivory;
            this.lbErase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbErase.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbErase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbErase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbErase.Image = global::JSFW.PREZI.Properties.Resources.erase;
            this.lbErase.Location = new System.Drawing.Point(0, 27);
            this.lbErase.Name = "lbErase";
            this.lbErase.Size = new System.Drawing.Size(27, 27);
            this.lbErase.TabIndex = 21;
            this.lbErase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbErase.Click += new System.EventHandler(this.lbErase_Click);
            // 
            // lbPen
            // 
            this.lbPen.BackColor = System.Drawing.Color.Ivory;
            this.lbPen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPen.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbPen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbPen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbPen.Image = global::JSFW.PREZI.Properties.Resources.pen;
            this.lbPen.Location = new System.Drawing.Point(0, 0);
            this.lbPen.Name = "lbPen";
            this.lbPen.Size = new System.Drawing.Size(27, 27);
            this.lbPen.TabIndex = 20;
            this.lbPen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbPen.Click += new System.EventHandler(this.lbPen_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "단순한 아이콘을 그리는 용도로 사용.";
            // 
            // Editor_NewDrawingPicture
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(433, 485);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(449, 524);
            this.Name = "Editor_NewDrawingPicture";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editor_NewDrawingPicture";
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private PaintLabel lbBrush_Yellow;
        private PaintLabel lbBrush_Blue;
        private PaintLabel lbBrush_Red;
        private PaintLabel lbBrush_Black;
        private PaintLabel lbPenSize_8;
        private PaintLabel lbPenSize_3;
        private Label lbErase;
        private Label lbPen;
    }
}