namespace JSFW.PREZI
{
    partial class Edit_ToolBar
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label9 = new JSFW.PREZI.Controls.Label();
            this.rdoBorder_Dot = new System.Windows.Forms.RadioButton();
            this.rdoBorder_None = new System.Windows.Forms.RadioButton();
            this.rdoBorder_Solid = new System.Windows.Forms.RadioButton();
            this.cpBackColor = new DevAge.Windows.Forms.ColorPicker();
            this.cpForeColor = new DevAge.Windows.Forms.ColorPicker();
            this.label8 = new JSFW.PREZI.Controls.Label();
            this.label7 = new JSFW.PREZI.Controls.Label();
            this.udFontSize = new System.Windows.Forms.NumericUpDown();
            this.label6 = new JSFW.PREZI.Controls.Label();
            this.chkBold = new System.Windows.Forms.CheckBox();
            this.chkUnderLine = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoAlign_Right = new System.Windows.Forms.RadioButton();
            this.rdoAlign_Center = new System.Windows.Forms.RadioButton();
            this.rdoAlign_Left = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.udFontSize)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(599, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "외곽선";
            // 
            // rdoBorder_Dot
            // 
            this.rdoBorder_Dot.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoBorder_Dot.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.rdoBorder_Dot.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCyan;
            this.rdoBorder_Dot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoBorder_Dot.ForeColor = System.Drawing.Color.Gray;
            this.rdoBorder_Dot.Location = new System.Drawing.Point(732, 5);
            this.rdoBorder_Dot.Name = "rdoBorder_Dot";
            this.rdoBorder_Dot.Size = new System.Drawing.Size(43, 23);
            this.rdoBorder_Dot.TabIndex = 17;
            this.rdoBorder_Dot.Text = "점선";
            this.rdoBorder_Dot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoBorder_Dot.UseVisualStyleBackColor = true;
            this.rdoBorder_Dot.CheckedChanged += new System.EventHandler(this.rdoBorder_Dot_CheckedChanged);
            // 
            // rdoBorder_None
            // 
            this.rdoBorder_None.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoBorder_None.Checked = true;
            this.rdoBorder_None.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.rdoBorder_None.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCyan;
            this.rdoBorder_None.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoBorder_None.ForeColor = System.Drawing.Color.Gray;
            this.rdoBorder_None.Location = new System.Drawing.Point(644, 5);
            this.rdoBorder_None.Name = "rdoBorder_None";
            this.rdoBorder_None.Size = new System.Drawing.Size(43, 23);
            this.rdoBorder_None.TabIndex = 18;
            this.rdoBorder_None.TabStop = true;
            this.rdoBorder_None.Text = "없음";
            this.rdoBorder_None.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoBorder_None.UseVisualStyleBackColor = true;
            this.rdoBorder_None.CheckedChanged += new System.EventHandler(this.rdoBorder_None_CheckedChanged);
            // 
            // rdoBorder_Solid
            // 
            this.rdoBorder_Solid.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoBorder_Solid.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.rdoBorder_Solid.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCyan;
            this.rdoBorder_Solid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoBorder_Solid.ForeColor = System.Drawing.Color.Gray;
            this.rdoBorder_Solid.Location = new System.Drawing.Point(688, 5);
            this.rdoBorder_Solid.Name = "rdoBorder_Solid";
            this.rdoBorder_Solid.Size = new System.Drawing.Size(43, 23);
            this.rdoBorder_Solid.TabIndex = 19;
            this.rdoBorder_Solid.Text = "실선";
            this.rdoBorder_Solid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoBorder_Solid.UseVisualStyleBackColor = true;
            this.rdoBorder_Solid.CheckedChanged += new System.EventHandler(this.rdoBorder_Solid_CheckedChanged);
            // 
            // cpBackColor
            // 
            this.cpBackColor.BorderStyle = DevAge.Drawing.BorderStyle.None;
            this.cpBackColor.Location = new System.Drawing.Point(488, 7);
            this.cpBackColor.Name = "cpBackColor";
            this.cpBackColor.SelectedColor = System.Drawing.Color.Black;
            this.cpBackColor.Size = new System.Drawing.Size(102, 20);
            this.cpBackColor.TabIndex = 13;
            this.cpBackColor.TabStop = false;
            this.cpBackColor.SelectedColorChanged += new System.EventHandler(this.cpBackColor_SelectedColorChanged);
            // 
            // cpForeColor
            // 
            this.cpForeColor.BorderStyle = DevAge.Drawing.BorderStyle.None;
            this.cpForeColor.Location = new System.Drawing.Point(328, 7);
            this.cpForeColor.Name = "cpForeColor";
            this.cpForeColor.SelectedColor = System.Drawing.Color.Black;
            this.cpForeColor.Size = new System.Drawing.Size(102, 20);
            this.cpForeColor.TabIndex = 14;
            this.cpForeColor.TabStop = false;
            this.cpForeColor.SelectedColorChanged += new System.EventHandler(this.cpForeColor_SelectedColorChanged);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(434, 5);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(158, 24);
            this.label8.TabIndex = 15;
            this.label8.Text = "배경색";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(274, 5);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(158, 24);
            this.label7.TabIndex = 16;
            this.label7.Text = "글자색";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udFontSize
            // 
            this.udFontSize.ForeColor = System.Drawing.Color.Gray;
            this.udFontSize.Location = new System.Drawing.Point(38, 7);
            this.udFontSize.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.udFontSize.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.udFontSize.Name = "udFontSize";
            this.udFontSize.Size = new System.Drawing.Size(49, 21);
            this.udFontSize.TabIndex = 12;
            this.udFontSize.TabStop = false;
            this.udFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.udFontSize.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.udFontSize.ValueChanged += new System.EventHandler(this.udFontSize_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Gray;
            this.label6.Location = new System.Drawing.Point(7, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "Font";
            // 
            // chkBold
            // 
            this.chkBold.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBold.FlatAppearance.BorderSize = 0;
            this.chkBold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBold.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkBold.ForeColor = System.Drawing.Color.Gray;
            this.chkBold.Location = new System.Drawing.Point(94, 7);
            this.chkBold.Name = "chkBold";
            this.chkBold.Size = new System.Drawing.Size(21, 22);
            this.chkBold.TabIndex = 21;
            this.chkBold.Text = "B";
            this.chkBold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBold.UseVisualStyleBackColor = true;
            this.chkBold.CheckedChanged += new System.EventHandler(this.chkBold_CheckedChanged);
            // 
            // chkUnderLine
            // 
            this.chkUnderLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkUnderLine.FlatAppearance.BorderSize = 0;
            this.chkUnderLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkUnderLine.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkUnderLine.ForeColor = System.Drawing.Color.Gray;
            this.chkUnderLine.Location = new System.Drawing.Point(117, 6);
            this.chkUnderLine.Name = "chkUnderLine";
            this.chkUnderLine.Size = new System.Drawing.Size(21, 24);
            this.chkUnderLine.TabIndex = 21;
            this.chkUnderLine.Text = "U";
            this.chkUnderLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkUnderLine.UseVisualStyleBackColor = true;
            this.chkUnderLine.CheckedChanged += new System.EventHandler(this.chkUnderLine_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoAlign_Right);
            this.panel1.Controls.Add(this.rdoAlign_Center);
            this.panel1.Controls.Add(this.rdoAlign_Left);
            this.panel1.Location = new System.Drawing.Point(144, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(128, 26);
            this.panel1.TabIndex = 22;
            // 
            // rdoAlign_Right
            // 
            this.rdoAlign_Right.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoAlign_Right.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.rdoAlign_Right.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCyan;
            this.rdoAlign_Right.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoAlign_Right.ForeColor = System.Drawing.Color.Gray;
            this.rdoAlign_Right.Location = new System.Drawing.Point(86, 3);
            this.rdoAlign_Right.Name = "rdoAlign_Right";
            this.rdoAlign_Right.Size = new System.Drawing.Size(41, 20);
            this.rdoAlign_Right.TabIndex = 0;
            this.rdoAlign_Right.Text = "≡";
            this.rdoAlign_Right.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdoAlign_Right.UseVisualStyleBackColor = true;
            this.rdoAlign_Right.CheckedChanged += new System.EventHandler(this.rdoAlign_Right_CheckedChanged);
            // 
            // rdoAlign_Center
            // 
            this.rdoAlign_Center.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoAlign_Center.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.rdoAlign_Center.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCyan;
            this.rdoAlign_Center.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoAlign_Center.ForeColor = System.Drawing.Color.Gray;
            this.rdoAlign_Center.Location = new System.Drawing.Point(44, 3);
            this.rdoAlign_Center.Name = "rdoAlign_Center";
            this.rdoAlign_Center.Size = new System.Drawing.Size(41, 20);
            this.rdoAlign_Center.TabIndex = 0;
            this.rdoAlign_Center.Text = "≡";
            this.rdoAlign_Center.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoAlign_Center.UseVisualStyleBackColor = true;
            this.rdoAlign_Center.CheckedChanged += new System.EventHandler(this.rdoAlign_Center_CheckedChanged);
            // 
            // rdoAlign_Left
            // 
            this.rdoAlign_Left.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoAlign_Left.Checked = true;
            this.rdoAlign_Left.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.rdoAlign_Left.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCyan;
            this.rdoAlign_Left.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoAlign_Left.ForeColor = System.Drawing.Color.Gray;
            this.rdoAlign_Left.Location = new System.Drawing.Point(2, 3);
            this.rdoAlign_Left.Name = "rdoAlign_Left";
            this.rdoAlign_Left.Size = new System.Drawing.Size(41, 20);
            this.rdoAlign_Left.TabIndex = 0;
            this.rdoAlign_Left.TabStop = true;
            this.rdoAlign_Left.Text = "≡";
            this.rdoAlign_Left.UseVisualStyleBackColor = true;
            this.rdoAlign_Left.CheckedChanged += new System.EventHandler(this.rdoAlign_Left_CheckedChanged);
            // 
            // Edit_ToolBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkUnderLine);
            this.Controls.Add(this.chkBold);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.rdoBorder_Dot);
            this.Controls.Add(this.rdoBorder_None);
            this.Controls.Add(this.rdoBorder_Solid);
            this.Controls.Add(this.cpBackColor);
            this.Controls.Add(this.cpForeColor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.udFontSize);
            this.Controls.Add(this.label6);
            this.MaximumSize = new System.Drawing.Size(783, 31);
            this.MinimumSize = new System.Drawing.Size(783, 31);
            this.Name = "Edit_ToolBar";
            this.Size = new System.Drawing.Size(783, 31);
            ((System.ComponentModel.ISupportInitialize)(this.udFontSize)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private JSFW.PREZI.Controls.Label label9;
        private System.Windows.Forms.RadioButton rdoBorder_Dot;
        private System.Windows.Forms.RadioButton rdoBorder_None;
        private System.Windows.Forms.RadioButton rdoBorder_Solid;
        private DevAge.Windows.Forms.ColorPicker cpBackColor;
        private DevAge.Windows.Forms.ColorPicker cpForeColor;
        private JSFW.PREZI.Controls.Label label8;
        private JSFW.PREZI.Controls.Label label7;
        private System.Windows.Forms.NumericUpDown udFontSize;
        private JSFW.PREZI.Controls.Label label6;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.CheckBox chkUnderLine;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoAlign_Right;
        private System.Windows.Forms.RadioButton rdoAlign_Center;
        private System.Windows.Forms.RadioButton rdoAlign_Left;
    }
}
