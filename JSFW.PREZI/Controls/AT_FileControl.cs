using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JSFW.Common.Controls.Common;
using JSFW.PREZI.Editor;
using JSFW.PREZI.Properties;

namespace JSFW.PREZI
{
    public class AT_FileControl : JSFW.PREZI.Controls.Label, IATContent, IBoxInCircle
    {
        public event Action<IATContent, ATEditStateEnum> OpenViewAttributes = null;
        public Control EditControl { get { return Editor; } }
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public PreziDataClass Data { get; protected set; }

        public Editor_AT_FileControl Editor { get; set; }

        JSFW_BoxInCircle _BoxInCircle { get; set; }
  
        PictureBox icon = new PictureBox();

        public JSFW_BoxInCircle BoxInCircle
        {
            get { return _BoxInCircle; }
        }

        public Control HostControl
        {
            get { return this; }
        }

        public AT_FileControl()
        {
            _BoxInCircle = new JSFW_BoxInCircle(this);

            Data = new PreziDataClass();

            this.Text = "Text";
            this.TextAlign = ContentAlignment.MiddleLeft;
            
            BackColor = Color.White;
           // BorderStyle = BorderStyle.FixedSingle;

            Editor = new Editor_AT_FileControl();
            Editor.EditComplited += Editor_EditComplited;

            this.Disposed += AT_TextControl_Disposed;

            this.MouseDoubleClick += AT_TextControl_MouseDoubleClick;

            icon.Image = Resources.newfile;
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(icon);
            icon.Dock = DockStyle.Left;
            icon.Width = 24;
            Padding = new Padding(25, 0, 0, 0);

            this.Height = 26;
            MinimumSize = new System.Drawing.Size(84, 26);
            AllowDrop = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            ControlPaint.DrawBorder(e.Graphics, DisplayRectangle, Color.LightGray, ButtonBorderStyle.Solid);
        }

        public override string ToString()
        {
            return string.Format("TEXT={0}", Data);
        }

        private void AT_TextControl_Disposed(object sender, EventArgs e)
        {
            using (icon) {
                icon.Image = null;
            }
            using (Editor) { }
            Editor = null;
            Data = null;
        }

        private void AT_TextControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Editor == null)
            {
                Editor = new Editor_AT_FileControl();
                Editor.EditComplited += new Action<bool>(Editor_EditComplited);
            }

            Editor.ClearNewFileName();
             
            Editor.SetFileInfo(Data.Name, Data.FilePath);
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
                    if (Data.Name != Editor.FileName || Data.FilePath != Editor.FilePath)
                    {
                        RequestModifyIsDirtyToMainForm();
                    }
                    Text = Data.Name = Editor.FileName;
                    Data.FilePath = Editor.FilePath;

                    SetExtensionOfFileIcon();
                     
                    CalcWidth();
                }
                OnOpenViewAttributes(ATEditStateEnum.End);
            }
        }

        private void SetExtensionOfFileIcon()
        {
            string extension = System.IO.Path.GetExtension(Text);
            icon.Image = null;
            switch (extension.ToLower())
            {
                case ".xls":
                case ".xlsx":
                    icon.Image = Resources.xls;
                    break;

                case ".pptx":
                case ".ppt":
                    icon.Image = Resources.ppt;
                    break;

                case ".pdf":
                    icon.Image = Resources.pdf;
                    break;

                case ".txt":
                    icon.Image = Resources.txt;
                    break;

                case ".zip":
                    icon.Image = Resources.zip;
                    break;

                case ".dll":
                    icon.Image = Resources.dll;
                    break;

                case ".jpg":
                    icon.Image = Resources.jpg;
                    break;
                    
                case ".gif":
                    icon.Image = Resources.gif;
                    break;

                case ".png":
                    icon.Image = Resources.png;
                    break;

                default:
                    icon.Image = Resources.newfile;
                    break; 
            }
        }

        private void OnOpenViewAttributes(ATEditStateEnum state)
        {
            if (OpenViewAttributes != null) OpenViewAttributes(this, state);
        }

        bool isMouseDown = false;
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
            }
            pt = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isMouseDown)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
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
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isMouseDown = false;
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
            data.FilePath = Data.FilePath;
            data.Name = Data.Name;
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
            Text = this.Data.Text = data.Text;

            Data.Name = data.Name;
            Data.FilePath = data.FilePath;
            
            this.Data.Links.Clear();

            foreach (var linkID in data.Links)
            {
                if (Data.Links.Contains(linkID)) continue;
                Data.Links.Add(linkID);
            }
            OpenViewAttributes -= openViewAttributes;
            OpenViewAttributes += openViewAttributes;

            SetExtensionOfFileIcon();
        }
         
        internal void CalcWidth()
        {
            var sz = TextRenderer.MeasureText(Text, Font);
            if (64 < sz.Width)
                this.Width = 24 + sz.Width + 8;
            else
                this.Width = 64;
        }

        public void SetFontStyleAndBorder(int fontSize, bool fontBold, bool fontUnderLine, HorizontalAlignment textAlign, Color foreColor, Color backColor, AT_Border border)
        {
            // 파일명은 스타일 조정 X
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
