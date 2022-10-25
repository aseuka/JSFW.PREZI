using System;
using System.Drawing;
using System.Windows.Forms;

namespace JSFW.PREZI
{
    public partial class Edit_ToolBar : UserControl
    {
        public event EventHandler<ToolbarChangedValueEvent> DataChanged = null;

        IATContent Content { get; set; }

        bool IsDataBinding = false;

        public Edit_ToolBar()
        {
            InitializeComponent();
            this.Disposed += Edit_ToolBar_Disposed;
        }

        private void Edit_ToolBar_Disposed(object sender, EventArgs e)
        {
            Content = null;
        }

        public void SetContent(IATContent content)
        {
            Content = content;
            this.Enabled = false;
            DataClear();

            if (content is AT_TextControl)
            {
                this.Enabled = true;
            }
            else if (content is AT_GroupControl)
            {
                this.Enabled = true;
            }
            else {
                return;
            }
            DataBind();
        }

        private void DataClear()
        {
            try
            {
                IsDataBinding = true;

                udFontSize.Value = 9;
                chkBold.Checked = false;
                chkUnderLine.Checked = false;
                 
                initBold();
                initUnderLine();

                rdoAlign_Left.Checked = true;
                rdoAlign_Center.Checked = false;
                rdoAlign_Right.Checked = false;

                rdoBorder_None.Checked = true;
                rdoBorder_Solid.Checked = false;
                rdoBorder_Dot.Checked = false;
            }
            finally
            {
                IsDataBinding = false;
            }
        }

        private void initUnderLine()
        {
            if (chkUnderLine.Checked)
            {
                // 밑줄 추가
                chkUnderLine.Font = new Font(chkUnderLine.Font, chkUnderLine.Font.Style | FontStyle.Underline);
            }
            else
            {
                // 밑줄 제거
                chkUnderLine.Font = new Font(chkUnderLine.Font, chkUnderLine.Font.Style ^ FontStyle.Underline);
            }
        }

        private void initBold()
        {
            if (chkBold.Checked)
            {
                // 진하게 추가
                chkBold.Font = new Font(chkBold.Font, chkBold.Font.Style | FontStyle.Bold);
            }
            else
            {
                // 진하게 제거
                chkBold.Font = new Font(chkBold.Font, chkBold.Font.Style ^ FontStyle.Bold);
            }
        }

        private void DataBind()
        {
            try
            {
                IsDataBinding = true;
                // 폰트 크기
                if (Content != null)
                {
                    if (Content.Data.FontSize < 8) udFontSize.Value = 9; // default 값 설정. 
                    else udFontSize.Value = Content.Data.FontSize.To<int>(9);

                    chkBold.Checked = Content.Data.FontBold;
                    chkUnderLine.Checked = Content.Data.FontUnderLine;

                    rdoAlign_Left.Checked = false;
                    rdoAlign_Center.Checked = false;
                    rdoAlign_Right.Checked = false; 
                    switch (Content.Data.FontAlign)
                    {
                        default:
                        case HorizontalAlignment.Left:
                            rdoAlign_Left.Checked = true;
                            break;

                        case HorizontalAlignment.Center:
                            rdoAlign_Center.Checked = true;
                            break;

                        case HorizontalAlignment.Right:
                            rdoAlign_Right.Checked = true;
                            break;
                    }

                    cpForeColor.SelectedColor = ColorTranslator.FromHtml( Content.Data.ForeColor );
                    cpBackColor.SelectedColor = ColorTranslator.FromHtml( Content.Data.BackColor );

                    rdoBorder_None.Checked = false;
                    rdoBorder_Solid.Checked = false;
                    rdoBorder_Dot.Checked = false;

                    switch (Content.Data.Border)
                    {
                        default:
                        case AT_Border.NONE:
                            rdoBorder_None.Checked = true;
                            break;

                        case AT_Border.SOLID:
                            rdoBorder_Solid.Checked = true;
                            break;

                        case AT_Border.DOT:
                            rdoBorder_Dot.Checked = true;
                            break; 
                    }
                   
                }
            }
            finally
            {
                initBold();
                initUnderLine();
                IsDataBinding = false;
            }
            
        }

        private void udFontSize_ValueChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
        }

        private void OnDataChange()
        {
            if (DataChanged != null)
            {
                ToolbarChangedValueEvent args = new ToolbarChangedValueEvent(
                        Convert.ToInt32( udFontSize.Value ),
                        chkBold.Checked,
                        chkUnderLine.Checked,
                        GetTextAlign(),
                        cpForeColor.SelectedColor,
                        cpBackColor.SelectedColor,
                        GetBorder()
                    );
                DataChanged(this, args);
                args = null;
            }
        }

        private AT_Border GetBorder()
        {
            AT_Border border = AT_Border.SOLID;
            if (rdoBorder_Dot.Checked)
            {
                border = AT_Border.DOT;
            }
            else if (rdoBorder_None.Checked)
            {
                border = AT_Border.NONE;
            }
            return border;
        }

        private HorizontalAlignment GetTextAlign()
        {
            HorizontalAlignment align = HorizontalAlignment.Left;
            if (rdoAlign_Center.Checked)
            {
                align = HorizontalAlignment.Center;
            }
            else if (rdoAlign_Right.Checked)
            {
                align = HorizontalAlignment.Right;
            }
            return align;
        }

        private void chkBold_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
            initBold();
        }

        private void chkUnderLine_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
            initUnderLine();
        }

        private void rdoAlign_Left_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();

        }

        private void rdoAlign_Center_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
        }

        private void rdoAlign_Right_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
        }

        private void cpForeColor_SelectedColorChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
        }

        private void cpBackColor_SelectedColorChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
        }

        private void rdoBorder_None_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
        }

        private void rdoBorder_Solid_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
        }

        private void rdoBorder_Dot_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDataBinding) return;
            OnDataChange();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // front
            if (Content != null && Content is Control)
            {
                ((Control)Content).BringToFront();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // back
            if (Content != null && Content is Control)
            {
                ((Control)Content).SendToBack();
            }
        }

        internal void ChangeFont(decimal fontSz)
        {
            decimal beforeFontSize = udFontSize.Value;
            try
            { 
                udFontSize.Value = beforeFontSize + fontSz;
            }
            catch {
                udFontSize.Value = beforeFontSize;
            }
        }

        internal void ToggleFontBold()
        {
            chkBold.Checked = !chkBold.Checked;
        }
    }

    public class ToolbarChangedValueEvent : EventArgs
    {
        public int FontSize { get; protected set; }
        public bool FontBold { get; protected set; }
        public bool FontUnderLine { get; protected set; }
        public HorizontalAlignment TextAlign { get; protected set; }
        public Color ForeColor { get; protected set; }
        public Color BackColor { get; protected set; }
        public AT_Border Border { get; protected set; }

        public ToolbarChangedValueEvent(
            int fontsize, 
            bool fontbold,
            bool fontunderline,
            HorizontalAlignment textalign,
            Color forecolor, 
            Color backcolor,
            AT_Border border
            ) : base()
        {
            FontSize = fontsize;
            FontBold = fontbold;
            FontUnderLine = fontunderline;
            TextAlign = textalign;
            ForeColor = forecolor;
            BackColor = backcolor;
            Border = border;
        }
    }
}
