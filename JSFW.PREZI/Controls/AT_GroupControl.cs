using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JSFW.PREZI.Editor;

namespace JSFW.PREZI
{
    public partial class AT_GroupControl : JSFW.PREZI.Controls.Label, IATContent
    {
        public event Action<IATContent, ATEditStateEnum> OpenViewAttributes = null;
        public Control EditControl { get { return Editor; } }
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public PreziDataClass Data { get; protected set; }

        public Editor_AT_TextControl Editor { get; set; }

        public bool IsReadOnly { get; internal set; }

        AT_Border AT_BORDER { get; set; } = AT_Border.SOLID;
        
        internal JSFW.PREZI.Controls.Label Thumb = new JSFW.PREZI.Controls.Label() {
                    Width = 10, Height = 10, MaximumSize = new Size(10, 10), MinimumSize = new Size(10, 10), BackColor = Color.Gray
        }; // 이동자
         
        int thumbLeftOffset = 0;
        int thumbTopOffset = 0;
         
        public AT_GroupControl()
        {
            InitializeComponent();

            Data = new PreziDataClass();

            this.Text = "Text";

            this.TextAlign = ContentAlignment.MiddleCenter;

            BackColor = Color.White;
            //  BorderStyle = BorderStyle.FixedSingle;

            Data.FontSize = Font.Size;
            Data.FontBold = Font.Bold;
            Data.FontUnderLine = Font.Underline;
            switch (TextAlign)
            {
                default:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.TopLeft:
                    Data.FontAlign = HorizontalAlignment.Left;
                    break;

                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    Data.FontAlign = HorizontalAlignment.Center;
                    break;

                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    Data.FontAlign = HorizontalAlignment.Right;
                    break;
            }

            Data.ForeColor = ColorTranslator.ToHtml(ForeColor);
            Data.BackColor = ColorTranslator.ToHtml(BackColor);

            Editor = new Editor_AT_TextControl();
            Editor.EditComplited += Editor_EditComplited;

            this.Disposed += AT_TextControl_Disposed;

            this.MouseDoubleClick += AT_TextControl_MouseDoubleClick;

            this.Height = 21;
            MinimumSize = new System.Drawing.Size(10, 10);
             
         //   Thumb.Visible = false;
            Thumb.MouseDown += Thumb_MouseDown;
            Thumb.MouseMove += Thumb_MouseMove;
            Thumb.MouseUp += Thumb_MouseUp;
            Thumb.Move += Thumb_Move;
            Thumb.Left = this.Right + 20;
            Thumb.Top = this.Top;

            Thumb.DoubleClick += Thumb_DoubleClick;

            CalcThumb();
            Thumb.Visible = IsSelected;
        }

        private void Thumb_DoubleClick(object sender, EventArgs e)
        {
            // 수평인지 // 수직인지... 알아야 하는데?
            if (ModifierKeys == Keys.Control)
            {
                int diff_left = Math.Abs(this.Left - Thumb.Left);
                int diff_top = Math.Abs(this.Bottom - Thumb.Top);

                if (diff_left < diff_top)
                {
                    // 수직동기화..
                    if (diff_left <= 20)
                    {
                        Thumb.Left = this.Left - 5;
                    }
                }
                else
                {
                    // 수평
                    if (diff_top <= 20)
                    {
                        Thumb.Top = this.Bottom - 5;
                    }
                }

            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (AT_BORDER == AT_Border.SOLID)
            {
                ControlPaint.DrawBorder(e.Graphics, DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
            }
            else if (AT_BORDER == AT_Border.DOT)
            {
                ControlPaint.DrawBorder(e.Graphics, DisplayRectangle, Color.LightGray, ButtonBorderStyle.Dotted);
            } 
        }

        private void Thumb_Move(object sender, EventArgs e)
        {
            if (Parent != null)
                Parent.Invalidate();
        }

        bool isThumbMouseDn = false;
        Point thumbPt;
        private void Thumb_MouseUp(object sender, MouseEventArgs e)
        {
            isThumbMouseDn = false;
            thumbPt = e.Location;
            CalcThumb();
        }

        private void Thumb_MouseMove(object sender, MouseEventArgs e)
        {
            if (isThumbMouseDn)
            {
                Thumb.Left += e.Location.X - thumbPt.X;
                Thumb.Top += e.Location.Y - thumbPt.Y;
                CalcThumb();
            }
        }

        private void Thumb_MouseDown(object sender, MouseEventArgs e)
        {
            isThumbMouseDn = e.Button == MouseButtons.Left;
            thumbPt = e.Location;
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e); 
            Thumb.Left = this.Right + (  thumbLeftOffset);
            Thumb.Top = this.Top + ( thumbTopOffset);
        }
         
        internal void CalcThumb()
        {
            thumbLeftOffset =  Thumb.Left - this.Right;
            thumbTopOffset =  Thumb.Top - this.Top;
        }
          
        Pen pen = new Pen(Color.LightGray, 0.5f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
        internal void DrawLine(Graphics g)
        {
            Point origin = new Point(this.Left, this.Bottom);// GetOrigin(this);
            Point thumbOrigin = GetOrigin(Thumb); 
            g.DrawLine(pen, origin, thumbOrigin);
        }

        private Point GetOrigin(Control ctrl)
        {
            Point origin = new Point(); 
            origin.X = ctrl.Left + ctrl.Width / 2;
            origin.Y = ctrl.Top + ctrl.Height / 2;  
            return origin;
        }

        private void AT_TextControl_Disposed(object sender, EventArgs e)
        {
            using (Thumb) { }
            Thumb = null;

            using (Editor) { }
            Editor = null;
            Data = null;
        }

        private void AT_TextControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Editor == null)
            {
                Editor = new Editor_AT_TextControl() { Text = Text };
                Editor.EditComplited += new Action<bool>(Editor_EditComplited);
            }

            if (IsReadOnly) return; // 읽기전용인경우 수정 불가.

            Editor.Text = Text;

            OnOpenViewAttributes(ATEditStateEnum.Begin);
            Editor.BringToFront();
            Editor.Visible = true;
        }

        private void Editor_EditComplited(bool IsComplite)
        {
            if (Editor != null)
            {
                // 적용!
                Editor.SendToBack();
                Editor.Visible = false;
                if (IsComplite)
                {
                    if (Text != Editor.Text)
                    {
                        RequestModifyIsDirtyToMainForm();
                    }

                    Text = Editor.Text;
                }
                OnOpenViewAttributes(ATEditStateEnum.End);
            }
        }

        private void OnOpenViewAttributes(ATEditStateEnum state)
        {
            if (OpenViewAttributes != null) OpenViewAttributes(this, state);
        }

        bool isMouseDown = false;
        bool isCopyMouseDown = false;
        Point pt;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            isMouseDown = false;
            //if (this.IsDesignMode())
            {
                if (ModifierKeys == Keys.Shift && e.Button == MouseButtons.Left)
                {
                    isMouseDown = true;
                }
                else if (ModifierKeys == Keys.Control && e.Button == MouseButtons.Left)
                {
                    isCopyMouseDown = true;
                }
            }
            pt = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isMouseDown || isCopyMouseDown)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    if (isMouseDown)
                    {
                        LinkDragDropItem dragObject = new LinkDragDropItem() { ID = Data.ID };
                        try
                        {
                            DoDragDrop(dragObject, DragDropEffects.Link);
                            isMouseDown = false;
                        }
                        finally
                        {
                            dragObject = null;
                        }
                    }
                    else if (isCopyMouseDown)
                    {
                        PreziDataClass copyData = GetmkData().Clone() as PreziDataClass;
                        if (copyData != null)
                        {
                            ContentItemDragDropData dragObject = new ContentItemDragDropData(copyData) { IsCopyItem = true };
                            try
                            {
                                DoDragDrop(dragObject, DragDropEffects.Copy);
                                isCopyMouseDown = false;
                            }
                            finally
                            {
                                dragObject = null;
                            }
                        }
                        else
                        {
                            isCopyMouseDown = false;
                        }
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isMouseDown = isCopyMouseDown = false;
            CalcThumb();
            OnOpenViewAttributes(ATEditStateEnum.None);
        }

        private void RequestModifyIsDirtyToMainForm()
        {
            //변경여부를 알림. 
            //IDesignView mainForm = FindForm() as IDesignView;
            //if (mainForm != null) mainForm.IsDirty = true;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (e.AllowedEffect == DragDropEffects.Link)
            {
                e.Effect = e.AllowedEffect;
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            if (e.AllowedEffect == DragDropEffects.Link)
            {
                LinkDragDropItem dropObject = e.Data.GetData(typeof(LinkDragDropItem).FullName) as LinkDragDropItem;
                if (dropObject != null && dropObject.ID != this.Data.ID)
                {
                    if (Data.Links.Any( l => l.TargetID == dropObject.ID)) return;

                    Data.Links.Add( new LinkData() { TargetID = dropObject.ID });
                    try
                    {
                        Parent.Invalidate();
                    }
                    catch { }
                }
            }
        }

        public PreziDataClass GetmkData()
        {
            if (this.Data == null) return null;

            PreziDataClass data = this.Data;
            data.ID = this.Data.ID;
            data.TypeName = GetType().FullName;
            data.Left = Left;
            data.Top = Top;
            data.Height = Height;
            data.Width = Width;
            data.Text = Data.Text = Text;

            data.Thumb_Left = Thumb.Left;
            data.Thumb_Top = Thumb.Top;

            data.IsReadOnly = IsReadOnly;

            data.FontSize = Font.Size;
            data.FontBold = Font.Bold;
            data.FontUnderLine = Font.Underline;
            switch (TextAlign)
            {
                default:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.TopLeft:
                    data.FontAlign = HorizontalAlignment.Left;
                    break;

                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    data.FontAlign = HorizontalAlignment.Center;
                    break;

                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    data.FontAlign = HorizontalAlignment.Right;
                    break;
            }

            data.ForeColor = ColorTranslator.ToHtml(ForeColor);
            if (IsSelected)
            {
                data.BackColor = ColorTranslator.ToHtml(NomalColor);
            }
            else
            {
                data.BackColor = ColorTranslator.ToHtml(BackColor);
            }
            data.Border = AT_BORDER;

            //foreach (var linkID in Data.Links)
            //{
            //    if (data.Links.Contains(linkID)) continue;
            //    data.Links.Add(linkID);
            //}
            return data;
        }

        public void SetmkData(PreziDataClass data, Action<IATContent, ATEditStateEnum> openViewAttributes)
        {
            this.Data.ID = data.ID; // ID셋팅.. 자꾸 변한다.
            this.Data.TypeName = GetType().FullName;
            Left = data.Left;
            Top = data.Top;
            Height = data.Height;
            Width = data.Width;
            Text = this.Data.Text = data.Text;

            IsReadOnly = data.IsReadOnly;

            Data.FontSize = data.FontSize;
            Data.FontBold = data.FontBold;
            Data.FontUnderLine = data.FontUnderLine;
            Data.FontAlign = data.FontAlign;
            Data.ForeColor = data.ForeColor;
            Data.BackColor = data.BackColor;
            AT_BORDER = Data.Border = data.Border;

            ForeColor = ColorTranslator.FromHtml(Data.ForeColor);
            BackColor = ColorTranslator.FromHtml(Data.BackColor);

            float fsize = data.FontSize;
            if (data.FontSize < 8) fsize = 9f; // default 값 설정. 

            FontStyle fStyle = FontStyle.Regular;
            if (data.FontBold)
            {
                fStyle |= FontStyle.Bold;
            }
            if (data.FontUnderLine)
            {
                fStyle |= FontStyle.Underline;
            }
            Font = new Font(Font.FontFamily, fsize, fStyle);

            switch (data.FontAlign)
            {
                default:
                case HorizontalAlignment.Left:
                    TextAlign = ContentAlignment.MiddleLeft;
                    break;

                case HorizontalAlignment.Center:
                    TextAlign = ContentAlignment.MiddleCenter;
                    break;

                case HorizontalAlignment.Right:
                    TextAlign = ContentAlignment.MiddleRight;
                    break;
            }
             
            this.Data.Links.Clear();
             
            foreach (var linkID in data.Links)
            {
                if (Data.Links.Contains(linkID)) continue;
                Data.Links.Add(linkID);
            }

            Thumb.Left = data.Thumb_Left;
            Thumb.Top = data.Thumb_Top;

            CalcThumb();

            OpenViewAttributes -= openViewAttributes;
            OpenViewAttributes += openViewAttributes;
        }

        public void SetFontStyleAndBorder(int fontSize, bool fontBold, bool fontUnderLine, HorizontalAlignment textAlign, Color foreColor, Color backColor, AT_Border border)
        {
            Data.FontSize = fontSize;
            Data.FontBold = fontBold;
            Data.FontUnderLine = fontUnderLine;
            Data.FontAlign = textAlign;
            Data.ForeColor = ColorTranslator.ToHtml(foreColor);
            Data.BackColor = ColorTranslator.ToHtml(backColor);

            ForeColor = foreColor;
            if (IsSelected)
            {
                NomalColor = backColor;
            }
            else
            {
                BackColor = backColor;
            }

            AT_BORDER = Data.Border = border;

            float fsize = Data.FontSize;
            if (Data.FontSize < 8) fsize = 9f; // default 값 설정. 

            FontStyle fStyle = FontStyle.Regular;
            if (Data.FontBold)
            {
                fStyle |= FontStyle.Bold;
            }
            if (Data.FontUnderLine)
            {
                fStyle |= FontStyle.Underline;
            }
            Font = new Font(Font.FontFamily, fsize, fStyle);

            switch (Data.FontAlign)
            {
                default:
                case HorizontalAlignment.Left:
                    TextAlign = ContentAlignment.MiddleLeft;
                    break;

                case HorizontalAlignment.Center:
                    TextAlign = ContentAlignment.MiddleCenter;
                    break;

                case HorizontalAlignment.Right:
                    TextAlign = ContentAlignment.MiddleRight;
                    break;
            }

            Invalidate();
        }

        bool IsSelected = false;
        Color NomalColor { get; set; }
        public void Selecting()
        {
            NomalColor = BackColor;
            BackColor = Color.Aquamarine;
            IsSelected = true;

            Thumb.Visible = IsSelected;
        }

        public void UnSelecting()
        {
            if (IsSelected)
            {
                BackColor = NomalColor;
            }
            NomalColor = BackColor;
            IsSelected = false;

            Thumb.Visible = IsSelected;
        }
    }
}
