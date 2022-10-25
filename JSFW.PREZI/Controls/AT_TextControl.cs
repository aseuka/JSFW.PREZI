using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JSFW.Common.Controls.Common;
using JSFW.PREZI.Editor;

namespace JSFW.PREZI
{
    public partial class AT_TextControl : JSFW.PREZI.Controls.Label, IATContent, IBoxInCircle
    {
        public event Action<IATContent, ATEditStateEnum> OpenViewAttributes = null;
        public Control EditControl { get { return Editor; } }
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public PreziDataClass Data { get; protected set; }

        public Editor_AT_TextControl Editor { get; set; }

        JSFW_BoxInCircle _BoxInCircle { get; set; }

        AT_Border AT_BORDER { get; set; } = AT_Border.SOLID;
         
        public JSFW_BoxInCircle BoxInCircle
        {
            get { return _BoxInCircle; }
        }
         
        public Control HostControl
        {
            get { return this; }
        }

        public AT_TextControl()
        {
            InitializeComponent();

            _BoxInCircle = new JSFW_BoxInCircle(this);

            this.Text = "Text";
            this.TextAlign = ContentAlignment.MiddleLeft;

            BackColor = Color.White; 

            Data = new PreziDataClass();            
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

            Data.ForeColor = ColorTranslator.ToHtml( ForeColor );
            Data.BackColor = ColorTranslator.ToHtml( BackColor );
              
            Editor = new Editor_AT_TextControl();
            Editor.EditComplited += Editor_EditComplited;

            this.Disposed += AT_TextControl_Disposed;

            this.MouseDoubleClick += AT_TextControl_MouseDoubleClick;

            this.Height = 21;
            MinimumSize = new System.Drawing.Size(10, 10); 

            AllowDrop = true;
        } 
        
        public override string ToString()
        {
            return string.Format("TEXT={0}", Data);
        }

        private void AT_TextControl_Disposed(object sender, EventArgs e)
        {
            using (Editor) { }
            Editor = null;
            Data = null;
        }

        internal void CalcHeight()
        {
            int h = TextRenderer.MeasureText(this.Text, this.Font).Height + 13;
            if ((21) < h)
            {
                this.Height = h;
            }
            else
            {
                this.Height = 21;
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

        private void AT_TextControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Editor == null)
            {
                Editor = new Editor_AT_TextControl() {  Text = Text };
                Editor.EditComplited += new Action<bool>(Editor_EditComplited);
            }

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
                    else if(isCopyMouseDown)
                    {
                        PreziDataClass copyData = GetmkData().Clone() as PreziDataClass;
                        if (copyData != null)
                        {
                            ContentItemDragDropData dragObject = new ContentItemDragDropData(copyData);
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

        internal void ToggleFontBold()
        {
            float fz = Font.Size;
            if (Font.Bold)
            {
                Font = new Font(Font.FontFamily, fz, Font.Style ^ FontStyle.Bold);
            }
            else
            {
                Font = new Font(Font.FontFamily, fz, Font.Style | FontStyle.Bold);
            }
            Data.FontBold = Font.Bold;
            Data.FontSize = fz;
        }

        internal void ChangeFont(float off)
        {
            float fz = Font.Size;
            if (off < 0)
            { 
                fz = fz - 1;
                if (fz < 8)
                {
                    fz = 8;
                }
                if (Font.Bold)
                {
                    Font = new Font(Font.FontFamily, fz, FontStyle.Bold);
                }
                else
                {
                    Font = new Font(Font.FontFamily, fz, FontStyle.Regular);
                }
            }
            else if( 0 < off)
            { 
                fz = fz + 1;
                if (36 < fz)
                {
                    fz = 36;
                }
                if (Font.Bold)
                {
                    Font = new Font(Font.FontFamily, fz, FontStyle.Bold);
                }
                else
                {
                    Font = new Font(Font.FontFamily, fz, FontStyle.Regular);
                }
            }
            Data.FontBold = Font.Bold;
            Data.FontSize = fz;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isMouseDown = isCopyMouseDown = false;
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
                    if (Data.Links.Any(l => l.TargetID == dropObject.ID)) return;

                    Data.Links.Add(new LinkData() { TargetID = dropObject.ID });
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

            data.Text = Data.Text = Text; 
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
            Text = this.Data.Text = ( "" + data.Text).Trim().Replace("\r", "").Replace("\n", Environment.NewLine);
            this.Data.Links.Clear();
             
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

            foreach (var linkID in data.Links)
            {
                if (Data.Links.Contains(linkID)) continue;
                Data.Links.Add(linkID);
            } 
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
        Color SelectColor { get; set; } = Color.Aquamarine;
        public void Selecting()
        {
            NomalColor = BackColor;
            BackColor = SelectColor;
            IsSelected = true; 
        }

        public void UnSelecting()
        {
            if (IsSelected)
            {
                BackColor = NomalColor;
            }
            NomalColor = BackColor;
            IsSelected = false;
        }
    }

    public class LinkDragDropItem
    {
        public string ID { get; set; }
    }
}
