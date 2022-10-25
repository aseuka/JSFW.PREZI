using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace JSFW.PREZI
{
    [ToolboxItem(false)]
    public partial class Mv : UserControl
    {
        protected IMv Host = null;
        protected MoverPosition lv = MoverPosition.None;

        //true이면 이동 이벤트가 작동함. 
        internal bool IsMoved = false;

        public Mv()
        {
            InitializeComponent();
        }

        public Mv(MoverPosition _lv) : this()
        {
            lv = _lv;
            BorderStyle = BorderStyle.FixedSingle;
            Size = new Size(12, 12);

            // 커서변경
            if (lv == ( MoverPosition.Right | MoverPosition.Bottom))
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (lv == MoverPosition.Right || lv == MoverPosition.Left)
            {
                this.Cursor = Cursors.SizeWE;
            }
            else if (lv == MoverPosition.Top || lv == MoverPosition.Bottom)
            {
                this.Cursor = Cursors.SizeNS;
            }
            else if (lv == MoverPosition.Move)
            {
                this.Cursor = Cursors.Hand;
            }
            else
                this.Cursor = Cursors.Arrow;
        }

        internal Mv Regist(IMv mvHost)
        {
            Host = mvHost;
            return this;
        }

        Pen LinePen = new Pen(Color.Orange, 1.5f);
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rct = ClientRectangle;
            LinePen.SetLineCap(System.Drawing.Drawing2D.LineCap.ArrowAnchor, System.Drawing.Drawing2D.LineCap.ArrowAnchor, System.Drawing.Drawing2D.DashCap.Triangle);
            if (lv == MoverPosition.Top || lv == MoverPosition.Bottom || lv == MoverPosition.Move)
            {
                e.Graphics.DrawLine
                (
                      LinePen
                    , rct.Left + rct.Width / 2
                    , rct.Top
                    , rct.Left + rct.Width / 2
                    , rct.Bottom
                );
            }

            if (lv == MoverPosition.Left || lv == MoverPosition.Right || lv == MoverPosition.Move)
            {
                e.Graphics.DrawLine
                (
                      LinePen
                    , rct.Left
                    , rct.Top + rct.Height / 2
                    , rct.Right
                    , rct.Top + rct.Height / 2
                );
            }

            if (lv == (MoverPosition.Right | MoverPosition.Bottom))
            {
                e.Graphics.DrawLine
               (
                     LinePen
                   , rct.Left
                   , rct.Top 
                   , rct.Right
                   , rct.Bottom
               );
            }
        }
          
         
        #region   객체 이동 구문...
        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool ReleaseCapture();

        readonly int WM_NLBUTTONDOWN = 0xA1;
        readonly int HT_CAPTION = 0x2;

        #endregion

        protected Point pt = new Point();

        /// <summary>
        /// 무버 어디든 클릭후 드래그시 이동하게끔 api로 잡아줌. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            { 
                // 다른 컨트롤에 묶여있을 수 있을 수 있으므로 마우스캡쳐 해제
                ReleaseCapture();
                pt = MousePosition;
                // 타이틀 바의 다운 이벤트처럼 보냄
                SendMessage(this.Handle, WM_NLBUTTONDOWN, HT_CAPTION, 1);
            }
        }

        protected override void OnMove(EventArgs e)
        {
             base.OnMove(e); 
             Host.ChangeMv(this); 
        }

        internal virtual void SetInit()
        {
            if (Host != null && Host.HostControl != null)
            {
                if (Host.HostControl.Dock == DockStyle.Fill)
                {
                    this.Visible = false;
                }
                else if (Host.HostControl.Dock == DockStyle.None)
                {
                    this.Visible = true;
                }
                else if (Host.HostControl.Dock == DockStyle.Left)
                {
                    this.Visible = !(lv == MoverPosition.Left); 
                }
                else if (Host.HostControl.Dock == DockStyle.Top)
                {
                    this.Visible = !(lv == MoverPosition.Top);
                }
                else if (Host.HostControl.Dock == DockStyle.Right)
                {
                    this.Visible = !(lv == MoverPosition.Right) || lv == ( MoverPosition.Right | MoverPosition.Bottom );
                }
                else if (Host.HostControl.Dock == DockStyle.Bottom)
                {
                    this.Visible = !(lv == MoverPosition.Bottom) || lv == (MoverPosition.Right | MoverPosition.Bottom);
                }
            }
        }
    }

    [ToolboxItem(false)]
    public class MV_L : Mv
    {
        public MV_L() : base( MoverPosition.Left )
        {
            Move += new EventHandler(MV_Move);
        }

        void MV_Move(object sender, EventArgs e)
        {
            if (IsMoved)
            { 
                Host.HostControl.Width -= MousePosition.X - pt.X;
                Host.HostControl.Left += MousePosition.X - pt.X;
                pt = MousePosition;
            }
        }

        internal override void SetInit()
        {
            base.SetInit();

            if (Visible == false) return;

            IsMoved = false;
            var xy = Host.HostControl.Parent.PointToScreen(Host.HostControl.Location);
            var xy2 = Host.Parent.PointToClient(xy);
            this.Left = xy2.X - this.Width /2;
            this.Top = xy2.Y + Host.HostControl.Height / 2 - this.Height / 2;
            IsMoved = true;
        }
    }

    [ToolboxItem(false)]
    public class MV_R : Mv
    {
        public MV_R()
            : base(MoverPosition.Right)
        {
            Move += new EventHandler(MV_Move);
        }

        void MV_Move(object sender, EventArgs e)
        { 
            if (IsMoved)
            {
                Host.HostControl.Width += MousePosition.X - pt.X;
                pt = MousePosition;
            }
        }

        internal override void SetInit()
        {
            base.SetInit();

            if (Visible == false) return;

            IsMoved = false;
            var xy = Host.HostControl.Parent.PointToScreen(Host.HostControl.Location);
            var xy2 = Host.Parent.PointToClient(xy);
            this.Left = ( xy2.X + Host.HostControl.Width ) - this.Width / 2;
            this.Top = xy2.Y + Host.HostControl.Height /2 - this.Height / 2;
            IsMoved = true;
        }
    }

    [ToolboxItem(false)]
    public class MV_T : Mv
    {
        public MV_T()
            : base(MoverPosition.Top)
        {
            Move += new EventHandler(MV_Move);
        }

        void MV_Move(object sender, EventArgs e)
        {
            if (IsMoved)
            {
                Host.HostControl.Height -= MousePosition.Y - pt.Y;
                Host.HostControl.Top += MousePosition.Y - pt.Y;
                pt = MousePosition;
            }
        }

        internal override void SetInit()
        {
            base.SetInit();

            if (Visible == false) return;

            IsMoved = false;
            var xy = Host.HostControl.Parent.PointToScreen(Host.HostControl.Location);
            var xy2 = Host.Parent.PointToClient(xy);
            this.Left = xy2.X + Host.HostControl.Width / 2 - this.Width /2;
            this.Top = xy2.Y - this.Height / 2;
            IsMoved = true;
        }
    }

    [ToolboxItem(false)]
    public class MV_B : Mv
    {
        public MV_B()
            : base(MoverPosition.Bottom)
        {
            Move += new EventHandler(MV_Move);
        }

        void MV_Move(object sender, EventArgs e)
        {
            if (IsMoved)
            { 
                // Host.HostControl.Top = this.Top + ( MousePosition.X - pt.X );
                Host.HostControl.Height += MousePosition.Y - pt.Y;
                pt = MousePosition;
            }
        }

        internal override void SetInit()
        {
            base.SetInit();

            if (Visible == false) return;

            IsMoved = false;
            var xy = Host.HostControl.Parent.PointToScreen(Host.HostControl.Location);
            var xy2 = Host.Parent.PointToClient(xy);
            this.Left = xy2.X + Host.HostControl.Width / 2 - this.Width / 2;
            this.Top = (xy2.Y + Host.HostControl.Height) - this.Height / 2;
            IsMoved = true;
        }
    }

    [ToolboxItem(false)]
    public class MV_RB : Mv
    {
        public MV_RB()
            : base(MoverPosition.Right | MoverPosition.Bottom)
        {
            Move += new EventHandler(MV_Move);
        }

        void MV_Move(object sender, EventArgs e)
        {
            if (IsMoved)
            {
                Host.HostControl.Width += MousePosition.X - pt.X;
                Host.HostControl.Height += MousePosition.Y - pt.Y;
                pt = MousePosition;
            }
        }

        internal override void SetInit()
        {
            base.SetInit();

            if (Visible == false) return;

            IsMoved = false;
            var xy = Host.HostControl.Parent.PointToScreen(Host.HostControl.Location);
            var xy2 = Host.Parent.PointToClient(xy);
            
            this.Left = (xy2.X + Host.HostControl.Width) - this.Width / 2;
            this.Top = (xy2.Y + Host.HostControl.Height) - this.Height / 2;
            //this.Top = xy2.Y + Host.HostControl.Height / 2 - this.Height / 2;
            
            IsMoved = true;
        }
    }

    [ToolboxItem(false)]
    public class MV_M : Mv
    {
        public MV_M()
            : base(MoverPosition.Move)
        {
            Move += new EventHandler(MV_Move);
        }

        void MV_Move(object sender, EventArgs e)
        {
            if (IsMoved)
            {
                Host.HostControl.Left += MousePosition.X - pt.X;
                Host.HostControl.Top += MousePosition.Y - pt.Y;
                pt = MousePosition;

            }
        }

        internal override void SetInit()
        {
            if (Host.HostControl.Dock == DockStyle.None)
            {
                this.Visible = true;
            }
            else //if (Host.HostControl.Dock == DockStyle.None)
            {
                this.Visible = false;
            }

            if (Visible == false) return;

            IsMoved = false;
            var xy = Host.HostControl.Parent.PointToScreen(Host.HostControl.Location);
            var xy2 = Host.Parent.PointToClient(xy);
            this.Left = xy2.X + this.Width / 2;
            this.Top = xy2.Y - this.Height / 2;
            IsMoved = true;
        }
    }
     
    /// <summary>
    /// 상하좌우 위치구분자
    /// </summary>
    public enum MoverPosition { None, Left, Top, Right, Bottom, Move };
}
