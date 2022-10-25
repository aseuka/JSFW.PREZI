using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JSFW.PREZI
{
    [System.ComponentModel.ToolboxItem(false)]
    public class JSFW_Mover : IMv
    {
        #region IMv 멤버
        List<Mv> mvlst = new List<Mv>();

        public JSFW_Mover()
        {
        }

        public void Regist(Mv mv)
        {
            mvlst.Add(mv.Regist(this));
        }

        public void ChangeMv(Mv m)
        {
            foreach (Mv item in mvlst)
            {
                item.SetInit();
            }
            if (HostControl == null) return;

            //Control parent = hostControl.Parent;
            //if (parent != null)
            //    parent.Invalidate(System.Drawing.Rectangle.Ceiling(GetRegion(500)));
        }

        Control hostControl = null;
        public System.Windows.Forms.Control HostControl
        {
            get { return hostControl; }
            set
            {
                if (HostControl != null)
                    HostControl.DockChanged -= HostControl_DockChanged;

                if (value == null)
                {
                    ClearMvControl();
                    hostControl = null;
                    return;
                }

                hostControl = value;

                if (hostControl == null)
                {
                    ClearMvControl();
                    return;
                }

                foreach (var item in mvlst)
                {
                    item.IsMoved = false;
                    if (item is MV_T)
                    {
                        item.Visible = !(hostControl.Dock == DockStyle.Top || hostControl.Dock == DockStyle.Left || hostControl.Dock == DockStyle.Right || hostControl.Dock == DockStyle.Fill);
                    }
                    else if (item is MV_L)
                    {
                        item.Visible = !(hostControl.Dock == DockStyle.Left || hostControl.Dock == DockStyle.Bottom || hostControl.Dock == DockStyle.Top || hostControl.Dock == DockStyle.Fill);
                    }
                    else if (item is MV_R)
                    {
                        item.Visible = !(hostControl.Dock == DockStyle.Right || hostControl.Dock == DockStyle.Bottom || hostControl.Dock == DockStyle.Top || hostControl.Dock == DockStyle.Fill);
                    }
                    else if (item is MV_B)
                    {
                        item.Visible = !(hostControl.Dock == DockStyle.Bottom || hostControl.Dock == DockStyle.Left || hostControl.Dock == DockStyle.Right || hostControl.Dock == DockStyle.Fill);
                    }
                    else if (item is MV_M)
                    {
                        item.Visible = hostControl.Dock == DockStyle.None;
                    }
                    else
                        item.Visible = true;

                    if (hostControl.Parent != null)
                    {
                        item.SetInit();
                    }
                    if (item.Visible)
                        item.BringToFront();
                }

                if (HostControl != null)
                    HostControl.DockChanged += HostControl_DockChanged;
            }
        }

        public void SendToBack()
        {
            ClearMvControl();
        }

        void HostControl_DockChanged(object sender, EventArgs e)
        {
            // 다시 셋팅.
            HostControl = hostControl;
        }

        System.Drawing.Pen Bolder = new System.Drawing.Pen(System.Drawing.Color.Orange, 4f);
        protected virtual void BoundPainter(object sender, PaintEventArgs e)
        {
            if (hostControl == null || !mvlst.Any(itm => itm.Visible)) return;
            e.Graphics.DrawRectangle(Bolder, System.Drawing.Rectangle.Ceiling(GetRegion(0.8f)));
        }

        System.Drawing.RectangleF rct = new System.Drawing.RectangleF();
        protected System.Drawing.RectangleF GetRegion(float offset)
        {
            rct.X = -offset / 2f + hostControl.Bounds.X;
            rct.Y = -offset / 2f + hostControl.Bounds.Y;
            rct.Width = -0.3f + hostControl.Bounds.Width;
            rct.Height = -0.3f + hostControl.Bounds.Height;
            rct.Inflate(offset, offset);
            return rct;
        }

        private void ClearMvControl()
        {
            foreach (Mv item in mvlst)
            {
                item.IsMoved = false;
                item.Visible = false;
            }
        }

        public Control Parent { get; protected set; }

        public bool IsParentEquals(Control c)
        {
            if (Parent != null) return Parent.Equals(c);
            return false;
        }

        /// <summary>
        /// 마우스 다운 이벤트 정의.
        /// </summary>
        /// <param name="ctrl"></param>
        public void SetParent(Control ctrl)
        {
            Parent = ctrl;

            if (Parent == null) throw new Exception("Mv 컨테이너 지정이 안되어 있음.[ Parent is null ]");

            foreach (Mv item in mvlst)
            {
                Parent.Controls.Add(item);
                item.Visible = false;
            }
        }

        void ctrl_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.MouseDown -= (Control_MouseDown);
        }

        void ctrl_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.MouseDown += new MouseEventHandler(Control_MouseDown);
        }

        void Control_MouseDown(object sender, MouseEventArgs e)
        {
            HostControl = sender as Control;
        }


        #endregion

        internal void Enable(bool visible)
        {
            if (visible == false)
                ClearMvControl();
        }
    }

    public interface IMv
    {
        Control Parent { get; }
        System.Windows.Forms.Control HostControl { get; set; }
        void ChangeMv(Mv m);
    }
}
