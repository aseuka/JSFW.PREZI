using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JSFW.PREZI.Editor;
using System.IO;
using System.Runtime.InteropServices;

namespace JSFW.PREZI.Controls
{
    public partial class PreziDesignControl : UserControl, IMessageFilter
    {
        //  JSFW_Mover mv = new JSFW_Mover();

        LinkPanel lnkPanel { get; set; }

        bool IsEditing = false;

        public DataClass Data { get; protected set; }

        public Action SaveAction = null;

        public bool IsFrameEditing_Control { get; internal set; }

        public bool IsFrameEditing_Form { get; internal set; }

        Control SelectedControl { get; set; }

        public PreziDesignControl()
        {
            InitializeComponent();

            DoubleBuffered = true;

            MV_RB mv_rb = new MV_RB();
            // mv.Regist(mv_rb);
            //mv.Regist(new MV_B());
            // mv.SetParent(BackGroundPanel); 

            lnkPanel = new LinkPanel();
            BackGroundPanel.Controls.Add(lnkPanel);
            lnkPanel.SetHostControl(null);
            lnkPanel.ChangedLinkTargetID += LnkPanel_ChangedLinkTargetID;

            this.Disposed += PreziDesignControl_Disposed;

            BackGroundPanel.AutoScrollMinSize = Screen.FromControl(this).Bounds.Size;

            designPanel1.Width = Screen.FromControl(this).Bounds.Width - 10;   // BackGroundPanel.Width;
            designPanel1.Height = Screen.FromControl(this).Bounds.Height - 10; // BackGroundPanel.Height; 

            //designPanel1.HorizontalScroll.Maximum = Screen.FromControl(this).Bounds.Width;
            //designPanel1.VerticalScroll.Maximum = Screen.FromControl(this).Bounds.Height;

            transparent_Panel1.Width = Screen.FromControl(this).Bounds.Width - 10;   // BackGroundPanel.Width;
            transparent_Panel1.Height = Screen.FromControl(this).Bounds.Height - 10; // BackGroundPanel.Height; 

            transparent_Panel1.HorizontalScroll.Maximum = Screen.FromControl(this).Bounds.Width - 10;
            transparent_Panel1.VerticalScroll.Maximum = Screen.FromControl(this).Bounds.Height - 10;
             
            transparent_Panel1.Left = 5;
            transparent_Panel1.Top = 5;

            designPanel1.Left = 5;
            designPanel1.Top = 5;

            designPanel1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            transparent_Panel1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom; 

            Application.AddMessageFilter(this); 
        }

        internal void Commit()
        {
            designPanel1.UnSelecting(); 
            Save();
        }

        public void SetDataClass(DataClass ds, Action saveAction)
        {
            Data = ds;
            SaveAction = saveAction;
            DataBind();
        }

        private void Save()
        {
            PreziDataClass data = designPanel1.GetPreziDataClass();
            if (Data != null && data != null)
            {
                Data.DesignSource = data.Serialize();
                Data.Description = txtDataClassDesc.Text;
                Data.Frames.Clear();

                transparent_Panel1.Frames.ForEach(f =>
                {
                    int x = f.Bounds.X;
                    int y = f.Bounds.Y;
                    bool needChange = false;
                    if (x < 0)
                    {
                        needChange = true;
                        x = 0;
                    }
                    if (y < 0)
                    {
                        needChange = true;
                        y = 0;
                    }

                    if (transparent_Panel1.Width <= f.Bounds.X)
                    {
                        needChange = true;
                        x = transparent_Panel1.Width - f.Bounds.Width;
                    }
                    if (transparent_Panel1.Height <= f.Bounds.Y)
                    {
                        needChange = true;
                        y = transparent_Panel1.Height - f.Bounds.Height;
                    }

                    if (needChange)
                    {
                        f.Bounds = new Rectangle(x, y, f.Bounds.Width, f.Bounds.Height);
                    }
                });
                Data.Frames.AddRange(transparent_Panel1.Frames.ToArray());
            }
            if (SaveAction != null) SaveAction();
        }

        private void DataClear()
        {
            SelectedControl = null;
            //mv.HostControl = null;
            lnkPanel.SetHostControl(null);
            for (int loop = designPanel1.Controls.Count - 1; loop >= 0; loop--)
            {
                using (designPanel1.Controls[loop]) { }
            }
            transparent_Panel1.SaveAction = null;
            transparent_Panel1.Frames.Clear();
        }

        private void DataBind()
        {
            DataClear();

            if (Data != null)
            {
                PreziDataClass contentData = ("" + Data.DesignSource).DeSerialize<PreziDataClass>();
                designPanel1.SetPreziDataClass(contentData, designPanel1_OpenViewAttributes);
                transparent_Panel1.Frames.AddRange(Data.Frames.ToArray());

                transparent_Panel1.Frames.ForEach(f =>
                {
                    int x = f.Bounds.X;
                    int y = f.Bounds.Y;
                    bool needChange = false;
                    if (x < 0)
                    {
                        needChange = true;
                        x = 0;
                    }
                    if (y < 0)
                    {
                        needChange = true;
                        y = 0;
                    }

                    if (transparent_Panel1.Width <= f.Bounds.X)
                    {
                        needChange = true;
                        x = transparent_Panel1.Width - f.Bounds.Width;
                    }
                    if (transparent_Panel1.Height <= f.Bounds.Y)
                    {
                        needChange = true;
                        y = transparent_Panel1.Height - f.Bounds.Height;
                    }

                    if (needChange)
                    {
                        f.Bounds = new Rectangle(x, y, f.Bounds.Width, f.Bounds.Height);
                    }
                });

                txtDataClassDesc.Text = Data.Description;
                btnFadeClose.Visible = Data.IsSubDataClass;
                transparent_Panel1.SaveAction = SaveAction;
            }
            BackGroundPanel.Refresh();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BackGroundPanel.AutoScrollMinSize = Screen.FromControl(this).Bounds.Size;
              
            designPanel1.Width = Screen.FromControl(this).Bounds.Width - 10;   // BackGroundPanel.Width;
            designPanel1.Height = Screen.FromControl(this).Bounds.Height - 10; // BackGroundPanel.Height;
             
            //designPanel1.HorizontalScroll.Maximum = Screen.FromControl(this).Bounds.Width;
            //designPanel1.VerticalScroll.Maximum = Screen.FromControl(this).Bounds.Height;

            transparent_Panel1.Width = Screen.FromControl(this).Bounds.Width - 10;   // BackGroundPanel.Width;
            transparent_Panel1.Height = Screen.FromControl(this).Bounds.Height - 10; // BackGroundPanel.Height; 

            transparent_Panel1.HorizontalScroll.Maximum = Screen.FromControl(this).Bounds.Width - 10;
            transparent_Panel1.VerticalScroll.Maximum = Screen.FromControl(this).Bounds.Height - 10;

            transparent_Panel1.Left = 5;
            transparent_Panel1.Top = 5;

            designPanel1.Left = 5;
            designPanel1.Top = 5;

            designPanel1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            transparent_Panel1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            transparent_Panel1.BringToFront();
            // 작업모드를 컨텐츠 디자인으로 시작
            chkContentView.Checked = true;

            lbCursorSetItem(lbCursor);
            lbSizeSetItem(lbPenSize_3);
            lbBrushSetItem(lbBrush_Black);

            designPanel1.ChangePenMode(false);
            designPanel1.ChangeBrushEraseMode(true);
            designPanel1.ChangePen(designPanel1.CurrentPenWidth, designPanel1.CurrentColour);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            BackGroundPanel.Refresh();
        }

        private void PreziDesignControl_Disposed(object sender, EventArgs e)
        {
            // mv.HostControl = null;
            Application.RemoveMessageFilter(this);
             
            SelectedControl = null;
            using (lnkPanel)
            {
                if (lnkPanel != null)
                    lnkPanel.ChangedLinkTargetID -= LnkPanel_ChangedLinkTargetID;
            }
            lnkPanel = null;

            if (transparent_Panel1 != null && transparent_Panel1.Frames != null) transparent_Panel1.Frames.Clear();

            Data = null;
        }

        private void LnkPanel_ChangedLinkTargetID(string linkID)
        {
            if (designPanel1.CurrentLinkLineID != linkID || linkID == "")
            {
                designPanel1.CurrentLinkLineID = linkID;
                designPanel1.Invalidate();
            }
        }

        CheckBox SelectCheckBox { get; set; }

        private void chkFrameView_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectCheckBox == chkFrameView)
            {
                SelectCheckBox = null;
                chkFrameView.Checked = true;
                return;
            }

            if (chkFrameView.Checked)
            {
                // mv.HostControl = null;
                lnkPanel.SetHostControl(null);
                SelectCheckBox = chkFrameView;
                OnCheckedFrameView();
            }
            BackGroundPanel.Refresh();
        }

        private void OnCheckedFrameView()
        {
            chkContentView.Checked = false;
            transparent_Panel1.BringToFront();
        }

        private void chkContentView_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectCheckBox == chkContentView)
            {
                SelectCheckBox = null;
                chkContentView.Checked = true;
                return;
            }

            if (chkContentView.Checked)
            {
                SelectCheckBox = chkContentView;
                OnCheckedContentView();
            }
            BackGroundPanel.Refresh();
        }

        private void OnCheckedContentView()
        {
            chkFrameView.Checked = false;
            transparent_Panel1.SendToBack();
        }

        #region 라벨 드래그앤 드랍

        bool isMouseDn = false;
        Point pt;
        private void lbToolBoxItem_Text_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDn = e.Button == MouseButtons.Left;
            pt = e.Location;
        }

        private void lbToolBoxItem_Text_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDn)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    ContentItemDragDropData dragObject = new ContentItemDragDropData(new PreziDataClass() { TypeName = typeof(AT_TextControl).FullName, Text = "텍스트", Width = 80, Height = 21 });
                    try
                    {
                        chkContentView.Checked = true;
                        DoDragDrop(dragObject, DragDropEffects.Copy);
                        isMouseDn = false;
                    }
                    finally
                    {
                        dragObject = null;
                    }
                }
            }
        }

        private void lbToolBoxItem_Text_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDn = false;
        }
        #endregion        
        #region 이미지 드래그앤 드랍

        private void lbToolBoxItem_Image_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDn = e.Button == MouseButtons.Left;
            pt = e.Location;
        }

        private void lbToolBoxItem_Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDn)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    ContentItemDragDropData dragObject = new ContentItemDragDropData(new PreziDataClass() { TypeName = typeof(AT_ImageControl).FullName, Text = "이미지", Width = 48, Height = 48 });
                    try
                    {
                        chkContentView.Checked = true;
                        DoDragDrop(dragObject, DragDropEffects.Copy);
                        isMouseDn = false;
                    }
                    finally
                    {
                        dragObject = null;
                    }
                }
            }
        }

        private void lbToolBoxItem_Image_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDn = false;
        }
        #endregion
        #region 파일 드래그앤드랍

        private void lbToolBoxItem_File_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDn = e.Button == MouseButtons.Left;
            pt = e.Location;
        }

        private void lbToolBoxItem_File_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDn)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    ContentItemDragDropData dragObject = new ContentItemDragDropData(new PreziDataClass() { TypeName = typeof(AT_FileControl).FullName, Text = "파일", Width = 60, Height = 21 });
                    try
                    {
                        DoDragDrop(dragObject, DragDropEffects.Copy);
                        isMouseDn = false;
                    }
                    finally
                    {
                        dragObject = null;
                    }
                }
            }
        }

        private void lbToolBoxItem_File_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDn = false;
        }
        #endregion 

        #region 프레임 드래그앤드랍 
        private void lbToolBoxItem_Frame_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDn = e.Button == MouseButtons.Left;
            pt = e.Location;
        }

        private void lbToolBoxItem_Frame_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDn)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    TransparentDragDropEventArgs dropData = new TransparentDragDropEventArgs(true, "");
                    dropData.Text = @"새 프레임";
                    try
                    {
                        chkFrameView.Checked = true;
                        DoDragDrop(dropData, DragDropEffects.Copy);
                    }
                    finally
                    {
                        isMouseDn = false;
                        dropData = null;
                    }
                }
            }
        }

        private void lbToolBoxItem_Frame_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDn = false;
        }
        #endregion

        private void edit_ToolBar1_DataChanged(object sender, ToolbarChangedValueEvent e)
        {
            IATContent cont = SelectedControl as IATContent;
            if (cont != null)
            {
                cont.SetFontStyleAndBorder(e.FontSize, e.FontBold, e.FontUnderLine, e.TextAlign, e.ForeColor, e.BackColor, e.Border);
            }
        }

        private void designPanel1_OpenViewAttributes(IATContent content, ATEditStateEnum state)
        {
            //if (DesignView == DesignView.View)
            //{
            //    mv.HostControl = null;
            //    return;
            //}

            if (state == ATEditStateEnum.End || state == ATEditStateEnum.None)
            {
                bool isCompareResultTrue = false;
                if (SelectedControl != (content as Control))
                {
                    if (SelectedControl != null)
                    {
                        IATContent prevContent = SelectedControl as IATContent;
                        if (prevContent != null)
                        {
                            if (prevContent.EditControl is Editor_AT_ImageControl)
                            {
                                ((Editor_AT_ImageControl)prevContent.EditControl).CleanImages();
                            }
                            BackGroundPanel.Controls.Remove(prevContent.EditControl);
                            IsEditing = false;
                        }
                    }
                }
                else
                {
                    // 더블클릭 > 마우스 다운 이벤트가 발생하여
                    // EditState.Begin > EditState.None 이벤트 생겨 mv조정자가 앞으로 나오는 경우.
                    // 같은지 비교하고, 에디터가 활성화 되어 있으면 mv조정자를 뒤로 숨겨줌
                    if (content != null && content.EditControl != null && content.EditControl.Visible)
                    {
                        isCompareResultTrue = true;
                    }
                }
                SelectedControl = content as Control;

                if (isCompareResultTrue)
                {
                    if (SelectedControl != null)
                    {
                        SelectedControl.SendToBack();
                    }
                }
                if( !IsEditing )
                    lnkPanel.SetHostControl(SelectedControl); edit_ToolBar1.SetContent(SelectedControl as IATContent);
            }
            else if (state == ATEditStateEnum.Begin)
            {
                if (SelectedControl != null)
                {
                    IATContent prevContent = SelectedControl as IATContent;
                    if (prevContent != null)
                    {
                        if (prevContent.EditControl is Editor_AT_ImageControl)
                        {
                            ((Editor_AT_ImageControl)prevContent.EditControl).CleanImages();
                        }
                        BackGroundPanel.Controls.Remove(prevContent.EditControl);
                        IsEditing = false;
                    }
                }

                SelectedControl = content as Control;

                if (content.EditControl != null && SelectedControl != null)
                {
                    if (content.EditControl is Editor_AT_ImageControl)
                    {
                        ((Editor_AT_ImageControl)content.EditControl).RefreshImages();
                    }

                    BackGroundPanel.Controls.Add(content.EditControl);
                    Point pt = designPanel1.PointToScreen(SelectedControl.Location);
                    content.EditControl.Dock = DockStyle.None;
                    content.EditControl.Location = BackGroundPanel.PointToClient(pt);

                    content.EditControl.Visible = true;
                    content.EditControl.BringToFront();
                    content.EditControl.Focus();

                    // 백그라운드 밖으로 나가면 안쪽으로 옮겨준다.  
                    if (BackGroundPanel.Width <= (content.EditControl.Left + content.EditControl.Width + 17 + 5))
                    {
                        content.EditControl.Left = (BackGroundPanel.Width - content.EditControl.Width) - 17 - 5;
                    }

                    if (BackGroundPanel.Height <= (content.EditControl.Top + content.EditControl.Height + 17 + 5))
                    {
                        content.EditControl.Top = (BackGroundPanel.Height - content.EditControl.Height) - 17 - 5;
                    } 
                    IsEditing = true;
                }

                lnkPanel.SetHostControl(SelectedControl); edit_ToolBar1.SetContent(SelectedControl as IATContent);
            }
        }

        bool isClipBoadContentPasting = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ("" + keyData).DebugWarning();

            if (keyData == Keys.Escape)
            {
                if (SelectedControl != null)
                {
                    designPanel1_OpenViewAttributes(SelectedControl as IATContent, ATEditStateEnum.End);
                }

                if (SelectedControl != null)
                {
                    SelectedControl = null; // 판넬 같은게 없으니까...  Parent로 포커스 빼는 로직 삭제.
                }
                lnkPanel.SetHostControl(SelectedControl); edit_ToolBar1.SetContent(SelectedControl as IATContent);
                BackGroundPanel.Invalidate(true);
            }
            else if (keyData == (Keys.Delete | Keys.Control) && !IsEditing) // ( 기본판넬, MV_R, MV_B ) 말고 에디터가 떠있을때는... 수행하지 않음.
            {
                // frame 지우는 것.
                if (designPanel1.Equals(SelectedControl) == false)
                {
                    if (SelectedControl != null)
                    {
                        var ctrl = SelectedControl;
                        ctrl.Parent.Controls.Remove(ctrl);
                        IATContent content = ctrl as IATContent;
                        using (content.EditControl)
                        {
                            if (content.EditControl != null)
                            {
                                BackGroundPanel.Controls.Remove(content.EditControl);
                            }
                        }
                        //if (!(ctrl is MockupItem_DataGrid))
                        //{
                        //    //C1FlexGrid를 지우면 에러가??? 왜 나는걸까? 
                        //    using (ctrl) { }
                        //}
                        SelectedControl = null;
                    }
                    BackGroundPanel.Invalidate(true);
                }
                else if (designPanel1.MultiSelectedControls != null && 0 < designPanel1.MultiSelectedControls.Count) // 여럿을 동시에 삭제.
                {
                    for (int loop = designPanel1.MultiSelectedControls.Count - 1; loop >= 0; loop--)
                    {
                        IATContent content = designPanel1.MultiSelectedControls[loop]; designPanel1.MultiSelectedControls.RemoveAt(loop);
                        var ctrl = content as Control;
                        ctrl.Parent.Controls.Remove(ctrl);
                        content = ctrl as IATContent;
                        using (content.EditControl)
                        {
                            if (content.EditControl != null)
                            {
                                BackGroundPanel.Controls.Remove(content.EditControl);
                            }
                        }
                        SelectedControl = null;
                    }
                    BackGroundPanel.Invalidate(true);
                }
                lnkPanel.SetHostControl(SelectedControl); edit_ToolBar1.SetContent(SelectedControl as IATContent);
            }
            else if (!IsEditing && keyData == (Keys.V | Keys.Control))
            {
                if (isClipBoadContentPasting == false)
                {
                    try
                    {
                        isClipBoadContentPasting = true;
                        Point dOverPoint = designPanel1.PointToClient(MousePosition);
                        // 붙여넣기.
                        if (Clipboard.ContainsText())
                        {
                            // 텍스트.
                            string txt = Clipboard.GetText();
                            // 붙여넣기!  
                            PreziDataClass data = new PreziDataClass() { TypeName = typeof(AT_TextControl).FullName, Text = txt, Width = 80, Height = 21 };
                            AT_TextControl tBox = new AT_TextControl();
                            tBox.SetmkData(data, designPanel1_OpenViewAttributes);
                            tBox.CalcHeight();
                            designPanel1.Controls.Add(tBox);
                            designPanel1.CurrentMovingControl = tBox;
                            designPanel1.SetMagnaticDragOver(dOverPoint);
                            tBox.Location = designPanel1.NearPoint;
                        }
                        else if (Clipboard.ContainsImage())
                        {
                            //// 이미지. 
                            string imageName = "새 이미지";
                            bool isPastCancel = true;
                            string fileName = System.IO.Path.GetFileName(System.IO.Path.GetTempFileName());
                            fileName = fileName.Replace(System.IO.Path.GetExtension(fileName), "") + ".bmp";
                            string tempImage = System.IO.Path.GetTempPath() + fileName;
                            Size sz;

                            Image img = Clipboard.GetImage();
                            {
                                // 붙여넣기!  
                                img.Save(tempImage);
                                sz = img.Size;
                                using (NewImageForm fm = new NewImageForm() { IsPastImage = true, Image = img.Clone() as Image })
                                {
                                    if (fm.ShowDialog() == DialogResult.OK)
                                    {
                                        imageName = fm.ImageName;
                                        isPastCancel = false;
                                    }
                                }
                            }
                            img = null;

                            if (!isPastCancel)
                            {
                                ClipImageManger cm = new ClipImageManger();
                                cm.Load();
                                ClipImage clip = new ClipImage();
                                clip.ImageLocation = tempImage;
                                clip.Name = imageName;
                                clip.Tag = imageName;
                                cm.Add(clip);
                                cm.Save();
                                cm = null;

                                PreziDataClass data = new PreziDataClass() { TypeName = typeof(AT_ImageControl).FullName, Text = imageName, FilePath = clip.ImageLocation, Width = sz.Width, Height = sz.Height };
                                AT_ImageControl imgBox = new AT_ImageControl();
                                imgBox.SetmkData(data, designPanel1_OpenViewAttributes);
                                designPanel1.Controls.Add(imgBox);
                                designPanel1.CurrentMovingControl = imgBox;
                                designPanel1.SetMagnaticDragOver(dOverPoint);
                                imgBox.Location = designPanel1.NearPoint;
                                imgBox.Size = sz;
                                clip = null;
                            }
                        }
                        else if (Clipboard.ContainsFileDropList())
                        {
                            // 드랍했을때!!
                            // 이미지와 파일을 분리해야 한다. 
                            System.Collections.Specialized.StringCollection collection = Clipboard.GetFileDropList();
                            // 이미지 확장자면 위 이미지 처럼 등록.
                            // 다른 파일이면 파일로 등록
                            // 실행파일은 X
                            foreach (string item in collection)
                            {
                                if (string.IsNullOrEmpty(("" + item).Trim())) continue;

                                if (IsCheckIMAGE(item))
                                {
                                    string imageName = "새 이미지";
                                    bool isPastCancel = true;

                                    Size sz;
                                    Image img = Image.FromFile(item);
                                    {
                                        // 붙여넣기!   
                                        sz = img.Size;
                                        using (NewImageForm fm = new NewImageForm() { IsPastImage = true, Image = img.Clone() as Image })
                                        {
                                            if (fm.ShowDialog() == DialogResult.OK)
                                            {
                                                imageName = fm.ImageName;
                                                isPastCancel = false;
                                            }
                                        }
                                    }
                                    img = null;

                                    if (!isPastCancel)
                                    {
                                        ClipImageManger cm = new ClipImageManger();
                                        cm.Load();
                                        ClipImage clip = new ClipImage();
                                        clip.ImageLocation = item;
                                        clip.Name = imageName;
                                        clip.Tag = imageName;
                                        cm.Add(clip);
                                        cm.Save();
                                        cm = null;

                                        PreziDataClass data = new PreziDataClass() { TypeName = typeof(AT_ImageControl).FullName, Text = imageName, FilePath = clip.ImageLocation, Width = sz.Width, Height = sz.Height };
                                        AT_ImageControl imgBox = new AT_ImageControl();
                                        imgBox.SetmkData(data, designPanel1_OpenViewAttributes);
                                        designPanel1.Controls.Add(imgBox);
                                        designPanel1.CurrentMovingControl = imgBox;
                                        designPanel1.SetMagnaticDragOver(dOverPoint);
                                        imgBox.Location = designPanel1.NearPoint;
                                        imgBox.Size = sz;
                                        clip = null;
                                    }
                                }
                                else if (IsExe(item))
                                {

                                }
                                else
                                {
                                    string mngFileName = JSFW_PREZI_CONST.ProjectFilesDirectoryName + "\\" + Guid.NewGuid().ToString("N") + System.IO.Path.GetExtension(item);
                                    // 파일 복사.. C:\JSFW\PREZI\Files\
                                    System.IO.File.Copy(item, mngFileName, true);
                                    PreziDataClass data = new PreziDataClass() { TypeName = typeof(AT_FileControl).FullName, Name = System.IO.Path.GetFileName(item), Text = System.IO.Path.GetFileName(item), FilePath = mngFileName, Width = 120, Height = 21 };
                                    AT_FileControl fileBox = new AT_FileControl();
                                    fileBox.SetmkData(data, designPanel1_OpenViewAttributes);
                                    designPanel1.Controls.Add(fileBox);
                                    designPanel1.CurrentMovingControl = fileBox;
                                    designPanel1.SetMagnaticDragOver(dOverPoint);
                                    fileBox.Location = designPanel1.NearPoint;
                                }
                            }
                        }
                    }
                    finally
                    {
                        isClipBoadContentPasting = false;
                    }
                }
            }
            else
            {
                if (SelectedControl is AT_TextControl)
                {
                    if (keyData == (Keys.Shift | Keys.Control | Keys.Oemcomma))
                    {
                        edit_ToolBar1.ChangeFont(-1m);
                        // 폰트 작게
                        //((AT_TextControl)mv.HostControl).ChangeFont(-1f);
                    }
                    else if (keyData == (Keys.Shift | Keys.Control | Keys.OemPeriod))
                    {
                        edit_ToolBar1.ChangeFont(1m);
                        // 폰트 크게
                        //((AT_TextControl)mv.HostControl).ChangeFont(+1f);
                    }
                    else if (keyData == (Keys.Control | Keys.B))
                    {
                        edit_ToolBar1.ToggleFontBold();
                        // 볼드..
                        //((AT_TextControl)mv.HostControl).ToggleFontBold();
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool IsExe(string item)
        {
            bool isexe = false;

            string ext = System.IO.Path.GetExtension(item);
            if (".EXE;.COM;.BAT;.INI".Split(';').Contains(ext.ToUpper()))
            {
                isexe = true;
            }
            return isexe;
        }

        private bool IsCheckIMAGE(string item)
        {
            bool isImage = false;

            string ext = System.IO.Path.GetExtension(item);
            if (".JPEG;.JPG;.BMP;.PNG;.GIF;.TIFF".Split(';').Contains(ext.ToUpper()))
            {
                isImage = true;
            }
            return isImage;
        }

        private void btnFront_Click(object sender, EventArgs e)
        {
            if (SelectedControl != null && !(SelectedControl is DesignPanel || SelectedControl is Transparent_Panel))
                SelectedControl.BringToFront();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (SelectedControl != null && !(SelectedControl is DesignPanel || SelectedControl is Transparent_Panel))
                SelectedControl.SendToBack();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            SelectedControl = null;
            try
            {
                chkContentView.Checked = true;
                designPanel1.Dock = DockStyle.None;
                Rectangle rect = new Rectangle();
                for (int loop = 0; loop < designPanel1.Controls.Count; loop++)
                {
                    if (rect.Width < designPanel1.Controls[loop].Bounds.Right)
                        rect.Width = designPanel1.Controls[loop].Bounds.Right;

                    if (rect.Height < designPanel1.Controls[loop].Bounds.Bottom)
                        rect.Height = designPanel1.Controls[loop].Bounds.Bottom;
                }
                 
                if (BackGroundPanel.ClientRectangle.Contains(rect) == false)
                {
                    // 이미지 크기
                    if (designPanel1.IsDrawBmp)
                    {
                        rect = Screen.PrimaryScreen.Bounds;
                    }
                    designPanel1.Left = 3;
                    designPanel1.Top = 3;
                    designPanel1.Width = rect.Width + 10;
                    designPanel1.Height = rect.Height + 10;
                }
                else
                {
                    designPanel1.Left = 3;
                    designPanel1.Top = 3;
                    designPanel1.Width = BackGroundPanel.Width;
                    designPanel1.Height = BackGroundPanel.Height;
                }
                designPanel1.ControlShot(-1);
            }
            finally
            {
                designPanel1.Dock = DockStyle.Fill;
            }
        }

        private void txtDataClassDesc_TextChanged(object sender, EventArgs e)
        {
            // 값을... 저장!
            //  BackColor = ControlPaint.ContrastControlDark; 
        }

        public event Action<DesignView> ChangeDesignViewMode = null;

        private void chkDesignRunMode_CheckedChanged(object sender, EventArgs e)
        {
            // 디자인~ 관련 
            if (chkDesignRunMode.Checked) // 시행모드 true
            {
                //pnlContent.Enabled = false; // 컨트롤 판넬 숨기기
                //pnlFrame.Enabled = false;
                chkFrameView.Checked = true;
                // 실행상태
                chkDesignRunMode.BackgroundImage = Properties.Resources.Stop;
                chkDesignRunMode.BackgroundImageLayout = ImageLayout.Center;
                BackColor = Color.FromArgb(25, Color.Ivory);

                if (ChangeDesignViewMode != null) ChangeDesignViewMode(DesignView.View);
            }
            else
            {
                //pnlContent.Enabled = true; // 컨트롤 판넬 보이게
                //pnlFrame.Enabled = true;
                chkContentView.Checked = true;

                chkDesignRunMode.BackgroundImage = Properties.Resources.Play;
                chkDesignRunMode.BackgroundImageLayout = ImageLayout.Center;
                BackColor = Color.WhiteSmoke;

                if (ChangeDesignViewMode != null) ChangeDesignViewMode(DesignView.Design);
            }

            bool mode = this.IsDesignMode();
            pnlFrame.Enabled = mode;
            pnlContent.Enabled = mode;
            edit_ToolBar1.Enabled = mode;
            pnlBringBackShort.Enabled = mode;

            //모드를 바꾸고 배경컨트롤 리프레쉬
            BackGroundPanel.Refresh();
        }

        internal void SetDesignView(bool IsViewMode)
        {
            if (IsViewMode)
            {
                chkDesignRunMode.Checked = true;
            }
            else
            {
                chkDesignRunMode.Checked = false;
            }
        }

        Random rd = new Random();
        private void transparent_Panel1_OpenPreziDesignForm(Frame frm)
        {
            if (frm != null)
            {
                if (frm.IsOpenForm == Frame.Frame_OpenForm)
                {
                    // 디자인 오픈 
                    using (PreziDesignForm pdf = new PreziDesignForm())
                    {
                        pdf.FormClosed += (fs, fe) =>
                            {
                                //this.Show();
                                pdf.SaveAction = null;
                            };

                        pdf.Shown += (fs, fe) =>
                            {
                                //this.Hide();
                                pdf.SetDesignView(this.IsViewMode());
                            };

                        pdf.SetDataClass(frm.Data, SaveAction);
                        pdf.ShowDialog(this);
                    }
                }
                else
                {
                    FindForm().Text = frm.Data.FullDirectoryName;

                    PreziDesignControl ctrl = new PreziDesignControl();
                    ctrl.SetDataClass(frm.Data, SaveAction);
                    ctrl.btnFadeClose.Click += (bs, be) =>
                    {
                        ctrl.Save();
                        ctrl.Visible = false;
                        Parent.Controls.Remove(ctrl);

                        FindForm().Text = Data.FullDirectoryName;
                    };

                    ctrl.Dock = DockStyle.Fill;
                    Parent.Controls.Add(ctrl);
                    ctrl.BringToFront();

                    // 로드이벤트에서 설정이 우선 되므로... 
                    ctrl.SetDesignView(this.IsViewMode());
                    ctrl.ChangeDesignViewMode += (dvMode) =>
                    {
                        if (dvMode == DesignView.View)
                        {
                            chkDesignRunMode.Checked = true;
                        }
                        else
                        {
                            chkDesignRunMode.Checked = false;
                        }
                    };
                }
            }
        }

        private void btnFadeClose_Click(object sender, EventArgs e)
        {
            //if (IsFrameEditing_Control)
            //{
            //    // 프레임 수정중에 창을 띄워 닫기 안됨.
            //    "프레임 수정 중...[컨트롤]".Alert();
            //}
            //else 
            if (IsFrameEditing_Form || IsFrameEditing_Control)
            {
                FindForm().Close();
            }
        }

        Label lbSizeSelectItem = null;
        void lbSizeSetItem(Label lb)
        {
            if (lbSizeSelectItem != null)
            {
                lbSizeSelectItem.BackColor = Color.Ivory;
            }

            lbSizeSelectItem = lb;

            if (lbSizeSelectItem != null)
            {
                lbSizeSelectItem.BackColor = Color.DimGray;
            }
        }

        Label lbCursorSelectItem = null;
        void lbCursorSetItem(Label lb)
        {
            if (lbCursorSelectItem != null)
            {
                lbCursorSelectItem.BackColor = Color.Ivory;
            }

            lbCursorSelectItem = lb;

            if (lbCursorSelectItem != null)
            {
                lbCursorSelectItem.BackColor = Color.DimGray;
            }
        }

        Label lbBrushSelectItem = null;
        void lbBrushSetItem(Label lb)
        {
            if (lbBrushSelectItem != null)
            {
                lbBrushSelectItem.BackColor = Color.Ivory;
            }

            lbBrushSelectItem = lb;

            if (lbBrushSelectItem != null)
            {
                lbBrushSelectItem.BackColor = Color.DimGray;
            }
        }
         
        private void lbCursor_Click(object sender, EventArgs e)
        {
            lbCursorSetItem(sender as Label);
            designPanel1.ChangePenMode(false);
            designPanel1.ChangeBrushEraseMode(false);
            designPanel1.ChangePen(designPanel1.CurrentPenWidth, designPanel1.CurrentColour);
            designPanel1.Invalidate();
        }

        private void lbPen_Click(object sender, EventArgs e)
        {
            lbCursorSetItem(sender as Label);
            designPanel1.ChangePenMode(true);
            designPanel1.ChangeBrushEraseMode(true);
            designPanel1.ChangePen(designPanel1.CurrentPenWidth, designPanel1.CurrentColour);
            designPanel1.Invalidate();
        }

        private void lbErase_Click(object sender, EventArgs e)
        {
            lbCursorSetItem(sender as Label);
            designPanel1.ChangePenMode(true);
            designPanel1.ChangeBrushEraseMode(false);
            designPanel1.ChangePen(24, Color.Transparent);
            designPanel1.Invalidate();
        }

        private void lbPenSize_3_Click(object sender, EventArgs e)
        {
            lbSizeSetItem(sender as Label);
            designPanel1.ChangePenMode(true);
            designPanel1.ChangeBrushEraseMode(true);
            designPanel1.ChangePen(3, designPanel1.CurrentColour);
            lbCursorSetItem(lbPen);
            designPanel1.Invalidate();
        }

        private void lbPenSize_8_Click(object sender, EventArgs e)
        {
            lbSizeSetItem(sender as Label);
            designPanel1.ChangePenMode(true);
            designPanel1.ChangeBrushEraseMode(true);
            designPanel1.ChangePen(8, designPanel1.CurrentColour);
            lbCursorSetItem(lbPen);
            designPanel1.Invalidate();
        }

        private void lbBrush_Black_Click(object sender, EventArgs e)
        {
            lbBrushSetItem(sender as Label);
            designPanel1.ChangePenMode(true);
            designPanel1.ChangeBrushEraseMode(true);
            designPanel1.ChangePen(designPanel1.CurrentPenWidth, Color.Black);
            lbCursorSetItem(lbPen);
            designPanel1.Invalidate();
        }

        private void lbBrush_Red_Click(object sender, EventArgs e)
        {
            lbBrushSetItem(sender as Label);
            designPanel1.ChangePenMode(true);
            designPanel1.ChangeBrushEraseMode(true);
            designPanel1.ChangePen(designPanel1.CurrentPenWidth, Color.Red);
            lbCursorSetItem(lbPen);
            designPanel1.Invalidate();
        }

        private void lbBrush_Blue_Click(object sender, EventArgs e)
        {
            lbBrushSetItem(sender as Label);
            designPanel1.ChangePenMode(true);
            designPanel1.ChangeBrushEraseMode(true);
            designPanel1.ChangePen(designPanel1.CurrentPenWidth, Color.Blue);
            lbCursorSetItem(lbPen);
            designPanel1.Invalidate();
        }

        private void lbBrush_Yellow_Click(object sender, EventArgs e)
        {
            lbBrushSetItem(sender as Label);
            designPanel1.ChangePenMode(true);
            designPanel1.ChangeBrushEraseMode(true);
            designPanel1.ChangePen(designPanel1.CurrentPenWidth, Color.Yellow);
            lbCursorSetItem(lbPen);
            designPanel1.Invalidate();
        }

        private void lbItemDel_Click(object sender, EventArgs e)
        {
            // 삭제.
            SendKeys.SendWait("^{DEL}");
            SendKeys.Flush();
        }

        private void lbBackgroundImg_Click(object sender, EventArgs e)
        {
            // 백그라운드 이미지 설정! 
            // 이미지 경로 : JSFW_PREZI_CONST.ProjectFilesDrawImageDirectoryName + "\\" + Data.ID + "_BACKGROUND.png"
            // 경로에 파일이 있으면 로딩하여 불러들여 보여줌. 
            // 없으면 빈페이지
            // 삭제 < 처리 하면 파일 삭제.
            // 새로 지정하면 << 파일 복사 ( 덮어쓰기 )
            using (BackGroundImage_SettingForm bs = new BackGroundImage_SettingForm())
            {
                bs.TargetDataID = designPanel1.Data.ID;
                if (bs.ShowDialog(this) == DialogResult.OK)
                {
                    if (bs.IsDirty)
                    {
                        //삭제할때
                        if (bs.IsDelete)
                        {
                            if (string.IsNullOrWhiteSpace(bs.TargetDataID) == false
                                    && File.Exists(bs.ImageFilePath))
                            {
                                File.Delete(bs.ImageFilePath);
                            }
                        }
                        // 새로 등록이 되면.
                        if (string.IsNullOrWhiteSpace(bs.TargetDataID) == false
                                && File.Exists(bs.OpenFilePath))
                        {
                            File.Copy(bs.OpenFilePath, bs.ImageFilePath, true);
                        }
                        designPanel1.ReLoadBackGroundImage();
                        designPanel1.Invalidate();
                    }
                }
            }
        }

        private void designPanel1_Paint(object sender, PaintEventArgs e)
        {
            // 스크롤이 되는데도... 스크롤이 안먹어서...
            if (chkContentView.Checked)
            { 
                foreach (var fr in transparent_Panel1.Frames)
                {
                    fr.Draw(e.Graphics, designPanel1.Font, 0, true);
                }
            }
        }

        // 스크롤 이벤트를 받기위해.  https://social.msdn.microsoft.com/Forums/windows/en-US/2501d6c5-8903-4898-adf7-d275029ca51a/mouse-wheel-doesnt-scroll-panel-no-focus?forum=winforms
        private bool mFiltering;
        public bool PreFilterMessage(ref Message m)
        {
            // Force WM_MOUSEWHEEL message to be processed by the panel 
            if (m.Msg == 0x020a && !mFiltering)
            {
                mFiltering = true;
                SendMessage(BackGroundPanel.Handle, m.Msg, m.WParam, m.LParam);
                m.Result = IntPtr.Zero;  // Don't pass it to the parent window 
                mFiltering = false;
                BackGroundPanel.Refresh();
                return true;  // Don't let the focused control see it 
            }
            return false;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

    }
}
