using System;
using System.Windows.Forms;

namespace JSFW.PREZI
{
    public partial class LinkPanel : UserControl
    {
        public event Action<string> ChangedLinkTargetID = null;

        string _LinkTargetID { get; set; }
        /// <summary>
        /// 현재 마우스 오버 되어 있는 link ID
        /// </summary>
        public string LinkTargetID
        {
            get { return _LinkTargetID; }
            internal set
            {
                _LinkTargetID = value;
                if (ChangedLinkTargetID != null)
                    ChangedLinkTargetID(_LinkTargetID);
            }
        }

        public LinkPanel()
        {
            InitializeComponent();
            this.Disposed += LinkPanel_Disposed;
        }

        private void LinkPanel_Disposed(object sender, EventArgs e)
        {
            SetHostControl(null);
        }

        Control MonitorHostControl { get; set; }

        int LinkItemCount
        {
            get
            {
                IATContent content = MonitorHostControl as IATContent;
                if (content != null && content.Data != null && content.Data.Links != null)
                {
                    return content.Data.Links.Count;
                }
                return 0;
            }
        }
   
        public void SetHostControl(Control ctrl)
        {
            if (MonitorHostControl != null)
            {
                DeattachEvent(MonitorHostControl);
            }

            this.Visible = false;
            this.SendToBack();

            LinkTargetID = "";

            this.Width = 0;
            this.Height = 0;

            MonitorHostControl = null;
             
            IATContent content = ctrl as IATContent;
            if (content != null && !(ctrl is DesignPanel))
            {
                MonitorHostControl = ctrl;

                SetLinkItems(content.Data);
                 
                if (MonitorHostControl != null)
                {
                    AttatchEvent(MonitorHostControl);
                }
            }
             
            if (MonitorHostControl == null)
            { 
                this.Visible = false;
                this.SendToBack();
            }
            else
            {
                this.Visible = true;
                this.BringToFront();
            }

            initThumb(); 
        }

        private void SetLinkItems(PreziDataClass data)
        {
            try
            {
                this.SuspendLayout();
                this.Height = 0;
                LinkTargetID = "";
                for (int loop = Controls.Count - 1; loop >= 0; loop--)
                {
                    LinkItem item = Controls[loop] as LinkItem;
                    if( item != null)
                    {
                        item.Delete -= Item_Delete;
                        item.MouseHover -= Item_MouseHover;
                        item.Leave -= Item_Leave;

                        Controls.Remove(item);
                        item.SetLinkData(null);
                        item.Dispose();
                        item = null;
                    }
                }
                Controls.Clear();
                if (data != null)
                {
                    foreach (var lnk in data.Links)
                    {
                        LinkItem item = new LinkItem();
                        item.SetLinkData(lnk);
                        Controls.Add(item);
                        item.Dock = DockStyle.Top;
                        item.BringToFront();
                        this.Height += 18;
                        item.Height = 18;
                        item.Delete += Item_Delete;
                        item.MouseHover += Item_MouseHover;
                        item.Leave += Item_Leave;

                        if (this.Width < item.Width)
                        {
                            this.Width = item.Width;
                        }
                    }
                } 
            }
            finally
            {
           //     this.Height = 18 * Controls.Count + 6;
                this.ResumeLayout(false);
            }
        }

        private void Item_Leave(object sender, EventArgs e)
        {
            LinkTargetID = "";
        }

        private void Item_MouseHover(object sender, EventArgs e)
        {
            LinkItem lnk = sender as LinkItem;
            if (lnk != null)
            {
                LinkTargetID = lnk.Data.TargetID;
            }
            else
            {
                LinkTargetID = "";
            }
        }

        private void Item_Delete(LinkItem obj)
        {
            using (obj)
            {
                this.Height -= 18;
                this.Controls.Remove(obj);

                IATContent content = MonitorHostControl as IATContent;
                if (content != null)
                {
                    content.Data.Links.Remove(obj.Data);
                }
                if (obj.Data.TargetID == LinkTargetID)
                {
                    LinkTargetID = "";
                } 
            }
            ChangedLinkTargetID(LinkTargetID); 
        }

        private void AttatchEvent(Control monitorHostControl)
        {
            monitorHostControl.Move += MonitorHostControl_Move;
            monitorHostControl.Resize += MonitorHostControl_Resize;
            monitorHostControl.MouseDown += MonitorHostControl_MouseDown;
            monitorHostControl.MouseUp += MonitorHostControl_MouseUp; 
        }

        private void MonitorHostControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.Visible = MonitorHostControl != null;
            initThumb();
        }

        private void MonitorHostControl_Resize(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void MonitorHostControl_MouseUp(object sender, MouseEventArgs e)
        {
            this.Visible = MonitorHostControl != null;
            initThumb(); 
        }

        enum POSEnum { L, T, R, B }

        POSEnum em = POSEnum.L;

        private void initThumb()
        {
            if (MonitorHostControl != null)
            {
                // 방향 고르기 4방향.. 
                switch (Choice())
                {
                    default:
                    case POSEnum.B:
                        Left = MonitorHostControl.Left + 4 + Parent.DisplayRectangle.Left;
                        Top = MonitorHostControl.Bottom + 15 + Parent.DisplayRectangle.Top;
                        break;

                    case POSEnum.L: 
                        Left = MonitorHostControl.Left - ( Width + 4) + Parent.DisplayRectangle.Left;
                        Top = MonitorHostControl.Top + Parent.DisplayRectangle.Top;
                        break;
                         
                    case POSEnum.T:
                        Left = MonitorHostControl.Left + 4 + Parent.DisplayRectangle.Left;
                        Top = MonitorHostControl.Top - ( Height + 4) + Parent.DisplayRectangle.Top;
                        break;

                    case POSEnum.R:
                        Left = MonitorHostControl.Right + 15 + Parent.DisplayRectangle.Left;
                        Top = MonitorHostControl.Top + Parent.DisplayRectangle.Top;
                        break; 
                } 
            }

            if (Parent == null) return;

            if (Parent.Width <= (Left + Width + 10 ))
            {
                Left = (Parent.Width - Width) - 10;
            }

            if (Parent.Height <= (Top + Height + 10))
            {
                Top = (Parent.Height - Height) - 10;
            }

            Parent.Update();
        }

        private POSEnum Choice()
        {
            // 부모창이 BackGroundPanel 임.
            POSEnum em = POSEnum.B;
            // 아래쪽에 들어갈수 있나? 
            if (MonitorHostControl == null || LinkItemCount <= 0)
                return em;

            if ((Parent.Width < (MonitorHostControl.Right + Parent.DisplayRectangle.Left)))
            {
                // 왼쪽으로 보내버려. 
                em = POSEnum.L;
            }
            else if ((MonitorHostControl.Left < Parent.DisplayRectangle.Left))      // 높이가 여유가 없고, 왼쪽이 여유가 없다면? 
            {
                em = POSEnum.R;
            }            
            else if (Parent.Height < ((MonitorHostControl.Bottom + Parent.DisplayRectangle.Top )+ this.Height + 17) &&  // 높이가 여유가 없고, 오른쪽 여유가 있다면.
                    ((MonitorHostControl.Right + Parent.DisplayRectangle.Left) < Parent.Width))
            {
                em = POSEnum.T;
            } 
            return em;
        }

        private void MonitorHostControl_Move(object sender, EventArgs e)
        {
           // this.Visible = false;
            if (MonitorHostControl != null)
            {
                switch (Choice())
                {
                    default:
                    case POSEnum.B: 
                        Left = MonitorHostControl.Left + 4 + Parent.DisplayRectangle.Left;
                        Top = MonitorHostControl.Bottom + 15 + Parent.DisplayRectangle.Top;
                        break;

                    case POSEnum.L:
                        Left = MonitorHostControl.Left - (Width + 4) + Parent.DisplayRectangle.Left;
                        Top = MonitorHostControl.Top + Parent.DisplayRectangle.Top;
                        break;

                    case POSEnum.T:
                        Left = MonitorHostControl.Left + 4 + Parent.DisplayRectangle.Left;
                        Top = MonitorHostControl.Top - (Height + 4) + Parent.DisplayRectangle.Top;
                        break;

                    case POSEnum.R:
                        Left = MonitorHostControl.Right + 15 + Parent.DisplayRectangle.Left;
                        Top = MonitorHostControl.Top + Parent.DisplayRectangle.Top;
                        break;
                }

                
            }
        }

        private void DeattachEvent(Control monitorHostControl)
        {
            monitorHostControl.Move -= MonitorHostControl_Move;
            monitorHostControl.Resize -= MonitorHostControl_Resize;
            monitorHostControl.MouseUp -= MonitorHostControl_MouseUp;
            monitorHostControl.MouseDown -= MonitorHostControl_MouseDown;
        }
    }
}
