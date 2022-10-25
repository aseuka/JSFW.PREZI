using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JSFW.PREZI.Controls.Editor;

namespace JSFW.PREZI.Controls
{
    public partial class Transparent_Panel : Panel
    {
        public List<Frame> Frames { get; protected set; }

        /// <summary>
        /// 편집기에서 새창띄울때 저장 패스를 가져가기 위해. 
        /// </summary>
        public Action SaveAction = null;

        public event Action<Frame> OpenPreziDesignForm = null;

        public Transparent_Panel()
        {
            InitializeComponent();

            AllowDrop = true;

            Frames = new List<Frame>();
            DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            this.SetStyle(ControlStyles.UserPaint, true); 
            BackColor = Color.FromArgb( 30, Color.White);

            this.Disposed += Transparent_Panel_Disposed;
        }

        private void Transparent_Panel_Disposed(object sender, EventArgs e)
        {
            Frames.Clear();
            Frames = null;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e); 
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            InvalidateEx();  
        }

        internal void InvalidateEx()
        {
            if (Parent == null) return;

            //Parent.Invalidate(this.Bounds, true);

            Parent.Refresh();
        }
         
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //  ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Gray, ButtonBorderStyle.Dotted);
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
             
            foreach (var fr in Frames)
            {
                fr.Draw(e.Graphics, Font, 0);

                if (fr.Equals(currentSelectedFrame))
                {
                    ControlPaint.DrawBorder(
                        e.Graphics,                         
                        new Rectangle( 
                            fr.Bounds.Right - ResizeBoxSz,
                            fr.Bounds.Bottom - ResizeBoxSz,
                            ResizeBoxSz, ResizeBoxSz)
                        , Color.DarkBlue, 
                        ButtonBorderStyle.Dotted);
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            var frIndex = Frames.FindIndex(f => f.Bounds.Contains(e.Location));
            if (0 <= frIndex)
            {
                if (e.Button == MouseButtons.Left)
                {
                    // 상세보기
                    // 상태값에 따라 뷰 또는 에디터가 떠야 한다.
                    if (this.IsDesignMode())
                    {
                        bool isDeleted = false;
                        using (Editor_FrameForm ef = new Editor_FrameForm() )
                        {
                            ef.SaveAction = SaveAction;
                            ef.SetFrame(Frames[frIndex]);
                            ef.Delete += (es, ee) => {
                                isDeleted = true;
                            };
                            if (ef.ShowDialog() == DialogResult.OK)
                            {
                                Frames[frIndex].Name = ef.FrameName;
                                Frames[frIndex].IsOpenForm = ef.OpenType; 
                                Frames[frIndex].Data.Name = ef.FrameName; //? 트리형태의... Full Path를...
                                Frames[frIndex].Data.FullDirectoryName = FindForm().Text + " > "+ ef.FrameName;
                                Frames[frIndex].Data.IsSubDataClass = ef.OpenType == Frame.Frame_OpenControl;
                            }
                        }

                        if (isDeleted)
                        {
                            Frames.RemoveAt(frIndex); // 삭제!
                            Parent.Invalidate(true);
                            this.Invalidate();
                        }
                    }
                    else
                    {  if (Frames[frIndex].Data == null)
                        {
                            return;
                        }
                     
                        if (OpenPreziDesignForm != null)
                            OpenPreziDesignForm(Frames[frIndex]); 
                    }
                }
            }
        }

        private void RequestModifyIsDirtyToMainForm()
        {
            //변경여부를 알림. 
            IDesignView mainForm = FindForm() as IDesignView;
            if (mainForm != null) mainForm.IsDirty = true;
        }
         
        bool isFrameDown = false;
        bool isFrameReSize = false;
        Frame currentSelectedFrame { get; set; }
        Point pt;
        int ResizeBoxSz = 8;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            isFrameReSize = false;
            if (e.Button == MouseButtons.Left && this.IsDesignMode())
            {
                var frIndex = Frames.FindIndex(f => f.Bounds.Contains(e.Location));
                if (0 <= frIndex)
                {
                    isFrameDown = true;
                    pt = e.Location;
                    currentSelectedFrame = Frames[ frIndex ];

                    if (new Rectangle(
                            currentSelectedFrame.Bounds.Right - ResizeBoxSz,
                            currentSelectedFrame.Bounds.Bottom - ResizeBoxSz,
                            ResizeBoxSz, ResizeBoxSz).Contains(e.Location))
                    {
                        isFrameReSize = true;
                        Cursor = Cursors.SizeNWSE;
                        return;
                    }
                    Refresh();
                    Cursor = Cursors.Default;
                }
            }
        } 

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Invalidate(true);
            isFrameDown = false;
            isFrameReSize = false;
            currentSelectedFrame = null;
            Cursor = Cursors.Default;
            base.OnMouseUp(e);
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isFrameDown)
            {
                if (isFrameReSize)
                {
                    // 사이징..
                    int x = (e.X - pt.X);
                    int y = (e.Y - pt.Y);
                    int w = currentSelectedFrame.Bounds.Width + x;
                    int h = currentSelectedFrame.Bounds.Height + y;
                    if (currentSelectedFrame.Bounds.Width + x < 20)
                    {
                        w = 20;
                    }
                    if (currentSelectedFrame.Bounds.Height + y < 20)
                    {
                        h = 20;
                    }

                    currentSelectedFrame.Bounds = new Rectangle(currentSelectedFrame.Bounds.X, currentSelectedFrame.Bounds.Y, w, h);
                    pt = e.Location;
                }
                else
                {
                    // 이동!
                    int x = currentSelectedFrame.Bounds.X + (e.X - pt.X);
                    int y = currentSelectedFrame.Bounds.Y + (e.Y - pt.Y);
                    currentSelectedFrame.Bounds = new Rectangle(x, y, currentSelectedFrame.Bounds.Width, currentSelectedFrame.Bounds.Height);
                    pt = e.Location;   // 다른것과 다르게 있어야 함.                  
                }

                if (Parent != null) Parent.Refresh();
            }

            var frIndex = Frames.FindIndex(f => f.Bounds.Contains(e.Location));
            if (0 <= frIndex)
            { 
                var xFm = Frames[frIndex];
                if (new Rectangle(
                        xFm.Bounds.Right - ResizeBoxSz,
                        xFm.Bounds.Bottom - ResizeBoxSz,
                        ResizeBoxSz, ResizeBoxSz).Contains(e.Location))
                {
                    Cursor = Cursors.SizeNWSE;
                    return;
                }
            }
            Cursor = Cursors.Default;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // add this
                return cp;
            }
        }

        protected override void OnNotifyMessage(System.Windows.Forms.Message m)
        {
            if (m.Msg != 0x0014) // WM_ERASEBKGND == 0X0014
            {
                base.OnNotifyMessage(m);
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
          
            if (e.AllowedEffect == DragDropEffects.Copy)
            {
                e.Effect = e.AllowedEffect; BringToFront();
            }
            else
            {
                e.Effect = DragDropEffects.None; SendToBack();
            }
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            //if (e.Effect == DragDropEffects.Copy)
            //{
            //    //BackColor = Color.FromArgb( 50, Color.DeepSkyBlue);
            //    Invalidate();
            //}
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            if (e.Effect == DragDropEffects.Copy)
            {
                if (e.Data.GetDataPresent(typeof(TransparentDragDropEventArgs).FullName))
                {
                    TransparentDragDropEventArgs dropData = e.Data.GetData(typeof(TransparentDragDropEventArgs).FullName) as TransparentDragDropEventArgs;
                    if (dropData != null)
                    {
                        if (dropData.IsTransparentDoDragStarted)
                        {
                            Point pt = PointToClient(new Point(e.X, e.Y));
                            Frame fr = new Frame() { Bounds = new Rectangle(pt.X, pt.Y, 48, 48), Name = dropData.Text };
                            Frames.Add(fr);
                        }
                        else
                        {
                            if (Parent != null)
                            {
                                var ctrl = Activator.CreateInstance( Type.GetType( dropData.FullTypeName )) as System.Windows.Forms.Control;
                                if (ctrl != null)
                                {
                                    ctrl.Text = dropData.Text;
                                    ctrl.BackColor = Color.Transparent;
                                    ctrl.Location = Parent.PointToClient(new Point(e.X, e.Y));
                                    Parent.Controls.Add(ctrl);
                                    ctrl.Parent = Parent;
                                }
                            }
                        }
                    } 
                } 
            }
            Invalidate(); 
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            Invalidate();
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
            Invalidate(); 
        } 
    }

    public class Frame : ICloneable
    {
        public static readonly bool Frame_OpenForm = true;
        public static readonly bool Frame_OpenControl = false;


        public string ID { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 열리는 타입 지정 ( true: 폼, false: 컨트롤 FadeIn )
        /// </summary>
        public bool IsOpenForm { get; set; } = Frame_OpenForm; 

        public Rectangle Bounds { get; set; }
         
        public Frame()
        {
            ID = Guid.NewGuid().ToString("N");
        } 

        public DataClass Data { get; set; }

        public object Clone()
        {
            Frame nfm = new Frame();
            nfm.Name = Name;
            nfm.Bounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            nfm.IsOpenForm = IsOpenForm;
            nfm.Data = Data?.Clone() as DataClass;
            return nfm;
        }

        internal void Draw(Graphics g, Font fnt, int xyoffset, bool drawText = true)
        {
            //xyoffset : 프레임 그릴때 외부에서 호출해서 판넬에 그려지면 오프셋이 발생함. 
            Rectangle rct = new Rectangle(Bounds.X - xyoffset, Bounds.Y - xyoffset, Bounds.Width, Bounds.Height);

            Color borderColor = Color.Coral;
            if (Data == null)
            {
                borderColor = Color.Gray;
            }

            ControlPaint.DrawBorder(g, rct,
                   borderColor, 2, ButtonBorderStyle.Dashed,
                   borderColor, 2, ButtonBorderStyle.Dashed,
                   borderColor, 2, ButtonBorderStyle.Dashed,
                   borderColor, 2, ButtonBorderStyle.Dashed);

            if (drawText)
            {
                TextRenderer.DrawText(g, Name, fnt, rct.Location, Color.Navy);
            }
        }
    }

    public class TransparentDragDropEventArgs : EventArgs
    {
        public string FullTypeName { get; private set; }

        public bool IsTransparentDoDragStarted { get; private set; }

        public string Text { get; set; }

        public TransparentDragDropEventArgs(bool started, string fullName )
        {
            IsTransparentDoDragStarted = started;
            FullTypeName = fullName;
        }
    }
}
