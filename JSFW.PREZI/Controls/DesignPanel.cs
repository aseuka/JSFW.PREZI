using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JSFW.Common.Controls.Common;
using JSFW.Common.Drawing;
using JSFW.PREZI.Controls;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace JSFW.PREZI
{
    public class DesignPanel : PictureBox, IATContent
    {
        public bool IsPenMode = true; // 펜모드( true ) 

        private bool BrushEreaseMode = true;//펜(true)으로 그릴지, 지우개(false)로 사용할지.

        private Bitmap bmp; // Place to store our drawings

        private Graphics grp = null;

        public bool IsDrawBmp { get { return bmp != null; } }
         
        //private List<Point> points = new List<Point>(); // Points of currently drawing line

        Point MouseLoc;
        internal float CurrentPenWidth = 3;
        internal Color CurrentColour = Color.Black;
        internal Pen CurrentPen = new Pen( Color.Black, 3f)  // 현재 펜
        {
            StartCap = System.Drawing.Drawing2D.LineCap.Round,
            EndCap = System.Drawing.Drawing2D.LineCap.Round,
            //DashStyle = System.Drawing.Drawing2D.DashStyle.Solid,
            //MiterLimit = 12,
            //LineJoin = System.Drawing.Drawing2D.LineJoin.MiterClipped,
            //Alignment = System.Drawing.Drawing2D.PenAlignment.Center
        };
        
        private bool isPenMDown = false;

        public event Action<IATContent, ATEditStateEnum> OpenViewAttributes = null;
         
        public Control EditControl { get { return null; } }

        PreziDataClass _Data { get; set; }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public PreziDataClass Data { get { return _Data; } protected set { _Data = value; } }

        bool IsOldMDown = false;
        bool isMDown = false;
        bool isSleep = false;
        Point pt;
        double oldDistance = 10000d;
        internal Point NearPoint;
        Point oldNearPoint;

        /*컨트롤 사이즈 변경*/
        bool IsRS = false;        
        Rectangle RSZ;
        readonly int OFFSET = 8;
        Control CurrentReSizeControl { get; set; }// 컨트롤.MouseMove시 커서변경과 관련된 변수.

        internal Control CurrentMovingControl { get; set; }

        readonly int sMargin = 5;

        readonly int _Width = JSFW_Design_Const.PanlBlockWidth;
        readonly int _Height = JSFW_Design_Const.PanlBlockHeight;
         
        /// <summary>
        /// 아이템의 Link 아이템의 그려진 라인 색상을 반전 해준다. 
        /// </summary>
        public string CurrentLinkLineID { get; set; }
         
        /// <summary>
        /// 여러개 선택시 필요한 저장소
        /// </summary>
        public List<IATContent> MultiSelectedControls { get; private set; }

        #region 마우스 누르고 있으면 동그라미 그려지는 것 차단 (https://stackoverflow.com/questions/18496745/disable-right-click-via-touch-and-hold-on-windows-7-touchscreen-device)
        /*
          UInt16 atom = GlobalAddAtom("MicrosoftTabletPenServiceProperty");
            if (atom != 0) SetProp(panel1.Handle, (UInt32)atom, 1);
        */

        [DllImport("kernel32.dll", EntryPoint = "GlobalAddAtomA", CharSet = CharSet.Ansi)]
        static extern UInt16 GlobalAddAtom(string lpString);

        [DllImport("user32.dll", EntryPoint = "SetPropA", CharSet = CharSet.Ansi)]
        static extern UInt32 SetProp(IntPtr hWnd, UInt32 lpString, UInt32 hData); 
        #endregion

        public DesignPanel()
        {
            Data = new PreziDataClass(); 

            this.Disposed += new EventHandler(GST_Panel_Disposed);
            this.Paint += new PaintEventHandler(GST_Panel_Paint);
            AllowDrop = true;

            this.DragDrop += new DragEventHandler(GST_Panel_DragDrop);
            this.DragEnter += new DragEventHandler(GST_Panel_DragEnter);
            this.DragOver += new DragEventHandler(GST_Panel_DragOver);
            this.DragLeave += new EventHandler(GST_Panel_DragLeave);
            //BorderStyle = BorderStyle.FixedSingle;

           // this.Height = _Height * 4 - 8;

            this.DoubleClick += GST_Panel_DoubleClick;

            MinimumSize = new System.Drawing.Size(10, 10);
            MultiSelectedControls = new List<IATContent>();

            ChangePenMode(true); 

            DoubleBuffered = true;

           // SetStyle(System.Windows.Forms.ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, false);
            //SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);

            //SetStyle(System.Windows.Forms.ControlStyles.UserPaint |
            //         System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
            //         System.Windows.Forms.ControlStyles.SupportsTransparentBackColor |
            //         System.Windows.Forms.ControlStyles.DoubleBuffer, true);
         
            LoadDrawImage();
            ReLoadBackGroundImage();

            BackColor = Color.Transparent; 

            UInt16 atom = GlobalAddAtom("MicrosoftTabletPenServiceProperty");
            if (atom != 0) SetProp(this.Handle, (UInt32)atom, 1);
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x00000020; // add this
        //        return cp;
        //    }
        //}

        private void LoadDrawImage()
        {
            this.Image = null;

            if (bmp != null)
            {
                using (bmp) { using (grp) { } }
                grp = null;
                bmp = null;
            }

            string drawImagePath = JSFW_PREZI_CONST.ProjectFilesDrawImageDirectoryName + "\\" + Data.ID + ".png";
            
            if (System.IO.File.Exists(drawImagePath))
            {
                using (var img = Image.FromFile(drawImagePath, false))
                {
                    bmp = new Bitmap(img); // new Bitmap(this.Width, this.Height); // This is our canvas that will store drawn lines
                                           //using (Graphics g = Graphics.FromImage(bmp))
                                           //    g.Clear(Color.Transparent); // Let's make it white, like paper 
                }
            }
            else
            {
                // 모니터 해상도 크기로 해야 겠네?
                bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); // This is our canvas that will store drawn lines
            }

            this.Image = bmp;
            bmp.MakeTransparent(Color.White);
            grp = Graphics.FromImage(bmp); 
        }
         
        internal void ReLoadBackGroundImage()
        {
            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
                BackgroundImage = null;
            }
             
            string drawImagePath = JSFW_PREZI_CONST.ProjectFilesDrawImageDirectoryName + "\\" + Data.ID + "_BACKGROUND.png";

            if (System.IO.File.Exists(drawImagePath))
            {
                using (var img = Image.FromFile(drawImagePath, false))
                {
                    BackgroundImage = new Bitmap( img );
                }
            }
        }

        private void SaveToBitmap()
        {
            //if (points.Count <= 1)
            //    return;

            //Console.WriteLine($"점 갯수 : {points.Count}");
            //Point _pt = Point.Empty;
            //using (Graphics g = Graphics.FromImage(bmp))
            //{ 
            //    if (BrushEreaseMode)
            //    {
            //        // 펜모드
            //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            //        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            //        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            //    }
            //    else
            //    {
            //        // 지우개 모드 ( 이거 아니면 찌꺼기가 남음. )
            //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            //        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            //        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            //        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            //    }
            //    for (int loop = 0; loop < points.Count; loop++)
            //    {
            //        if (_pt == Point.Empty) _pt = points[loop];

            //        g.DrawLine(CurrentPen, _pt, points[loop]); // 여기서 그려지는 것 때문에....
            //        _pt = points[loop];
            //    }
            //}
            //// 지우개로 이미지에 선을 투명색으로 덧칠한 후에 
            //// 흰색으로 보이는 영역을 투명으로 만들어주어야 함.

            using (Bitmap saveImage = bmp.Clone() as Bitmap)
            {
                if (!BrushEreaseMode)
                {
                    try
                    {
                        this.SuspendLayout();
                        //this.Visible = false;
                        // 그리기 잠시 정지
                        saveImage.MakeTransparent(Color.Transparent);
                        Image = null;
                        using (bmp) { using (grp) { } }

                        Image = bmp = saveImage.Clone() as Bitmap; 
                        grp = Graphics.FromImage(bmp);
                    }
                    finally
                    {
                        //this.Visible = true;
                        this.ResumeLayout(false);
                        this.PerformLayout();
                    }
                }
                string drawImagePath = JSFW_PREZI_CONST.ProjectFilesDrawImageDirectoryName + "\\" + Data.ID + ".png";
                saveImage.Save(drawImagePath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void DesignPanel_Leave(object sender, EventArgs e)
        {
            Cursor.Show();
        }

        private void DesignPanel_Enter(object sender, EventArgs e)
        {
            ChangePenMode(IsPenMode);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Invalidate(true);
        }

        /// <summary>
        /// 이미지를 Overlay 합니다.
        /// </summary>
        /// <param name="b">소스 이미지</param>
        /// <param name="over">위에 overlay할 이미지</param>
        /// <param name="nLeft">겹치기 시작 포인트</param>
        /// <param name="nTop">겹치기 시작 포인트</param>
        /// <returns></returns>
        public Bitmap OverlayImage(Image b, Image over, int nLeft, int nTop)
        {
            Bitmap result = new Bitmap(b.Width, b.Height);
            {
                using (Graphics g = Graphics.FromImage((Image)result))
                {
                    g.DrawImage(b, 0, 0, b.Width, b.Height);
                    g.DrawImage(over, nLeft, nTop, over.Width, over.Height);
                }
            }
            return result;
        }
         
        protected override void OnPaint(PaintEventArgs e)
        {
            if (bmp == null) return;

            base.OnPaint(e);

            if (IsPenMode)
            { 
                // 펜 포인터!! 
                if (!BrushEreaseMode)
                {
                    e.Graphics.DrawEllipse(Pens.DimGray, MouseLoc.X - (CurrentPen.Width / 2) + 1, MouseLoc.Y - (CurrentPen.Width / 2) + 1, CurrentPen.Width - 2, CurrentPen.Width - 2);
                }
            }
        }

        private void GST_Panel_DoubleClick(object sender, EventArgs e)
        {
            // if (this.IsViewMode()) return; 
            //if (Dock != DockStyle.Fill)
            //{
            //    Dock = DockStyle.Fill;
            //    BringToFront();
            //}
            //else
            //    Dock = DockStyle.None;
        }

        bool IsDragDown = false; 
        Rectangle dragBox;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left && !((ModifierKeys & Keys.Shift) == Keys.Shift))
            {
                UnSelecting();
            }

            isPenMDown = false;
            IsDragDown = false;

            if (e.Button == MouseButtons.Left)
            {
                if (IsPenMode)
                {
                    isPenMDown = true;
                    //points.Clear();
                    //points.Add( new Point( e.X, e.Y )); // Remember the first point
                    MouseLoc = e.Location;
                      
                    Invalidate();
                }
                else
                {
                    IsDragDown = true;
                    dragBox.Location = e.Location;
                    dragBox.Height = 0;
                    dragBox.Width = 0;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        { 
            if (IsDragDown)//&& ( e.Button == MouseButtons.Left && (ModifierKeys & Keys.Shift) == Keys.Shift))
            {
                Rectangle rrc = GetBoxRect(dragBox);

                foreach (Control ctrl in Controls)
                { 
                    IATContent cont = ctrl as IATContent;
                    if (cont != null && rrc.Contains(ctrl.Bounds))
                    {
                        if (MultiSelectedControls.Contains(cont) == false)
                        {
                            MultiSelectedControls.Add(cont);
                            cont.Selecting();
                        }
                    }
                }
            }

            isPenMDown = false;
            IsDragDown = false;
            
            SaveToBitmap(); // Save the drawn line to bitmap 
           
            //points.Clear(); // Our drawing is saved, we can clear the list of points 
            base.OnMouseUp(e);
            OnOpenViewAttributes(ATEditStateEnum.None); 
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
           // base.OnMouseMove(e);

            if (IsDragDown)
            {
                //Invalidate();
                dragBox.Width = e.Location.X - dragBox.Location.X;
                dragBox.Height = e.Location.Y - dragBox.Location.Y;
               
                Invalidate();
            }
            else if (IsPenMode && isPenMDown)
            {
                //PAINTING 
                if (IsPenMode)
                {
                    // 펜 포인터!! 
                    if (!BrushEreaseMode)
                    {
                        // 지우개 모드 ( 이거 아니면 찌꺼기가 남음. )
                        grp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        grp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                        grp.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                        grp.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                        grp.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy; 
                    }
                    else
                    {
                        // 펜모드
                        grp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                        grp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                        grp.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        grp.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default; 
                        grp.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    }
                    grp.DrawLine(CurrentPen, MouseLoc, e.Location);
                    MouseLoc = e.Location;
                    int offset = (int)(CurrentPen.Width / 2);
                    grp.FillEllipse(new SolidBrush(CurrentPen.Color), Rectangle.FromLTRB(e.Location.X - offset, e.Location.Y - offset, e.Location.X + offset, e.Location.Y + offset));
                }
                //points.Add(new Point(e.X, e.Y)); // Add points to path 
                //refresh the panel so it will be forced to re-draw. 
                Invalidate(); 
            }

            if (!IsRS)
            {
                Cursor = Cursors.Default;
            }
        }
         
        private void OnOpenViewAttributes(ATEditStateEnum state)
        {
            if (OpenViewAttributes != null) OpenViewAttributes(this, state);
        }

        void GST_Panel_DragLeave(object sender, EventArgs e)
        {
            control_MouseUp(null, null);
        }

        void GST_Panel_DragOver(object sender, DragEventArgs e)
        { 
            if (e.AllowedEffect == DragDropEffects.Copy && e.Data.GetDataPresent(typeof(ContentItemDragDropData).FullName))
            {
                CurrentMovingControl = sender as Control;
                isMDown = IsOldMDown = true;
                oldDistance = 10000d;
                isSleep = false;
                pt = MousePosition;
                Point dOverPoint = PointToClient(new Point(e.X, e.Y));
                SetMagnaticDragOver( dOverPoint );
            }
            else if (e.AllowedEffect == (DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move))
            {
                /*
                    [5]	"FileDrop"	string
		            [6]	"FileNameW"	string
		            [7]	"FileName"	string
		            [8]	"FileContents"	string 
                */
                if (e.Data.GetDataPresent("FileDrop") || e.Data.GetDataPresent("FileNameW") ||
                    e.Data.GetDataPresent("FileName") || e.Data.GetDataPresent("FileContents"))
                {
                   
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    control_MouseUp(null, null);
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
                control_MouseUp( null, null );
            }
            this.Invalidate();
        }

        internal void SetMagnaticDragOver(Point dOverPoint)
        {
            if (CurrentMovingControl == null) return;

            int r = dOverPoint.Y / _Height;
            int c = dOverPoint.X / _Width;
            if (0 < dOverPoint.X % _Width)
            {
                c++;
            }
            int cnt = (Width / _Width);

            // 가까운 점 구하기 
            int xx = c * _Width;
            if ((dOverPoint.X % _Width) != 0 && (dOverPoint.X % _Width) < (_Width / 2))
            {
                xx = (c - 1) * _Width;
            }
            int yy = yy = r * _Height;
            if (0 < (dOverPoint.Y % _Height) / (_Height / 2))
            {
                yy = (r + 1) * _Height;
            }
            oldNearPoint.X = NearPoint.X;
            oldNearPoint.Y = NearPoint.Y;

            NearPoint = new Point(sMargin + xx, sMargin + yy);

            double dx = Math.Abs(dOverPoint.X - NearPoint.X);
            double dy = Math.Abs(dOverPoint.Y - NearPoint.Y);
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (Math.Abs(distance / 2) > 2)
            {
                int arrow = 0;
                if (oldDistance > distance)
                {
                    arrow = -1;
                }
                else if (oldDistance < distance)
                {
                    arrow = 1;
                }

                if (arrow < 0 && distance <= 8)
                {
                    dOverPoint.X = NearPoint.X;
                    dOverPoint.Y = NearPoint.Y;
                    oldDistance = distance = 0;
                    IsOldMDown = isMDown;
                    isMDown = false;
                    isSleep = true;
                    Action sleep = new Action(DragSleep);
                    sleep.BeginInvoke(ir => sleep.EndInvoke(ir), sleep);
                }
                else
                {
                    oldDistance = distance;
                }
                //label1.Text = "" + (r * cnt + c) + ", " + (int)distance + " : " + arrow;
                //label1.Update(); 
            }
        }

        void GST_Panel_DragEnter(object sender, DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("?:" + e.AllowedEffect);

            if (e.AllowedEffect == DragDropEffects.Copy || e.Data.GetDataPresent(typeof(ContentItemDragDropData).FullName))
            {
                e.Effect = e.AllowedEffect;
            }
            else if (e.AllowedEffect == (DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move))
            {
                /*
        [5]	"FileDrop"	string
		[6]	"FileNameW"	string
		[7]	"FileName"	string
		[8]	"FileContents"	string 
             */
                if (e.Data.GetDataPresent("FileDrop") || e.Data.GetDataPresent("FileNameW") ||
                    e.Data.GetDataPresent("FileName") || e.Data.GetDataPresent("FileContents"))
                {
                    e.Effect = e.AllowedEffect;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    control_MouseUp(null, null);
                } 
            }
            else
            {
                e.Effect = DragDropEffects.None;
                control_MouseUp(null, null);
            }
        }

        void GST_Panel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == DragDropEffects.Copy && e.Data.GetDataPresent(typeof(ContentItemDragDropData).FullName))
            { 
                ContentItemDragDropData dropObject = e.Data.GetData(typeof(ContentItemDragDropData).FullName) as ContentItemDragDropData;
                if (dropObject != null && dropObject.Data != null)
                { // 템플릿에서 넘어오는 아이템.
                    Control ctrl = dropObject.Data.ToControl(OpenViewAttributes);
                    if (ctrl != null)
                    {
                        IATContent content = ctrl as IATContent;
                        if (content != null)
                        {
                            content.Data.Left = CurrentMovingControl.Left;
                            content.Data.Top = CurrentMovingControl.Top;
                        }
                        this.Controls.Add(ctrl);

                        if (ctrl is AT_GroupControl)
                        {
                            if (dropObject.IsCopyItem == false)
                            {
                                ((AT_GroupControl)ctrl).Thumb.Left = ctrl.Right + 20;
                                ((AT_GroupControl)ctrl).Thumb.Top = ctrl.Top;
                                ((AT_GroupControl)ctrl).CalcThumb();
                                ((AT_GroupControl)ctrl).IsReadOnly = dropObject.Data.IsReadOnly;
                            }
                            else
                            {
                                ((AT_GroupControl)ctrl).CalcThumb();
                            }
                        }

                        ctrl.Location = NearPoint;
                        ctrl.Focus();
                    }
                }
                control_MouseUp(null, null);
            }
            else if (e.AllowedEffect == (DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move))
            {
                /*     # 외부에서 파일을 드랍했을때!
                       [5]	"FileDrop"	string  :: 파일 목록의 full경로
                       [6]	"FileNameW"	string  :: 파일의 새 이름 ( 첫번째 파일만 )
                       [7]	"FileName"	string  :: 파일의 이름 ( 첫번째 파일만 )
                       [8]	"FileContents"	string  :: null
                */
                if (e.Data.GetDataPresent("FileDrop"))
                {
                    int offset = 0;

                    string[] fileDrop =  e.Data.GetData("FileDrop") as string[];
                    if (fileDrop != null)
                    {
                        foreach (var item in fileDrop)
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
                                    using (Editor.NewImageForm fm = new Editor.NewImageForm() { IsPastImage = true, Image = img.Clone() as Image })
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
                                    imgBox.SetmkData(data, OpenViewAttributes);
                                    this.Controls.Add(imgBox);
                                    this.CurrentMovingControl = imgBox;
                                    this.SetMagnaticDragOver( this.PointToClient(new Point(e.X, e.Y)) );
                                    imgBox.Location = this.NearPoint;
                                    imgBox.Size = sz;
                                 //   imgBox.Left += offset;
                                    imgBox.Top += offset;
                                    imgBox.BringToFront();
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
                                fileBox.SetmkData(data, OpenViewAttributes);
                                this.Controls.Add(fileBox);
                                this.CurrentMovingControl = fileBox;
                                this.SetMagnaticDragOver( this.PointToClient( new Point(e.X, e.Y) ) );
                                fileBox.Location = this.NearPoint;
                              //  fileBox.Left += offset;
                                fileBox.Top += offset;
                                fileBox.BringToFront();
                            }
                            offset += 23;
                        }

                    }     
                }
                control_MouseUp(null, null);
            }

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



        Pen Nomal_Pen = DrawUtil.CreatePan(
                                    Color.DodgerBlue,
                                    2f, 
                                    System.Drawing.Drawing2D.LineCap.NoAnchor,    // ArrowAnchor : >>>
                                    System.Drawing.Drawing2D.LineCap.NoAnchor);

        Pen Event_Pen = DrawUtil.CreatePan(
                                 Color.Coral,
                                 2f,
                                 System.Drawing.Drawing2D.LineCap.NoAnchor,    // ArrowAnchor : >>>
                                 System.Drawing.Drawing2D.LineCap.NoAnchor);
  
        void GST_Panel_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentMovingControl != null)
            {
                for (int loop = 0; loop < (Width / _Width); loop++)
                {
                    for (int loop2 = 0; loop2 < (Height / _Height); loop2++)
                    {
                        int x = sMargin + (loop * _Width);
                        int y = sMargin + (loop2 * _Height);
                        e.Graphics.DrawRectangle(Pens.LightGray, x, y, _Width, _Height);
                        //e.Graphics.FillEllipse(Brushes.Gray, x - 3, y - 3, 6, 6);
                        //e.Graphics.DrawEllipse(Pens.Black, x - 16, y - 16, 32, 32);
                        //TextRenderer.DrawText(e.Graphics, "" + (loop2 * (Width / _Width) + loop + 1), Font, new Point(x + 10, y + 10), Color.Black);
                    }
                }
                //e.Graphics.DrawLine(Pens.Red, label1.Location, NearPoint);  
                //e.Graphics.DrawRectangle(Pens.Yellow, NearPoint.X, NearPoint.Y, _Width, _Width);
                e.Graphics.FillEllipse(Brushes.Orange, NearPoint.X - 4, NearPoint.Y - 4, 8, 8);
                e.Graphics.DrawEllipse(Pens.Silver, NearPoint.X - 8, NearPoint.Y - 8, 16, 16);
            }

            if (IsDragDown)
            {
                Rectangle rrc = dragBox; 
                ControlPaint.DrawVisualStyleBorder(e.Graphics, GetBoxRect(rrc));//, Color.FromArgb(50, Color.DodgerBlue)); 
            }
             
            if (isMDown) return; // 컨트롤 이동중에는 그리지 않음! 

            // 라인 그리기..
            for (int loop = 0; loop < Controls.Count; loop++)
            {
                IATContent content = Controls[loop] as IATContent;
                if (content != null && content is IBoxInCircle)
                {
                    var box = content as IBoxInCircle;
                    foreach (var linkID in content.Data.Links)
                    {
                        IATContent target = FindContent(linkID) as IATContent;
                        if (target != null && target is IBoxInCircle)
                        {
                            JSFW_BoxInCircleEx.DrawCrossLine(
                                   box.BoxInCircle, "<<<",
                                   e.Graphics,
                                    target.Data.ID == CurrentLinkLineID ? Event_Pen : Nomal_Pen,
                                   ((IBoxInCircle)target).BoxInCircle,
                                   DisplayRectangle,
                                   content.Data.Links.Count == 1 ? 0 : 0.3f
                            );
                        }
                    }
                }
                else if (content is AT_GroupControl)
                {
                    ((AT_GroupControl)content).DrawLine(e.Graphics);
                }
            } 
        }

        private Rectangle GetBoxRect(Rectangle rect)
        {
            //            |
            //    1       |      2
            //            |
            //-----------0,0------------
            //            |
            //    3       |      4
            //            |

            // w +, h + 4
            // w -, h + 3
            // w +, h - 2
            // w -, h - 1

            if (rect.Width < 0)
            {
                rect.X = rect.X + rect.Width;
                rect.Width *= -1;
            }

            if (rect.Height < 0)
            {
                rect.Y = rect.Y + rect.Height;
                rect.Height *= -1;
            }
            return rect;
        }

        private IATContent FindContent(LinkData linkID)
        {
            IATContent ctrl = null; 
            for (int loop = 0; loop < Controls.Count; loop++)
            {
                IATContent tmp = Controls[loop] as IATContent;
                if (tmp != null && tmp.Data.ID == linkID.TargetID)
                {
                    ctrl = tmp; break;
                }
            }
            return ctrl;
        }

        void GST_Panel_Disposed(object sender, EventArgs e)
        {
            using (bmp)
            {
                using (grp)
                {
                    Image = null;
                }
            }

            bmp = null;
            grp = null;
             
            UnSelecting(); 
            MultiSelectedControls = null;
            CurrentMovingControl = null;
            for (int loop = Controls.Count - 1; loop >= 0; loop--)
            {
                using (Controls[loop]) { }
            }
            Data = null;
        }

        bool isThumbAddAndRemove = false;

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (isThumbAddAndRemove) return;

            Attatched_Event(e.Control); 
            if (e.Control is AT_GroupControl)
            {
                isThumbAddAndRemove = true;
                this.Controls.Add(((AT_GroupControl)e.Control).Thumb);  
                isThumbAddAndRemove = false;
            } 
            RequestModifyIsDirtyToMainForm(); // 컨트롤 추가.
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (isThumbAddAndRemove == false)
            {
                DeAttatched_Event(e.Control);
            }
            if (e.Control is AT_GroupControl)
            {
                isThumbAddAndRemove = true;
                this.Controls.Remove(((AT_GroupControl)e.Control).Thumb);
                isThumbAddAndRemove = false;
            } 
            base.OnControlRemoved(e);
            if (isThumbAddAndRemove == false)
            {
                RequestModifyIsDirtyToMainForm(); // 컨트롤 삭제
            }
        }

        private void DeAttatched_Event(Control control)
        {
            IATContent innerContent = control as IATContent;
            if (innerContent != null)
            {
                innerContent.OpenViewAttributes -= OpenViewAttributes;
            } 
            control.MouseDown -= control_MouseDown;
            control.MouseUp -= control_MouseUp;
            control.MouseMove -= control_MouseMove;
            control.MouseLeave -= Control_MouseLeave;
            control.MouseEnter -= Control_MouseEnter;
        }

        private void Attatched_Event(Control control)
        {
            DeAttatched_Event(control);

            control.MouseDown += new MouseEventHandler(control_MouseDown);
            control.MouseUp += new MouseEventHandler(control_MouseUp);
            control.MouseMove += new MouseEventHandler(control_MouseMove);
            control.MouseLeave += Control_MouseLeave;
            control.MouseEnter += Control_MouseEnter;

            IATContent innerContent = control as IATContent;
            if (innerContent != null)
            {
                innerContent.OpenViewAttributes += OpenViewAttributes;
            }
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            CurrentReSizeControl = sender as Control;
        }
         
        private void Control_MouseLeave(object sender, EventArgs e)
        {
            CurrentReSizeControl = null;
        }

        void control_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
             
            CurrentMovingControl = null;
            CurrentReSizeControl = null;

            if (isSleep)
            {
                isMDown = IsOldMDown = false; isSleep = false;
            }
            else
            {
                isMDown = IsOldMDown = false;
                IsRS = false;
            }
            oldDistance = 10000d;
             
            ReCalcBox( sender as Control);
            //Invalidate(Rectangle.FromLTRB(oldNearPoint.X - 32, oldNearPoint.Y - 32, oldNearPoint.X + 32, oldNearPoint.Y + 32));
            //Invalidate(Rectangle.FromLTRB(NearPoint.X - 32, NearPoint.Y - 32, NearPoint.X + 32, NearPoint.Y + 32));
        }

        public void LayoutAlign()
        {
            // 정렬위치 바른 계산을 위해 자동정렬 시켜줌 ( 마우스로 끌어다 맞추니 조금씩 틀어지는 것 바로 잡아 줄세움 )
            // 가까운 점으로 컨트롤 위치 이동시켜줌.
            foreach (Control ctrl in Controls)
            {
                CurrentMovingControl = ctrl;
                SetMagnaticDragOver(ctrl.Location);
                ctrl.Location = NearPoint;
            }
            CurrentMovingControl = null;
            Invalidate();
        }
         
        private void ReCalcBox(Control sender = null)
        {
            if (CurrentReSizeControl != null)
            {
                sender = CurrentReSizeControl;
            }

            if (sender != null)
            {
                RSZ.X = sender.Width - OFFSET;
                RSZ.Y = sender.Height - OFFSET;
                RSZ.Width = OFFSET;
                RSZ.Height = OFFSET;
            }
            else
            {
                RSZ.X = - OFFSET;
                RSZ.Y = - OFFSET;
                RSZ.Width = OFFSET;
                RSZ.Height = OFFSET;
            }
        }

        void control_MouseDown(object sender, MouseEventArgs e)
        { 
            CurrentMovingControl = null;

            IATContent content = sender as IATContent;
            /*
                아이템 선택의 조건!

                1. 마우스 왼쪽 버튼을 클릭하여 아이템을 선택한 경우.
                    - Shift 키가 같이 눌리지 않고, 선택된 그룹에 속하지 않았을때 ... 선택그룹을 초기화.
                
                2. 마우스 왼쪽 버튼이 Shift나 Control이 동시에 눌리지 않은 경우 
                    - 아이템 선택으로 간주하여 선택 그룹에 등록
                3. 마우스 왼쪽 버튼을 클릭하고 Shift 키가 눌렸을 경우
                    - 선택그룹에 추가되어 있지 않은 아이템은 추가로 등록. 
             */

            if (content != null && MultiSelectedControls.Contains(content) == false)
            {
                //시프트가 눌리지 않았다면!!!
                if (!((ModifierKeys & Keys.Shift) == Keys.Shift)) UnSelecting();
            }

            isMDown = IsOldMDown = (e.Button == MouseButtons.Left &&
                                       !((ModifierKeys & Keys.Control) == Keys.Control ||
                                          (ModifierKeys & Keys.Shift) == Keys.Shift));

            if (isMDown)
            {
                CurrentMovingControl = sender as Control;

                if (MultiSelectedControls.Contains(content) == false)
                {
                    MultiSelectedControls.Add(content); content.Selecting();
                }
            }

            if ((e.Button == MouseButtons.Left) && (ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                if (MultiSelectedControls.Contains(content) == false)
                {
                    MultiSelectedControls.Add(content); content.Selecting();
                }
            }

            oldDistance = 10000d;
            isSleep = false;
            pt = e.Location;

            if (isMDown)
            {
                ReCalcBox(sender as Control);

                if (RSZ.Contains(e.Location))
                {
                    IsRS = true;
                    Cursor = Cursors.SizeNWSE;
                    return;
                }
            }
            Cursor = Cursors.Default;
        }

        void control_MouseMove(object sender, MouseEventArgs e)
        { 
            if (isMDown && CurrentMovingControl != null)
            {
                if (IsRS)
                {
                    int x = (e.X - pt.X);
                    int y = (e.Y - pt.Y);
                    int w = CurrentMovingControl.Width + x;
                    int h = CurrentMovingControl.Height + y;
                    if (CurrentMovingControl.Width + x < 20)
                    {
                        w = 20;
                    }
                    if (CurrentMovingControl.Height + y < 20)
                    {
                        h = 20;
                    }
                    CurrentMovingControl.Width = w;
                    CurrentMovingControl.Height = h;
                    pt = e.Location; // 1개씩 늘리자.
                }
                else
                {
                    CurrentMovingControl.Left += e.Location.X - pt.X;
                    CurrentMovingControl.Top += e.Location.Y - pt.Y;
                }

                foreach (var item in MultiSelectedControls)
                {
                    if (CurrentMovingControl.Equals(item)) continue;

                    ((Control)item).Left += e.Location.X - pt.X;
                    ((Control)item).Top += e.Location.Y - pt.Y;
                }

                if (MultiSelectedControls.Count <= 1)
                {
                    SetMagnatic(CurrentMovingControl);
                }
                RequestModifyIsDirtyToMainForm(); // 컨트롤 위치 변경시 변경여부 on

                //Invalidate(Rectangle.FromLTRB(oldNearPoint.X - 32, oldNearPoint.Y - 32, oldNearPoint.X + 32, oldNearPoint.Y + 32));
                //Invalidate(Rectangle.FromLTRB(NearPoint.X - 32, NearPoint.Y - 32, NearPoint.X + 32, NearPoint.Y + 32));
                this.Invalidate(); 
            }

            ReCalcBox(sender as Control);

            if (RSZ.Contains(e.Location))
            {
                Cursor = Cursors.SizeNWSE;
                return;
            }
            else
            {
                Cursor = Cursors.Default;
            } 
        }

        private void SetMagnatic(Control calcControl)
        {
            if (calcControl == null) return;

            int r = calcControl.Top / _Height;
            int c = calcControl.Left / _Width;
            if (0 < calcControl.Left % _Width)
            {
                c++;
            }
            int cnt = (Width / _Width);

            // 가까운 점 구하기 
            int xx = c * _Width;
            if ((calcControl.Left % _Width) != 0 && (calcControl.Left % _Width) < (_Width / 2))
            {
                xx = (c - 1) * _Width;
            }
            int yy = yy = r * _Height;
            if (0 < (calcControl.Top % _Height) / (_Height / 2))
            {
                yy = (r + 1) * _Height;
            }
            oldNearPoint.X = NearPoint.X;
            oldNearPoint.Y = NearPoint.Y;

            NearPoint = new Point(sMargin + xx, sMargin + yy);

            double dx = Math.Abs(calcControl.Left - NearPoint.X);
            double dy = Math.Abs(calcControl.Top - NearPoint.Y);
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (Math.Abs(distance / 2) > 2)
            {
                int arrow = 0;
                if (oldDistance > distance)
                {
                    arrow = -1;
                }
                else if (oldDistance < distance)
                {
                    arrow = 1;
                }

                if (arrow < 0 && distance <= 8)
                {
                    calcControl.Left = NearPoint.X;
                    calcControl.Top = NearPoint.Y;
                    oldDistance = distance = 0;
                    IsOldMDown = isMDown;
                    isMDown = false;
                    isSleep = true;
                    Action sleep = new Action(DragSleep);
                    sleep.BeginInvoke(ir => sleep.EndInvoke(ir), sleep);
                }
                else
                {
                    oldDistance = distance;
                }
                //label1.Text = "" + (r * cnt + c) + ", " + (int)distance + " : " + arrow;
                //label1.Update();
                // todo : 컨트롤 위치값 기록! 
                IATContent content = calcControl as IATContent;
                if (content != null)
                {
                    content.Data.Left = calcControl.Left;
                    content.Data.Top = calcControl.Top;
                }
            }
        }

        private void DragSleep()
        {
            int time = 12;
            while (isSleep && (0 < time--))
            { // 120ms 동안.. 
                System.Threading.Thread.Sleep(10);
            }
            isMDown = IsOldMDown;
        }

        public PreziDataClass GetPreziDataClass()
        {
            if (Data == null) return null;

            UnSelecting();
        
            PreziDataClass data = this.Data;
            data.TypeName = GetType().FullName;
            data.Left = 5;// Left;
            data.Top = 5;// Top;
            data.Height = Height;
            data.Width = Width;
            data.InnerDatas.Clear();
            //data.DrawingShapes.AddRange(DrawingShapes.ToArray());

            List<PreziDataClass> Datas = new List<PreziDataClass>();
            foreach (Control InnerCtrl in Controls)
            { 
                int r = InnerCtrl.Top / _Height;
                int c = InnerCtrl.Left / _Width;
                if (0 < InnerCtrl.Left % _Width)
                {
                    c++;
                }
                int cnt = (Width / _Width);

                IATContent content = InnerCtrl as IATContent;
                if (content != null)
                {
                    System.Reflection.MethodInfo getmkDataMethodInfo = InnerCtrl.GetType().GetMethod("GetmkData", System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (getmkDataMethodInfo != null)
                    {
                        PreziDataClass InnerData = getmkDataMethodInfo.Invoke(InnerCtrl, null) as PreziDataClass;
                        if (InnerData != null)
                        {
                            Datas.Add(InnerData);
                        }
                    }
                }
            }
            data.InnerDatas.AddRange(Datas.ToList());
            Datas.Clear();
            return data;
        }

        internal void ChangePen(float penSize, Color penColor)
        {
            if (BrushEreaseMode)
            {
                // 그림 그릴때 펜
                CurrentPenWidth = penSize;
                CurrentColour = Color.FromArgb(255, penColor );
                CurrentPen = new Pen(CurrentColour, penSize)
                {
                    StartCap = System.Drawing.Drawing2D.LineCap.Round,
                    EndCap = System.Drawing.Drawing2D.LineCap.Round,
                    //DashStyle = System.Drawing.Drawing2D.DashStyle.Solid,
                    //MiterLimit = 12,
                    //LineJoin = System.Drawing.Drawing2D.LineJoin.MiterClipped,
                    //Alignment = System.Drawing.Drawing2D.PenAlignment.Center
                };
            }
            else
            {
                // 지우개 펜
                CurrentPen = new Pen(Color.Transparent, penSize)
                {
                    StartCap = System.Drawing.Drawing2D.LineCap.Round,
                    EndCap = System.Drawing.Drawing2D.LineCap.Round,
                    //DashStyle = System.Drawing.Drawing2D.DashStyle.Solid,
                    //MiterLimit = 12,
                    //LineJoin = System.Drawing.Drawing2D.LineJoin.MiterClipped,
                    //Alignment = System.Drawing.Drawing2D.PenAlignment.Center
                };
            }
        }
         
        public void SetPreziDataClass(PreziDataClass data, Action<IATContent, ATEditStateEnum> openViewAttributes)
        {
            UnSelecting();  
            CurrentMovingControl = null;
            for (int loop = 0; loop < Controls.Count; loop++)
            { 
                using (Controls[loop]) { }
            }
            Controls.Clear();

            if (data == null) return;

            Data = data;

            Left = 5;// data.Left;
            Top = 5;// data.Top;

            //Height = data.Height;
            //Width = data.Width;

            LoadDrawImage();
            ReLoadBackGroundImage();

            OpenViewAttributes -= openViewAttributes;
            OpenViewAttributes += openViewAttributes;

            foreach (PreziDataClass innerContent in data.InnerDatas)
            {
                Type innerType = Type.GetType(innerContent.TypeName);
                Control innerCtrl = Activator.CreateInstance(innerType) as Control;
                if (innerCtrl != null)
                {
                    System.Reflection.MethodInfo setmkDataMethodInfo = innerType.GetMethod("SetmkData", System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (setmkDataMethodInfo != null)
                    {
                        setmkDataMethodInfo.Invoke(innerCtrl, new object[] { innerContent, openViewAttributes });
                    }
                    Controls.Add(innerCtrl);

                    if (innerCtrl.Left < 0) innerCtrl.Left = 0;
                    if (innerCtrl.Top < 0) innerCtrl.Top = 0;

                    if ( Width < innerCtrl.Left) innerCtrl.Left = Right - innerCtrl.Width;
                    if ( Height < innerCtrl.Top) innerCtrl.Top = Bottom - innerCtrl.Height;
                }
            }
             
            Invalidate();
        }
         
        /// <summary>
        /// 에디터에만 적용.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isRequired"></param>
        /// <param name="text"></param>
        public void SetKeyInfo(string key, bool isRequired, string text)
        {
        }

        private void RequestModifyIsDirtyToMainForm()
        {
            //변경여부를 알림. 
            //IDesignView mainForm = FindForm() as IDesignView;
            //if (mainForm != null) mainForm.IsDirty = true;
        }
         
        public void SetFontStyleAndBorder(int fontSize, bool fontBold, bool fontUnderLine, HorizontalAlignment textAlign, Color foreColor, Color backColor, AT_Border border)
        {
            // 구현 X
        }
         
        bool IsSelected = false;
        Color NomalColor { get; set; }
        public void Selecting()
        {
            //NomalColor = BackColor;
            //IsSelected = true;
        }

        public void UnSelecting()
        {
            if (MultiSelectedControls != null)
            {
                foreach (var item in MultiSelectedControls)
                {
                    item.UnSelecting();
                }
                MultiSelectedControls.Clear();
            }
        }

        /// <summary>
        /// 펜모드, 마우스모드( play모드 )
        /// </summary>
        /// <param name="isPenMode"></param>
        public void ChangePenMode(bool isPenMode)
        {
            IsPenMode = isPenMode;
            //if (isPenMode) // 커서를 숨기니 ... 프로그램 밖에서 이상하게 동작함.
            //{
            //    Cursor.Hide();
            //}
            //else
            //{
            //    Cursor.Show();
            //}
            this.Refresh();
        }

        /// <summary>
        /// 그리기(true), 지우개(false)
        /// </summary>
        /// <param name="isBrush"></param>
        public void ChangeBrushEraseMode(bool isBrush)
        {
            BrushEreaseMode = isBrush;
            this.Refresh();
        }
    }

    public enum AT_Border
    {
        NONE,
        SOLID,
        DOT,
    }

    public interface IATContent
    {
        /// <summary>
        /// 대상컨트롤, 속성수정여부( isComplite )
        /// * 수정 후 컨트롤 크기가 변경되는 경우 mv가 따라가야 하므로.. 
        /// </summary>
        event Action<IATContent, ATEditStateEnum> OpenViewAttributes;

        System.Windows.Forms.Control EditControl { get; }

        PreziDataClass Data { get; }

        void SetFontStyleAndBorder(int fontSize, bool fontBold, bool fontUnderLine, HorizontalAlignment textAlign, Color foreColor, Color backColor, AT_Border border);

        void Selecting();

        void UnSelecting();
    }

    public enum ATEditStateEnum
    {
        None,
        Begin,
        End
    }

    internal static class JSFW_Design_Const
    {
        public readonly static int PanlBlockWidth = 52;
        public readonly static int PanlBlockHeight = 23;
    }

    public static class PreziDataClassEx
    {
        public static System.Windows.Forms.Control ToControl(this PreziDataClass data, Action<IATContent, ATEditStateEnum> openViewAttributes)
        {
            System.Windows.Forms.Control ctrl = null;

            if (data != null)
            {

                Type type = Type.GetType(data.TypeName);
                ctrl = Activator.CreateInstance(type) as System.Windows.Forms.Control;

                System.Reflection.MethodInfo setmkDataMethodInfo = type.GetMethod("SetmkData", System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (setmkDataMethodInfo != null)
                {
                    setmkDataMethodInfo.Invoke(ctrl, new object[] { data, openViewAttributes });
                }

                foreach (PreziDataClass innerContent in data.InnerDatas)
                {
                    System.Windows.Forms.Control innerCtrl = innerContent.ToControl(openViewAttributes);
                    if (innerCtrl != null)
                    {
                        ctrl.Controls.Add(innerCtrl);
                    }
                }
            }
            return ctrl;
        }
    }


    public class ContentItemDragDropData 
    {
        public bool IsCopyItem { get; set; }

        public PreziDataClass Data { get; set; }
         
        public ContentItemDragDropData(PreziDataClass data)
        {
            // 템플릿으로 올때.
            Data = data;
        }
    } 
}
