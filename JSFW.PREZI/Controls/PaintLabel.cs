using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSFW.PREZI.Controls
{
    public class PaintLabel : Label
    {
        bool _IsElipse = false;
        public bool IsElipse { get { return _IsElipse; } set { _IsElipse = value; Refresh(); } }

        RectangleF Box = new RectangleF();
        float _BrushWidth = 3;
        public float BrushWidth
        {
            get { return _BrushWidth; }
            set
            {
                _BrushWidth = value;
                ReCalcBoxing();
                Refresh();
            }
        }

        private void ReCalcBoxing()
        {
            float x = (float)DisplayRectangle.Width / 2f;
            float y = (float)DisplayRectangle.Height / 2f;
            x = x - _BrushWidth / 2f;
            y = y - _BrushWidth / 2f;
            Box.X = x;
            Box.Y = y;
            Box.Width = _BrushWidth;
            Box.Height = _BrushWidth;
        }

        Brush _Brush { get; set; } = Brushes.Black;
        Color _BrushColor = Color.Black;
        public Color BrushColor
        {
            get { return _BrushColor; }
            set
            {
                _BrushColor = value;
                if (_Brush != null)
                {
                    _Brush.Dispose();
                    _Brush = null;
                }
                _Brush = new SolidBrush(_BrushColor);
                Refresh();
            }
        }

        public PaintLabel()
        {
            ReCalcBoxing(); 
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ReCalcBoxing();
            Refresh();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e); 
            Text = "";
            ReCalcBoxing();
            Refresh();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ReCalcBoxing();
            Refresh(); 
        }

        protected override void OnAutoSizeChanged(EventArgs e)
        { 
            base.OnAutoSizeChanged(e);
            if (AutoSize == false)
            {
                ReCalcBoxing();
                Refresh();
            } 
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (AutoSize) return;
            if (IsElipse)
            {
                e.Graphics.FillEllipse(_Brush, Box);
            }
            else
            {
                e.Graphics.FillRectangle(_Brush, Box);
            }
        }
    }
}
