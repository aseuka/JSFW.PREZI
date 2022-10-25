using System;
using System.Windows.Forms;

namespace JSFW.PREZI
{
    public partial class LinkItem : UserControl
    { 
        public event Action<LinkItem> Delete = null;

        public LinkData Data { get; protected set; }

        public LinkItem()
        {
            InitializeComponent();

            this.Disposed += LinkItem_Disposed;

            label1.MouseHover += Label1_MouseHover;
            label1.MouseLeave += Label1_Leave;
            btnDelete.MouseHover += BtnDelete_MouseHover;
            btnDelete.MouseLeave += BtnDelete_MouseLeave;
        }

        private void BtnDelete_MouseLeave(object sender, EventArgs e)
        {
            OnLeave(e);
        }

        private void BtnDelete_MouseHover(object sender, EventArgs e)
        {
            OnMouseHover(e);
        }

        private void Label1_Leave(object sender, EventArgs e)
        {
            OnLeave(e);
        }

        private void Label1_MouseHover(object sender, EventArgs e)
        {
            OnMouseHover(e);
        }

        private void LinkItem_Disposed(object sender, EventArgs e)
        {
            Data = null;
        }

        public void SetLinkData(LinkData data)
        {
            Data = data;
            DataBind();
            
        }

        private void DataBind()
        {
            DataClear();
            if (Data != null)
            {
                label1.Text = Data.TargetID;
            }
        }

        private void DataClear()
        {
            label1.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ("삭제?".Confirm() == DialogResult.Yes)
            {
                if (Delete != null) Delete(this);
            }
        }
    }
}
