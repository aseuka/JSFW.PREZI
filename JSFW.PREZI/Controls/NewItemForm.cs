using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JSFW.PREZI.Controls
{
    public partial class NewItemForm : Form
    { 
        public bool IsDirty { get; set; }

        public bool HasRaiseViewEvent { get; set; }

        public string NameText { get; protected set; }

        public NewItemForm()
        {
            InitializeComponent();
        }

        public bool IsNew { get; set; } 

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtName.ReadOnly = !IsNew;
            btnOK.SendToBack();

            if (IsNew)
            {
                btnOK.BringToFront();
                txtName.TabIndex = 0;
                btnOK.TabIndex = 1;
                btnView.TabIndex = 2;
            }
            else
            {
                btnView.BringToFront();
                txtName.TabIndex = 0;
                btnView.TabIndex = 1;
                btnOK.TabIndex = 2;
                
                txtName.BackColor = Color.White;
            }
        }

        public void SetNameText(string txt)
        {
            txtName.Text = txt;
            IsDirty = false;
        }
          
        private void btnCancel_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            HasRaiseViewEvent = true;
            DialogResult = DialogResult.OK;
            this.Close(); 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Validation() == false) return;

            NameText = txtName.Text.Trim();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
            btnOK.BringToFront();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Validation())
                {
                    if (IsNew || IsDirty) btnOK.PerformClick();
                    else
                    {
                        btnView.PerformClick();
                    }
                }
            }
            else if (IsNew == false && e.KeyCode == Keys.F2)
            {
                txtName.ReadOnly = false;
                txtName.Focus();
            }
        }


        private bool Validation()
        {
            bool result = true;
            string msg = "";
        
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                msg += "Name Empty";
                result = false;
            }

            if (result == false)
            {
                MessageBox.Show(msg);
            }

            return result;
        }

    }
}
