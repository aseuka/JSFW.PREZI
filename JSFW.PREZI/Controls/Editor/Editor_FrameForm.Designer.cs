namespace JSFW.PREZI.Controls.Editor
{
    partial class Editor_FrameForm
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
            this.txtFrameName = new System.Windows.Forms.TextBox();
            this.rdoOpenType_Control = new System.Windows.Forms.RadioButton();
            this.rdoOpenType_Form = new System.Windows.Forms.RadioButton();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.label1 = new JSFW.PREZI.Controls.Label();
            this.SuspendLayout();
            // 
            // txtFrameName
            // 
            this.txtFrameName.Location = new System.Drawing.Point(79, 19);
            this.txtFrameName.Name = "txtFrameName";
            this.txtFrameName.Size = new System.Drawing.Size(183, 21);
            this.txtFrameName.TabIndex = 1;
            this.txtFrameName.TextChanged += new System.EventHandler(this.txtFrameName_TextChanged);
            // 
            // rdoOpenType_Control
            // 
            this.rdoOpenType_Control.AutoSize = true;
            this.rdoOpenType_Control.Checked = true;
            this.rdoOpenType_Control.Location = new System.Drawing.Point(199, 48);
            this.rdoOpenType_Control.Name = "rdoOpenType_Control";
            this.rdoOpenType_Control.Size = new System.Drawing.Size(63, 16);
            this.rdoOpenType_Control.TabIndex = 2;
            this.rdoOpenType_Control.TabStop = true;
            this.rdoOpenType_Control.Text = "Control";
            this.rdoOpenType_Control.UseVisualStyleBackColor = true;
            this.rdoOpenType_Control.CheckedChanged += new System.EventHandler(this.rdoOpenType_Control_CheckedChanged);
            // 
            // rdoOpenType_Form
            // 
            this.rdoOpenType_Form.AutoSize = true;
            this.rdoOpenType_Form.Location = new System.Drawing.Point(199, 70);
            this.rdoOpenType_Form.Name = "rdoOpenType_Form";
            this.rdoOpenType_Form.Size = new System.Drawing.Size(52, 16);
            this.rdoOpenType_Form.TabIndex = 2;
            this.rdoOpenType_Form.Text = "From";
            this.rdoOpenType_Form.UseVisualStyleBackColor = true;
            this.rdoOpenType_Form.CheckedChanged += new System.EventHandler(this.rdoOpenType_Form_CheckedChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.Maroon;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.Maroon;
            this.btnDelete.Location = new System.Drawing.Point(12, 77);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.ForestGreen;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnOK.Location = new System.Drawing.Point(283, 19);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 55);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "적용";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.Location = new System.Drawing.Point(283, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.FlatAppearance.BorderColor = System.Drawing.Color.ForestGreen;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnCreate.Location = new System.Drawing.Point(12, 48);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(60, 23);
            this.btnCreate.TabIndex = 3;
            this.btnCreate.Text = "생성";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Location = new System.Drawing.Point(79, 48);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(60, 52);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "수정";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "이름";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Editor_FrameForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(370, 113);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.rdoOpenType_Form);
            this.Controls.Add(this.rdoOpenType_Control);
            this.Controls.Add(this.txtFrameName);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Editor_FrameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "편집] 프레임";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private System.Windows.Forms.TextBox txtFrameName;
        private System.Windows.Forms.RadioButton rdoOpenType_Control;
        private System.Windows.Forms.RadioButton rdoOpenType_Form;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnEdit;
    }
}