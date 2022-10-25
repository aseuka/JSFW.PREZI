using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace JSFW.PREZI.Controls.Editor
{
    public partial class Editor_NewDrawingPicture : Form
    {
        string newTempImageName { get; set; }

        public bool IsPenMode = true; // 펜모드( true ) 

        /// <summary>
        /// false 는 지우개
        /// </summary>
        private bool BrushEreaseMode = true;//펜(true)으로 그릴지, 지우개(false)로 사용할지.

        private Bitmap bmp; // Place to store our drawings
        private List<Point> points = new List<Point>(); // Points of currently drawing line
        Point MouseLoc;
        internal float CurrentPenWidth = 3;
        internal Color CurrentColour = Color.Black;
        internal Pen CurrentPen = new Pen(Color.Black, 3); // 현재 펜
        private bool isPenMDown = false;
        
        public string ImageName { get; set; }
        public string ImageTag { get; set; }
        public string ImageLocation { get; set; }

        private bool haveDrawing = false;

        public Editor_NewDrawingPicture()
        {
            InitializeComponent();

            DoubleBuffered = true;

            SetNonPublicProperty(panel1, "DoubleBuffered", true);
            GetNonPublicMethod(panel1, "SetStyle", new object[] { System.Windows.Forms.ControlStyles.UserPaint | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | System.Windows.Forms.ControlStyles.DoubleBuffer, true });

            this.Disposed += Editor_NewDrawingPicture_Disposed; 

            this.panel1.Enter += Panel1_Enter;
            this.panel1.Leave += Panel1_Leave;
            this.panel1.Paint += Panel1_Paint;

            this.panel1.MouseDown += Panel1_MouseDown;
            this.panel1.MouseUp += Panel1_MouseUp;
            this.panel1.MouseMove += Panel1_MouseMove;

            ChangePenMode(true);

            SetStyle(System.Windows.Forms.ControlStyles.UserPaint | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            LoadDrawImage(); 
        }

        private static object GetNonPublicProperty(object obj, string propertyName)
        {
            PropertyInfo p = obj.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
            return p.GetValue(obj, null);
        }

        private static void SetNonPublicProperty(object obj, string propertyName, object value)
        {
            PropertyInfo p = obj.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty);
            p.SetValue(obj, value);
        }

        private static void GetNonPublicMethod(object obj, string methodName, object[] values)
        {
            MethodInfo m = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            m.Invoke(obj, values);
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            lbCursorSetItem(lbPen);
            lbSizeSetItem(lbPenSize_3);
            lbBrushSetItem(lbBrush_Black);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate(true);
        }

        private void Panel1_Leave(object sender, EventArgs e)
        {
            Cursor.Show();
        }

        private void Panel1_Enter(object sender, EventArgs e)
        {
            ChangePenMode(IsPenMode); 
        }
         
        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isPenMDown = false; 
            panel1.Invalidate();

            SaveToBitmap(); // Save the drawn line to bitmap
            points.Clear(); // Our drawing is saved, we can clear the list of points
             
            base.OnMouseUp(e); 
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPenMDown = true;
                points.Clear();
                points.Add(new Point(e.X, e.Y)); // Remember the first point
                MouseLoc = e.Location;
                panel1.Invalidate();
            }
        }
         
        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsPenMode && isPenMDown)
            {
                haveDrawing = true;
                //PAINTING
                MouseLoc = e.Location;
                points.Add(new Point(e.X, e.Y)); // Add points to path 
               
                //refresh the panel so it will be forced to re-draw.
                panel1.Refresh();
            }
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (bmp == null) return;
             
            e.Graphics.Clear(Color.Transparent);
            e.Graphics.Clear(Color.White);

            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

            e.Graphics.DrawImage(bmp, 0, 0); // Show what is drawn 
            //if (points.Count > 1)
            //    e.Graphics.DrawLines(pen, points.ToArray()); // Show what is currently being drawn
            Point _pt = Point.Empty;
            for (int loop = 0; loop < points.Count; loop++)
            {
                if (_pt == Point.Empty) _pt = points[loop];

                e.Graphics.DrawLine(CurrentPen, _pt, points[loop]);
                _pt = points[loop];
            }

            if (IsPenMode)
            {
                if (!BrushEreaseMode)
                {
                    // e.Graphics.FillEllipse(Brushes.DimGray, MouseLoc.X - (CurrentPen.Width / 2), MouseLoc.Y - (CurrentPen.Width / 2), CurrentPen.Width, CurrentPen.Width);
                    e.Graphics.DrawEllipse(Pens.DimGray, MouseLoc.X - (CurrentPen.Width / 2) + 2, MouseLoc.Y - (CurrentPen.Width / 2) + 2, CurrentPen.Width - 4, CurrentPen.Width - 4);
                }
                else
                {
                    e.Graphics.DrawEllipse(CurrentPen, MouseLoc.X - (CurrentPenWidth / 2), MouseLoc.Y - (CurrentPenWidth / 2), CurrentPenWidth, CurrentPenWidth);
                }
            }
        }

        private void Editor_NewDrawingPicture_Disposed(object sender, EventArgs e)
        {
            using (bmp)
            {
                bmp = null;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 등록정보 유효성 체크. 
            // 드로잉 여부 확인.
            //public string ImageName { get; set; }
            //public string ImageTag { get; set; }
            //public string ImageLocation { get; set; } 
            ImageName = textBox1.Text.Trim();
            ImageTag = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(ImageName))
            {

                "이미지명이 필요함.".Alert();
                return;
            }

            if (string.IsNullOrWhiteSpace(ImageTag))
            {

                "이미지 Tag명이 필요함.".Alert();
                return;
            }

            if (!haveDrawing)
            {
                "그림을 그려야 함.".Alert();
                return;
            }

            try
            {
                using (bmp)
                {
                    // 이미지를 잘라야지?? 
                    panel1.BorderStyle = BorderStyle.None;
                    using (Bitmap tmp = new Bitmap(panel1.ControlShot(-1, copyClipboard: false)))
                    {
                        tmp.MakeTransparent(Color.DimGray);
                        tmp.MakeTransparent(Color.White);
                        tmp.MakeTransparent(Color.Transparent);
                        tmp.Save(ImageLocation);
                    }
                    panel1.BorderStyle = BorderStyle.FixedSingle;
                }
            }
            finally
            {
                bmp = null;
            }
         
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 드로잉! 한것이면 confirm 

            using (bmp)
            {
                bmp = null;
            }

            DialogResult = DialogResult.Cancel;
            this.Close();
        }


        internal void ChangePen(float penSize, Color penColor)
        {
            if (BrushEreaseMode) // false 는 지우개
            {
                CurrentPenWidth = penSize;
                CurrentColour = penColor;
                CurrentPen = new Pen(penColor, penSize) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round };
            }
            else
            {
                CurrentPen = new Pen(Color.White, penSize) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round };
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
            panel1.Refresh();
        }
         
        private void LoadDrawImage()
        {
            if (bmp != null)
            {
                bmp.Dispose();
                bmp = null;
            }
            
            newTempImageName = Path.GetTempFileName();

            newTempImageName = Path.GetFileName(newTempImageName).Replace(".tmp", "");
            
            string drawImagePath = JSFW_PREZI_CONST.ProjectNewImagesDirectoryName + "\\" + newTempImageName + ".png";

            ImageLocation = drawImagePath;// JSFW_PREZI_CONST.ProjectNewImagesDirectoryName + "\\";

            textBox1.Text = textBox2.Text = newTempImageName;
            
            if (System.IO.File.Exists(drawImagePath))
            {
                using (var img = Image.FromFile(drawImagePath, true))
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

            haveDrawing = false;

            points.Clear();
        }

        private void SaveToBitmap()
        {
            if (points.Count <= 1)
                return;

            //  Console.WriteLine($"점 갯수 : {points.Count}");
            Point _pt = Point.Empty;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //g.Clear(Color.DimGray);
                //g.Clear(Color.White);
                //g.Clear(Color.Transparent);

                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

                //  g.DrawLines(CurrentPen, points.ToArray());
                for (int loop = 0; loop < points.Count; loop++)
                {
                    if (_pt == Point.Empty) _pt = points[loop];

                    g.DrawLine(CurrentPen, _pt, points[loop]);
                    _pt = points[loop];
                }
            }
             
            string drawImagePath = JSFW_PREZI_CONST.ProjectNewImagesDirectoryName + "\\" + newTempImageName + ".png";
            bmp.Save(drawImagePath, System.Drawing.Imaging.ImageFormat.Png);
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
            ChangePenMode(false);
            ChangeBrushEraseMode(false);
            ChangePen(CurrentPenWidth, CurrentColour);

            panel1.Invalidate();
        }

        private void lbPen_Click(object sender, EventArgs e)
        {
            lbCursorSetItem(sender as Label);
            ChangePenMode(true);
            ChangeBrushEraseMode(true);
            ChangePen(CurrentPenWidth, CurrentColour);

            panel1.Invalidate();
        }

        private void lbErase_Click(object sender, EventArgs e)
        {
            lbCursorSetItem(sender as Label);
            ChangePenMode(true);
            ChangeBrushEraseMode(false);
            ChangePen(24, Color.Transparent);

            panel1.Invalidate();
        }

        private void lbPenSize_3_Click(object sender, EventArgs e)
        {
            lbSizeSetItem(sender as Label);
            ChangePenMode(true);
            ChangeBrushEraseMode(true);
            ChangePen(3, CurrentColour);
            lbCursorSetItem(lbPen);

            panel1.Invalidate();
        }

        private void lbPenSize_8_Click(object sender, EventArgs e)
        {
            lbSizeSetItem(sender as Label);
            ChangePenMode(true);
            ChangeBrushEraseMode(true);
            ChangePen(8, CurrentColour);
            lbCursorSetItem(lbPen);

            panel1.Invalidate();
        }

        private void lbBrush_Black_Click(object sender, EventArgs e)
        {
            lbBrushSetItem(sender as Label);
            ChangePenMode(true);
            ChangeBrushEraseMode(true);
            ChangePen(CurrentPenWidth, Color.Black);
            lbCursorSetItem(lbPen);

            panel1.Invalidate();
        }

        private void lbBrush_Red_Click(object sender, EventArgs e)
        {
            lbBrushSetItem(sender as Label);
            ChangePenMode(true);
            ChangeBrushEraseMode(true);
            ChangePen(CurrentPenWidth, Color.Red);
            lbCursorSetItem(lbPen);

            panel1.Invalidate();
        }

        private void lbBrush_Blue_Click(object sender, EventArgs e)
        {
            lbBrushSetItem(sender as Label);
            ChangePenMode(true);
            ChangeBrushEraseMode(true);
            ChangePen(CurrentPenWidth, Color.Blue);
            lbCursorSetItem(lbPen);

            panel1.Invalidate();
        }

        private void lbBrush_Yellow_Click(object sender, EventArgs e)
        {
            lbBrushSetItem(sender as Label);
            ChangePenMode(true);
            ChangeBrushEraseMode(true);
            ChangePen(CurrentPenWidth, Color.Yellow);
            lbCursorSetItem(lbPen);

            panel1.Invalidate();
        }
    }
}
