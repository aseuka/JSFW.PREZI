using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JSFW.Common.Controls.Common;
using JSFW.PREZI.Editor;

namespace JSFW.PREZI
{
    public partial class AT_ImageControl : PictureBox, IATContent, IBoxInCircle
    {
        public event Action<IATContent, ATEditStateEnum> OpenViewAttributes = null;
        public Control EditControl { get { return Editor; } }
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public PreziDataClass Data { get; protected set; }

        public Editor_AT_ImageControl Editor { get; set; }

        JSFW_BoxInCircle _BoxInCircle { get; set; }

        public JSFW_BoxInCircle BoxInCircle
        {
            get { return _BoxInCircle; }
        }

        public Control HostControl
        {
            get { return this; }
        }

        public AT_ImageControl()
        {
            InitializeComponent();

            DoubleBuffered = true;

            _BoxInCircle = new JSFW_BoxInCircle(this);

            Data = new PreziDataClass();

            this.Text = "Text";

            BackColor = Color.Transparent;
           // BorderStyle = BorderStyle.FixedSingle;
            SizeMode = PictureBoxSizeMode.StretchImage;
            BorderStyle = BorderStyle.None;
             
            this.Disposed += AT_ImageControl_Disposed;
            this.MouseDoubleClick += AT_ImageControl_MouseDoubleClick;

            this.MinimumSize = new System.Drawing.Size(24, 24); 
            this.Size = new System.Drawing.Size(48, 48);
             
            AllowDrop = true; 
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (!string.IsNullOrEmpty(("" + ImageLocation).Trim()))
            {
                return;
            }

            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            ControlPaint.DrawBorder(e.Graphics, DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        public override string ToString()
        {
            return string.Format("IMAGE={0}", Data);
        }

        private void AT_ImageControl_Disposed(object sender, EventArgs e)
        {
            using (Editor) { }
            Editor = null;
            Data = null;
        } 

        private void AT_ImageControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Editor == null)
            {
                Editor = new Editor_AT_ImageControl() { ImageFilePath = "", ImageName = "" };
                Editor.EditComplited += new Action<bool>(Editor_EditComplited);
            }

            Editor.ImageName = Data.Name;
            Editor.ImageFilePath = Data.FilePath;

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
                    if (Data.Name != Editor.ImageName || Editor.ImageFilePath != Data.FilePath)
                    {
                        RequestModifyIsDirtyToMainForm();
                    }

                    Data.Name = Editor.ImageName;
                    Data.FilePath = Editor.ImageFilePath;
                    ImageLocation = Editor.ImageFilePath;

                    //if (string.IsNullOrEmpty(("" + ImageLocation).Trim()))
                    //{
                    //    BackColor = Color.White;
                    //}
                    //else
                    //{
                    //    BackColor = Color.Transparent;
                    //}
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
            base.OnMouseDown(e);
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
            data.TypeName = GetType().FullName;
            data.Left = Left;
            data.Top = Top;
            data.Height = Height;
            data.Width = Width;
            data.Text = Text;
            data.Name = this.Data.Name;
            data.FilePath = this.Data.FilePath;
            if (IsSelected)
            {
                data.BackColor = ColorTranslator.ToHtml(NomalColor);
            }
            else
            {
                data.BackColor = ColorTranslator.ToHtml(BackColor);
            }
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
            Text = data.Text;
            if (this.Data == null) this.Data = new PreziDataClass() { Left = data.Left, Top = data.Top, Height = data.Height, Width = data.Width };

            this.Data.Name = data.Name;
            this.Data.FilePath = data.FilePath;
            ImageLocation = this.Data.FilePath;

            //if (string.IsNullOrEmpty(("" + ImageLocation).Trim()))
            //{
            //    BackColor = Color.White;
            //}
            //else
            //{
            //    BackColor = Color.Transparent;
            //}

            this.Data.Links.Clear();
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
            // 구현 X
            if (IsSelected)
            {
                NomalColor = backColor;
            }
            else
            {
                BackColor = backColor;
            }
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
            BackColor = NomalColor;
            IsSelected = false;
        }
    }
}
