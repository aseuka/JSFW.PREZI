using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JSFW.PREZI.Controls;

namespace JSFW.PREZI
{
    public partial class PreziDesignForm : Form, IDesignView
    {
        public DataClass Data { get; set; }

        public Action SaveAction = null;

        //현재 사용하지 않음.
        public bool IsDirty { get; set; }

        DesignView DesignViewMode { get; set; } = DesignView.Design;

        public DesignView DesignView
        {
            get
            {
                return DesignViewMode;
            }
        }

        public bool IsFrameEditing_Control { get; internal set; }

        public bool IsFrameEditing_Form { get; internal set; }

        public PreziDesignForm()
        {
            InitializeComponent();

            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width + 38, Screen.PrimaryScreen.WorkingArea.Height + 16);
            this.FormClosing += PreziDesignForm_FormClosing;
        }

        private void PreziDesignForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            preziDesignControl1.Commit();
            Save();
            SaveAction = null;
            Data = null;
        }

        private void Save()
        {
            if (SaveAction != null) SaveAction();
        }
          
        internal void SetDataClass(DataClass data, Action saveAction )
        {
            Text = data.FullDirectoryName; 

            SaveAction = saveAction;
            Data = data;

            preziDesignControl1.IsFrameEditing_Form = IsFrameEditing_Form;
            preziDesignControl1.IsFrameEditing_Control = IsFrameEditing_Control;
            preziDesignControl1.SetDataClass(Data, SaveAction);
        }

        private void preziDesignControl1_ChangeDesignViewMode(DesignView designview)
        {
            DesignViewMode = designview;
        }

        internal void SetDesignView(bool isViewMode)
        {
            if (isViewMode)
            {
                DesignViewMode = DesignView.View; 
            }
            else
            {
                DesignViewMode = DesignView.Design; 
            }
            preziDesignControl1.SetDesignView(isViewMode);
        }
    }
}
