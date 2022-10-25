using System;
using System.Windows.Forms;

namespace JSFW.PREZI.Editor
{
    public partial class Editor_AT_TextControl : UserControl
    {
        bool IsComplite = false;
        public event Action<bool> EditComplited = null;

        public override string Text
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                base.Text = textBox1.Text = value;
                IsComplite = false;
            }
        }

        public Editor_AT_TextControl()
        {
            InitializeComponent();
             
            textBox1.TextChanged += TextBox1_TextChanged;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            CalcHeight();
            CalcWidth();
        }

        private void CalcWidth()
        {
            int w = TextRenderer.MeasureText(textBox1.Text, textBox1.Font).Width + 13;
            if ((260) < w)
            {
                this.Width = w;
            }
            else
            {
                this.Width = 260;
            }
        }

        void CalcHeight()
        {
            int h = TextRenderer.MeasureText(textBox1.Text, textBox1.Font).Height + 13;
            if ((21 * 5) < h)
            {
                this.Height = h;
            }
            else
            {
                this.Height = 21 * 5;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            textBox1.Focus();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (EditComplited != null) EditComplited(IsComplite);
        }
        
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Control)
            {
                // 적용.
                IsComplite = true;
                OnLeave(e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                IsComplite = false;
                OnLeave(e);
            }
        }
    }
}
