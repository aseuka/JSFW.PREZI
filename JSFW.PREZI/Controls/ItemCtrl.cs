using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JSFW.PREZI.Controls
{
    public partial class ItemCtrl : UserControl, IItemControl<ItemCtrl>
    { 
        public DataClass Data { get; protected set; } 

        public ItemCtrl()
        {
            InitializeComponent();

            this.lbText.Click += lbText_Click;
            this.lbText.DoubleClick += lbText_DoubleClick;

            this.Disposed += ItemCtrl_Disposed;
        }

        private void ItemCtrl_Disposed(object sender, EventArgs e)
        {
            Data = null;
        }

        public override string Text
        {
            get
            {
                return lbText.Text;
            }

            set
            {
                lbText.Text = value;
            }
        }

        private void lbText_DoubleClick(object sender, EventArgs e)
        {
            OnDoubleClick(e);
        }

        private void lbText_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        internal void SetDataClass(DataClass data)
        {
            Data = data;
            DataBind();
        }
         
        public bool IsSelected
        {
            get
            {
                return checkBox1.Checked;
            }
        }

        public event EventHandler<ItemsSelectedEventArgs<ItemCtrl>> ItemDbClick;
        public event EventHandler<ItemsSelectedEventArgs<ItemCtrl>> ItemSelected;

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (ItemSelected != null)
                ItemSelected(this, new ItemsSelectedEventArgs<ItemCtrl>(this));
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (ItemDbClick != null)
                ItemDbClick(this, new ItemsSelectedEventArgs<ItemCtrl>(this));
        }

        public void HideCheckBox()
        {
            checkBox1.Checked = false;
            checkBox1.Visible = false;
        }

        public void ShowCheckBox()
        {
            checkBox1.Checked = false;
            checkBox1.Visible = true;
        }
         

        private void DataBind()
        {
            DataClear();
            if (Data != null)
            {
                Text = Data.Name;
            }
        }

        private void DataClear()
        {
            HideCheckBox();
            lbText.Text = "";
        }
    }
}
