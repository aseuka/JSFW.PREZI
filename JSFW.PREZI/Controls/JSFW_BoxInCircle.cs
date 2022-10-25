using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using JSFW.Common.Drawing;
using JSFW.Common.Controls.Common;

namespace JSFW.Common.Controls.Common
{
    public class JSFW_BoxInCircle
    {
        internal IBoxInCircle Box { get; set; }
        const double PointCount = 8;
        double Radius { get { return GetRadus(); } }

        protected internal SortedList<char, PointF> Points = new SortedList<char, PointF>();

        private double GetRadus()
        {
            int bw = Box.HostControl.Width;
            int hw = Box.HostControl.Height;
            return (bw > hw ? hw / 2d : bw / 2d) - 8;
        }

        protected PointF Origin
        {
            get { return GetOrigin(); }
        }

        private PointF GetOrigin()
        {
            return new PointF(this.Box.HostControl.DisplayRectangle.Left + this.Box.HostControl.Width / 2f, this.Box.HostControl.DisplayRectangle.Top + this.Box.HostControl.Height / 2f);
        }

        public JSFW_BoxInCircle(IBoxInCircle box)
        {
            this.Box = box;
            this.Box.HostControl.Paint += new System.Windows.Forms.PaintEventHandler(box_Paint);
            this.Box.HostControl.Resize += new EventHandler(box_Resize);
            this.Box.HostControl.Move += new EventHandler(box_Move);
        }

        void box_Move(object sender, EventArgs e)
        {
            if (Box.HostControl.Parent != null) Box.HostControl.Parent.Invalidate();
        }

        void box_Resize(object sender, EventArgs e)
        {
            if (Box.HostControl.Parent != null) Box.HostControl.Parent.Invalidate();
            Box.HostControl.Invalidate();
        }

        void box_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            PaintCircle(e.Graphics);
            //CalcDotPoints();
        }

        private void PaintCircle(Graphics g)
        {
            PointF[] points = CalcDotPoints();
            // 컨트롤내에 점 찍기 
            //PointF temp = points[0];
            //for (int loop = 1; loop < points.Length; loop++)
            //{
            //    g.DrawLine(Pens.Black, temp, points[loop]);
            //    g.FillEllipse(Brushes.Red, temp.X - 5, temp.Y - 5, 10, 10);
            //    g.DrawString("" + Points.Keys[loop], Box.HostControl.Font, Brushes.Blue, points[loop]);
            //    temp = points[loop];
            //}
            //g.DrawLine(Pens.Black, temp, points[0]);
            //g.FillEllipse(Brushes.Red, temp.X - 5, temp.Y - 5, 10, 10);
            //g.DrawString("" + Points.Keys[0], Box.HostControl.Font, Brushes.Blue, points[0]);
        }

        internal PointF[] CalcDotPoints()
        {
            Points.Clear();
            double theta = ((360d / PointCount) / 180d) * Math.PI;
            PointF origin = Origin;
            double x = 0f;
            double y = 0f;
            double r = GetRadus();
            for (int loop = 0; loop < PointCount; loop++)
            {
                x = r * Math.Sin(theta * loop);
                y = r * Math.Cos(theta * loop);
                Points.Add((char)('a' + loop), new PointF(origin.X + (float)x, origin.Y + (float)y - 2));
            }
            return Points.Values.ToArray();
        }

        public KeyValuePair<char, PointF>[] NearPointSearch(IBoxInCircle target)
        {
            Func<float, float, float, float, double> CalcRadius = (x, y, tx, ty) =>
            {
                return Math.Sqrt(Math.Pow(x - tx, 2) + Math.Pow(y - ty, 2));
            };

            //this.Points
            //target.C.Points
            KeyValuePair<char, PointF>[] v = new KeyValuePair<char, PointF>[2];
            double Min = double.MaxValue;
            char k1 = '-';
            char k2 = '-';
            foreach (var p1 in this.Points)
            {
                Point p1Location = this.Box.HostControl.PointToScreen(Point.Round(p1.Value));
                foreach (var p2 in target.BoxInCircle.Points)
                {
                    Point p2Location = target.HostControl.PointToScreen(Point.Round(p2.Value));
                    double r = CalcRadius(p1Location.X, p1Location.Y, p2Location.X, p2Location.Y);
                    if (r < Min)
                    {
                        Min = r;
                        k1 = p1.Key;
                        k2 = p2.Key;
                    }
                }
            }
            if (k1 == '-' || k2 == '-') return null;
            v[0] = new KeyValuePair<char, PointF>(k1, this.Box.HostControl.PointToScreen(Point.Round(this.Points[k1])));
            v[1] = new KeyValuePair<char, PointF>(k2, target.HostControl.PointToScreen(Point.Round(target.BoxInCircle.Points[k2])));

            return v;
        }
    }
}

namespace JSFW.Common.Controls.Common
{
    public interface IBoxInCircle
    {
        /// <summary>
        /// 컨트롤 내에 Circle 박스
        /// </summary>
        JSFW_BoxInCircle BoxInCircle { get; }
        /// <summary>
        /// 대상 컨트롤
        /// </summary>
        Control HostControl { get; }
    }
}

namespace JSFW.Common.Drawing
{
    internal class LineClass
    {
        public enum LinePosition { None, Left, Top, Right, Bottom }

        public LinePosition Type { get; internal set; }

        public LineClass(float x1, float y1, float x2, float y2)
            : this(new PointF(x1, y1), new PointF(x2, y2))
        { }

        public LineClass(PointF p1, PointF p2)
        {
            StartPointF = p1;
            EndPointF = p2;
        }

        public PointF StartPointF { get; private set; }
        public PointF EndPointF { get; private set; }

        private PointF GetOrigin(Rectangle rct)
        {
            return new PointF(rct.X + rct.Width / 2f, rct.Y + rct.Height / 2f);
        }

        internal static bool GetAcrossPointF(LineClass l1, LineClass l2, ref PointF across)
        {
            return GetIntersectPointF(l1.StartPointF, l1.EndPointF, l2.StartPointF, l2.EndPointF, ref across);
        }

        internal static bool GetIntersectPointF(PointF AP1, PointF AP2, PointF BP1, PointF BP2, ref PointF CrossPointF)
        {
            /*출처 : http://www.gisdeveloper.co.kr/15 */
            double t;
            double s;
            double under = (BP2.Y - BP1.Y) * (AP2.X - AP1.X) - (BP2.X - BP1.X) * (AP2.Y - AP1.Y);
            if (under == 0) return false;

            double _t = (BP2.X - BP1.X) * (AP1.Y - BP1.Y) - (BP2.Y - BP1.Y) * (AP1.X - BP1.X);
            double _s = (AP2.X - AP1.X) * (AP1.Y - BP1.Y) - (AP2.Y - AP1.Y) * (AP1.X - BP1.X);

            t = _t / under;
            s = _s / under;

            if (t < 0.0 || t > 1.0 || s < 0.0 || s > 1.0) return false;
            if (_t == 0 && _s == 0) return false;

            CrossPointF.X = AP1.X + (float)(t * Convert.ToDouble(AP2.X - AP1.X));
            CrossPointF.Y = AP1.Y + (float)(t * Convert.ToDouble(AP2.Y - AP1.Y));

            return true;
        }

        internal static PointF CalcAcrossPointF(LineClass BetweenControlCenterLine, Rectangle rectangle, out LineClass.LinePosition type)
        {
            type = LineClass.LinePosition.None;
            PointF cross = PointF.Empty;
            foreach (LineClass item in CalcRectangleAcrossLine(rectangle))
            {
                if (LineClass.GetAcrossPointF(BetweenControlCenterLine, item, ref cross))
                {
                    type = item.Type;
                    break;
                }
            }
            return cross;
        }

        internal static LineClass[] CalcRectangleAcrossLine(Rectangle rct)
        {
            LineClass top = new LineClass(rct.Left, rct.Top, rct.Right, rct.Top) { Type = LineClass.LinePosition.Top };
            LineClass left = new LineClass(rct.Left, rct.Top, rct.Left, rct.Bottom) { Type = LineClass.LinePosition.Left };
            LineClass bottom = new LineClass(rct.Left, rct.Bottom, rct.Right, rct.Bottom) { Type = LineClass.LinePosition.Bottom };
            LineClass right = new LineClass(rct.Right, rct.Top, rct.Right, rct.Bottom) { Type = LineClass.LinePosition.Right };
            return new LineClass[] { left, top, bottom, right };
        }
    }

    public static class DrawUtil
    {
        public static Pen CreatePan(this Color c, float width, System.Drawing.Drawing2D.LineCap startcap = System.Drawing.Drawing2D.LineCap.Square, System.Drawing.Drawing2D.LineCap endcap = System.Drawing.Drawing2D.LineCap.Square)
        {
            Pen p = new Pen(c, width);
            p.StartCap = startcap;
            p.EndCap = endcap;
            return p;
        }
    }

    public static class JSFW_BoxInCircleEx
    {
        static PointF startPoint;
        static PointF endPoint;
        static LineClass.LinePosition startType = LineClass.LinePosition.None;
        static LineClass.LinePosition endType = LineClass.LinePosition.None;
        static LineClass line;
        static PointF startPointF;
        static PointF endPointF;

        static PointF GetOrigin(Rectangle rct)
        {
            return new PointF(rct.X + rct.Width / 2f, rct.Y + rct.Height / 2f);
        }

        static float getTextAngle(PointF p1, PointF p2)
        {
            double angle = Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * 180d / Math.PI;
            if (angle < 0) angle += 360d;
            return (float)angle;
        }

        /// <summary>
        /// 사각형 중심위치구하기
        /// </summary>
        /// <param name="rct"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        static void CalcRectangleCenterPoint(RectangleF rct, out float cx, out float cy)
        {
            cx = rct.Width / 2f + rct.Left;
            cy = rct.Height / 2f + rct.Top;
        }

        /// <summary>
        /// 컨트롤들 가상 박스 구하기.
        /// </summary>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <returns></returns>
        static RectangleF GetObjectOutLineRectangle(RectangleF[] rcts, float cx, float cy)
        {
            float x, y, r, b;
            x = 100000; y = 10000; r = -10000f; b = -10000f;
            foreach (RectangleF rct in rcts)
            {
                if (x > rct.Left) x = rct.Left;
                if (y > rct.Top) y = rct.Top;
                if (r < rct.Right) r = rct.Right;
                if (b < rct.Bottom) b = rct.Bottom;
            }
            return RectangleF.FromLTRB(x, y, r, b);
        }

        const float crossRate = 10f;
        public static void DrawLine(this JSFW_BoxInCircle box, string LineText, Graphics g, Pen linePan, JSFW_BoxInCircle target, RectangleF area, float DrawCmdIndex = 0)
        {
            //그려지는 컨트롤에 Paint event 이어야 정상동작함! 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            KeyValuePair<char, PointF>[] v = box.NearPointSearch(target.Box);
            if (v != null)
            {
                startPoint = box.Box.HostControl.Parent.PointToClient(Point.Round(v[0].Value));
                endPoint = target.Box.HostControl.Parent.PointToClient(Point.Round(v[1].Value));

                line = new LineClass(endPoint, startPoint);
                startPointF = LineClass.CalcAcrossPointF(line, box.Box.HostControl.Bounds, out startType);
                endPointF = LineClass.CalcAcrossPointF(line, target.Box.HostControl.Bounds, out endType);

                float x = 0f, y = 0f, xyoff = 4f;
                CalcRectangleCenterPoint(RectangleF.FromLTRB(startPointF.X - xyoff, startPointF.Y - xyoff, endPointF.X - xyoff, endPointF.Y - xyoff), out x, out y);
                RectangleF rect = GetObjectOutLineRectangle(new RectangleF[] {
                                                                                                    new RectangleF(startPointF.X - xyoff, startPointF.Y - xyoff, xyoff, xyoff),
                                                                                                    new RectangleF(endPointF.X - xyoff, endPointF.Y - xyoff, xyoff, xyoff) }, x, y);
                //g.DrawRectangle(Pens.Black, rect.Left, rect.Top, rect.Width, rect.Height);
                if (area.Contains(rect))
                {
                    if ((startType == LineClass.LinePosition.Left && endType == LineClass.LinePosition.Right) ||
                        (startType == LineClass.LinePosition.Right && endType == LineClass.LinePosition.Left))
                    {
                        startPointF.Y = startPointF.Y + (DrawCmdIndex * crossRate); endPointF.Y = endPointF.Y - (DrawCmdIndex * crossRate);
                    }
                    else
                    {
                        startPointF.X = startPointF.X + (DrawCmdIndex * crossRate); endPointF.X = endPointF.X - (DrawCmdIndex * crossRate);
                    }

                    g.DrawLine(linePan, startPointF.X, startPointF.Y, endPointF.X, endPointF.Y);

                    DrawString(box, LineText, g, linePan);
                }
            }
        }

        /// <summary>
        /// 점 추적 없이. 박스 사각형 경계선까지만 체크 
        /// </summary>
        /// <param name="box"></param>
        /// <param name="LineText"></param>
        /// <param name="g"></param>
        /// <param name="linePan"></param>
        /// <param name="target"></param>
        /// <param name="area"></param>
        /// <param name="DrawCmdIndex"></param>
        public static void DrawCrossLine(this JSFW_BoxInCircle box, string LineText, Graphics g, Pen linePan, JSFW_BoxInCircle target, RectangleF area, float DrawCmdIndex = 0)
        {
            //그려지는 컨트롤에 Paint event 이어야 정상동작함! 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            // 점간 계산 안하는 로직.
            startPoint = GetOrigin(box.Box.HostControl.Bounds);
            endPoint = GetOrigin(target.Box.HostControl.Bounds);


            line = new LineClass(endPoint, startPoint);
            startPointF = LineClass.CalcAcrossPointF(line, box.Box.HostControl.Bounds, out startType);
            endPointF = LineClass.CalcAcrossPointF(line, target.Box.HostControl.Bounds, out endType);

            float x = 0f, y = 0f, xyoff = 4f;
            CalcRectangleCenterPoint(RectangleF.FromLTRB(startPointF.X - xyoff, startPointF.Y - xyoff, endPointF.X - xyoff, endPointF.Y - xyoff), out x, out y);
            RectangleF rect = GetObjectOutLineRectangle(new RectangleF[] {
                                                                            new RectangleF(startPointF.X - xyoff, startPointF.Y - xyoff, xyoff, xyoff),
                                                                            new RectangleF(endPointF.X - xyoff, endPointF.Y - xyoff, xyoff, xyoff)
                                                        }, x, y);

            //g.DrawRectangle(Pens.Black, rect.Left, rect.Top, rect.Width, rect.Height);
            if (area.Contains(rect))
            {
                if ((startType == LineClass.LinePosition.Left && endType == LineClass.LinePosition.Right) ||
                    (startType == LineClass.LinePosition.Right && endType == LineClass.LinePosition.Left))
                {
                    startPointF.Y = startPointF.Y + (DrawCmdIndex * crossRate); endPointF.Y = endPointF.Y - (DrawCmdIndex * crossRate);
                }
                else
                {
                    startPointF.X = startPointF.X + (DrawCmdIndex * crossRate); endPointF.X = endPointF.X - (DrawCmdIndex * crossRate);
                }
                g.DrawLine(linePan, startPointF.X, startPointF.Y, endPointF.X, endPointF.Y);
                DrawString(box, LineText, g, linePan);
            }
        }

        /// <summary>
        /// 컨트롤 별 점간 >>> 추적
        /// </summary>
        /// <param name="box"></param>
        /// <param name="DrawControl"></param>
        /// <param name="LineText"></param>
        /// <param name="g"></param>
        /// <param name="linePan"></param>
        /// <param name="target"></param>
        /// <param name="area"></param>
        /// <param name="DrawCmdIndex"></param>
        public static void DrawCrossLine(this JSFW_BoxInCircle box, Control DrawControl, string LineText, Graphics g, Pen linePan, JSFW_BoxInCircle target, RectangleF area, float DrawCmdIndex = 0)
        {
            //그려지는 컨트롤에 Paint event 이어야 정상동작함! 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            // 점간 사이계산 
            KeyValuePair<char, PointF>[] v = box.Box.BoxInCircle.NearPointSearch(target.Box);
            if (v != null)
            {
                startPoint = DrawControl.PointToClient(Point.Round(v[0].Value));
                endPoint = DrawControl.PointToClient(Point.Round(v[1].Value));

                line = new LineClass(endPoint, startPoint);
                startPointF = LineClass.CalcAcrossPointF(line, box.Box.HostControl.Bounds, out startType);
                endPointF = LineClass.CalcAcrossPointF(line, target.Box.HostControl.Bounds, out endType);

                float x = 0f, y = 0f, xyoff = 4f;
                CalcRectangleCenterPoint(RectangleF.FromLTRB(startPointF.X - xyoff, startPointF.Y - xyoff, endPointF.X - xyoff, endPointF.Y - xyoff), out x, out y);
                RectangleF rect = GetObjectOutLineRectangle(new RectangleF[] {
                                                                            new RectangleF(startPointF.X - xyoff, startPointF.Y - xyoff, xyoff, xyoff),
                                                                            new RectangleF(endPointF.X - xyoff, endPointF.Y - xyoff, xyoff, xyoff)
                                                        }, x, y);

                //g.DrawRectangle(Pens.Black, rect.Left, rect.Top, rect.Width, rect.Height);
                if (area.Contains(rect))
                {
                    if ((startType == LineClass.LinePosition.Left && endType == LineClass.LinePosition.Right) ||
                        (startType == LineClass.LinePosition.Right && endType == LineClass.LinePosition.Left))
                    {
                        startPointF.Y = startPointF.Y + (DrawCmdIndex * crossRate); endPointF.Y = endPointF.Y - (DrawCmdIndex * crossRate);
                    }
                    else
                    {
                        startPointF.X = startPointF.X + (DrawCmdIndex * crossRate); endPointF.X = endPointF.X - (DrawCmdIndex * crossRate);
                    }
                    g.DrawLine(linePan, startPointF.X, startPointF.Y, endPointF.X, endPointF.Y);
                    DrawString(box, LineText, g, linePan);
                }
            }
            else
            {
                // 점간 계산 안하는 로직.
                startPoint = GetOrigin(box.Box.HostControl.Bounds);
                endPoint = GetOrigin(target.Box.HostControl.Bounds);


                line = new LineClass(endPoint, startPoint);
                startPointF = LineClass.CalcAcrossPointF(line, box.Box.HostControl.Bounds, out startType);
                endPointF = LineClass.CalcAcrossPointF(line, target.Box.HostControl.Bounds, out endType);

                float x = 0f, y = 0f, xyoff = 4f;
                CalcRectangleCenterPoint(RectangleF.FromLTRB(startPointF.X - xyoff, startPointF.Y - xyoff, endPointF.X - xyoff, endPointF.Y - xyoff), out x, out y);
                RectangleF rect = GetObjectOutLineRectangle(new RectangleF[] {
                                                                            new RectangleF(startPointF.X - xyoff, startPointF.Y - xyoff, xyoff, xyoff),
                                                                            new RectangleF(endPointF.X - xyoff, endPointF.Y - xyoff, xyoff, xyoff)
                                                        }, x, y);

                //g.DrawRectangle(Pens.Black, rect.Left, rect.Top, rect.Width, rect.Height);
                if (area.Contains(rect))
                {
                    if ((startType == LineClass.LinePosition.Left && endType == LineClass.LinePosition.Right) ||
                        (startType == LineClass.LinePosition.Right && endType == LineClass.LinePosition.Left))
                    {
                        startPointF.Y = startPointF.Y + (DrawCmdIndex * crossRate); endPointF.Y = endPointF.Y - (DrawCmdIndex * crossRate);
                    }
                    else
                    {
                        startPointF.X = startPointF.X + (DrawCmdIndex * crossRate); endPointF.X = endPointF.X - (DrawCmdIndex * crossRate);
                    }
                    g.DrawLine(linePan, startPointF.X, startPointF.Y, endPointF.X, endPointF.Y);
                    DrawString(box, LineText, g, linePan);
                }

            }
        }


        private static void DrawString(JSFW_BoxInCircle box, string LineText, Graphics g, Pen linePan)
        {
            float angle = getTextAngle(startPointF, endPointF);

            SizeF TextSizeF = g.MeasureString(LineText, box.Box.HostControl.Font);
            int xOffset = 10;
            int yOffset = 10;

            if (startType == LineClass.LinePosition.Left && endType == LineClass.LinePosition.Right)
            {
                xOffset *= -1;
                xOffset -= 0;
                yOffset -= (120f <= angle && angle <= 270f) ? (int)(angle / 8f) : 0;
                yOffset *= -1;
            }
            else if (startType == LineClass.LinePosition.Right && endType == LineClass.LinePosition.Left)
            {
                xOffset *= -1;
                xOffset += 20;
                yOffset += (0f <= angle && angle <= 90f) ? (int)(angle / 4f) : 0;
                yOffset *= -1;
            }
            else if (startType == LineClass.LinePosition.Bottom && (endType == LineClass.LinePosition.Top || endType == LineClass.LinePosition.Right || endType == LineClass.LinePosition.Left))
            {
                xOffset *= -1;
                xOffset += (40f <= angle && angle <= 165f) ? -(int)(angle / (166f - angle)) : 0;
                yOffset -= 0;
                yOffset *= -1;
            }
            else if (startType == LineClass.LinePosition.Right && (endType == LineClass.LinePosition.Top || endType == LineClass.LinePosition.Bottom))
            {
                xOffset *= -1;
                xOffset += 20;
                yOffset += 5;
                yOffset *= -1;
            }
            else if (startType == LineClass.LinePosition.Top && (endType == LineClass.LinePosition.Bottom || endType == LineClass.LinePosition.Right || endType == LineClass.LinePosition.Left))
            {
                xOffset *= -1;
                int xOffet2 = 0;
                if (270f <= angle && angle < 290) xOffet2 += 3;
                else if (290f <= angle && angle < 300) xOffet2 += 6;
                else if (300f <= angle && angle < 310) xOffet2 += 9;
                else if (310f <= angle && angle < 320) xOffet2 += 12;
                else if (320f <= angle && angle < 330) xOffet2 += 15;
                else if (330f <= angle && angle < 340) xOffet2 += 18;
                else if (340f <= angle && angle < 350) xOffet2 += 21;

                xOffset += 20 + xOffet2;

                int yOffet2 = 0;
                if (270f <= angle && angle < 290) yOffet2 += 1;
                else if (290f <= angle && angle < 300) yOffet2 += 2;
                else if (300f <= angle && angle < 310) yOffet2 += 3;
                else if (310f <= angle && angle < 320) yOffet2 += 4;
                else if (320f <= angle && angle < 330) yOffet2 += 5;
                else if (330f <= angle && angle < 340) yOffet2 += 6;
                else if (340f <= angle && angle < 350) yOffet2 += 7;

                yOffset -= 20 - yOffet2;
                yOffset *= -1;
            }
            g.RotateTransform(angle, System.Drawing.Drawing2D.MatrixOrder.Append);
            g.TranslateTransform(startPointF.X + xOffset, startPointF.Y - yOffset, System.Drawing.Drawing2D.MatrixOrder.Append);
            g.DrawString(LineText, box.Box.HostControl.Font, linePan.Brush, new Point());
            g.ResetTransform();
        }
    }

}
