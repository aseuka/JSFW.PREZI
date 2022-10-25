using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JSFW.PREZI.Controls.Editor
{
    public partial class Editor_FrameForm : Form
    {  
        public Frame Frame { get; protected set; }

        public event EventHandler Delete = null;

        public string FrameName { get; protected set; }

        public bool OpenType { get; protected set; }

        public Action SaveAction { get; internal set; }

        public Editor_FrameForm()
        {
            InitializeComponent();

            this.Disposed += Editor_FrameForm_Disposed;
        }

        public void SetFrame(Frame frm)
        {
            Frame = frm;
            DataBind();
        }

        private void DataBind()
        {
            try
            {
                IsDataBinding = true;
                DataClear();

                if (Frame != null)
                {
                    if (Frame.Data != null)
                    {
                        btnCreate.SendToBack();
                        txtFrameName.Enabled = true;
                        rdoOpenType_Control.Enabled = true;
                        rdoOpenType_Control.Enabled = true;
                        btnOK.Enabled = false; 
                    }
                    txtFrameName.Text = Frame.Name;
                    rdoOpenType_Control.Checked = Frame.IsOpenForm == Frame.Frame_OpenControl;
                    rdoOpenType_Form.Checked = Frame.IsOpenForm == Frame.Frame_OpenForm;

                    btnDelete.Enabled = true;//  Frame.Data != null;        // 생성안한거 지울 수가 없어서..
                    btnEdit.Enabled = Frame.Data != null;
                }
            }
            finally {
                IsDataBinding = false;
            }
        }

        private void DataClear()
        {
            txtFrameName.Text = "";
            rdoOpenType_Control.Checked = true;
            rdoOpenType_Form.Checked = false;

            btnCreate.BringToFront();
            txtFrameName.Enabled = false;
            rdoOpenType_Control.Enabled = false;
            rdoOpenType_Control.Enabled = false;
            btnOK.Enabled = false;
            btnDelete.Enabled = true;
            btnEdit.Enabled = false;
        }

        private void Editor_FrameForm_Disposed(object sender, EventArgs e)
        {
            Frame = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ("삭제?".Confirm() == DialogResult.Yes)
            {
                if (Delete != null) { Delete(this, e); }
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FrameName = txtFrameName.Text;
            OpenType = rdoOpenType_Form.Checked;

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.Cancel;
            this.Close();
        }


        bool IsDataBinding = false;
        private void rdoOpenType_Control_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;

            btnOK.Enabled = true;
        }

        private void rdoOpenType_Form_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;

            btnOK.Enabled = true;

        }

        private void txtFrameName_TextChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;

            btnOK.Enabled = true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // 생성
            if ("생성?".Confirm() == DialogResult.Yes)
            {
                Frame.Data = new DataClass();
                txtFrameName.Enabled = true;
                rdoOpenType_Control.Enabled = true;
                rdoOpenType_Control.Enabled = true;
                btnOK.Enabled = true;
                btnDelete.Enabled = true;
                btnEdit.Enabled = true;
                txtFrameName.Focus();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // 생성 후 .. 활성화 됨. 

            // 디자인용 에디터에서는 !!
            // 레벨을 1개뿐이 못올라가면?????
            // 무조건 폼으로 수정 팝업. 
            if (Frame != null)
            {
                using (PreziDesignForm pdf = new PreziDesignForm())
                {                    
                    pdf.IsFrameEditing_Form = rdoOpenType_Form.Checked;
                    pdf.IsFrameEditing_Control = rdoOpenType_Control.Checked;
                    pdf.FormClosed += (fs, fe) =>
                    {
                        //this.Show();
                        pdf.SaveAction = null;
                    };

                    pdf.Shown += (fs, fe) =>
                    {
                        //this.Hide();
                        pdf.SetDesignView(this.IsViewMode());
                    };

                    pdf.SetDataClass(Frame.Data, SaveAction);
                    pdf.ShowDialog(this);

                    btnOK.Enabled = true;
                }
            }

        }
    }
}
