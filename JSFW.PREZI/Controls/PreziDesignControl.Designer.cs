namespace JSFW.PREZI.Controls
{
    partial class PreziDesignControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreziDesignControl));
            this.BackGroundPanel = new System.Windows.Forms.Panel();
            this.transparent_Panel1 = new JSFW.PREZI.Controls.Transparent_Panel();
            this.designPanel1 = new JSFW.PREZI.DesignPanel();
            this.txtDataClassDesc = new System.Windows.Forms.TextBox();
            this.DescriptionPanel = new System.Windows.Forms.Panel();
            this.pnlFrame = new System.Windows.Forms.Panel();
            this.lbToolBoxItem_Frame = new JSFW.PREZI.Controls.Label();
            this.chkFrameView = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lbItemDel = new JSFW.PREZI.Controls.Label();
            this.lbToolBoxItem_File = new JSFW.PREZI.Controls.Label();
            this.lbToolBoxItem_Image = new JSFW.PREZI.Controls.Label();
            this.lbToolBoxItem_Text = new JSFW.PREZI.Controls.Label();
            this.chkContentView = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbBackgroundImg = new JSFW.PREZI.Controls.PaintLabel();
            this.lbBrush_Yellow = new JSFW.PREZI.Controls.PaintLabel();
            this.lbBrush_Blue = new JSFW.PREZI.Controls.PaintLabel();
            this.lbBrush_Red = new JSFW.PREZI.Controls.PaintLabel();
            this.lbBrush_Black = new JSFW.PREZI.Controls.PaintLabel();
            this.label6 = new JSFW.PREZI.Controls.Label();
            this.lbPenSize_8 = new JSFW.PREZI.Controls.PaintLabel();
            this.lbPenSize_3 = new JSFW.PREZI.Controls.PaintLabel();
            this.label1 = new JSFW.PREZI.Controls.Label();
            this.lbErase = new JSFW.PREZI.Controls.Label();
            this.lbPen = new JSFW.PREZI.Controls.Label();
            this.lbCursor = new JSFW.PREZI.Controls.Label();
            this.pnlBringBackShort = new System.Windows.Forms.Panel();
            this.btnFront = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFadeClose = new System.Windows.Forms.Button();
            this.edit_ToolBar1 = new JSFW.PREZI.Edit_ToolBar();
            this.chkDesignRunMode = new System.Windows.Forms.CheckBox();
            this.BackGroundPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.designPanel1)).BeginInit();
            this.DescriptionPanel.SuspendLayout();
            this.pnlFrame.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlBringBackShort.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BackGroundPanel
            // 
            this.BackGroundPanel.AutoScroll = true;
            this.BackGroundPanel.BackColor = System.Drawing.Color.Transparent;
            this.BackGroundPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackGroundPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BackGroundPanel.Controls.Add(this.transparent_Panel1);
            this.BackGroundPanel.Controls.Add(this.designPanel1);
            this.BackGroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackGroundPanel.Location = new System.Drawing.Point(27, 32);
            this.BackGroundPanel.Name = "BackGroundPanel";
            this.BackGroundPanel.Padding = new System.Windows.Forms.Padding(3);
            this.BackGroundPanel.Size = new System.Drawing.Size(1109, 606);
            this.BackGroundPanel.TabIndex = 10;
            // 
            // transparent_Panel1
            // 
            this.transparent_Panel1.AllowDrop = true;
            this.transparent_Panel1.BackColor = System.Drawing.Color.Transparent;
            this.transparent_Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.transparent_Panel1.Location = new System.Drawing.Point(544, 6);
            this.transparent_Panel1.Name = "transparent_Panel1";
            this.transparent_Panel1.Size = new System.Drawing.Size(558, 592);
            this.transparent_Panel1.TabIndex = 0;
            this.transparent_Panel1.OpenPreziDesignForm += new System.Action<JSFW.PREZI.Controls.Frame>(this.transparent_Panel1_OpenPreziDesignForm);
            // 
            // designPanel1
            // 
            this.designPanel1.AllowDrop = true;
            this.designPanel1.BackColor = System.Drawing.Color.Transparent;
            this.designPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.designPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.designPanel1.CurrentLinkLineID = null;
            this.designPanel1.Image = ((System.Drawing.Image)(resources.GetObject("designPanel1.Image")));
            this.designPanel1.Location = new System.Drawing.Point(5, 6);
            this.designPanel1.MinimumSize = new System.Drawing.Size(10, 10);
            this.designPanel1.Name = "designPanel1";
            this.designPanel1.Size = new System.Drawing.Size(538, 592);
            this.designPanel1.TabIndex = 1;
            this.designPanel1.TabStop = false;
            this.designPanel1.OpenViewAttributes += new System.Action<JSFW.PREZI.IATContent, JSFW.PREZI.ATEditStateEnum>(this.designPanel1_OpenViewAttributes);
            this.designPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.designPanel1_Paint);
            // 
            // txtDataClassDesc
            // 
            this.txtDataClassDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDataClassDesc.Location = new System.Drawing.Point(0, 2);
            this.txtDataClassDesc.Multiline = true;
            this.txtDataClassDesc.Name = "txtDataClassDesc";
            this.txtDataClassDesc.Size = new System.Drawing.Size(1109, 48);
            this.txtDataClassDesc.TabIndex = 0;
            this.txtDataClassDesc.TextChanged += new System.EventHandler(this.txtDataClassDesc_TextChanged);
            // 
            // DescriptionPanel
            // 
            this.DescriptionPanel.BackColor = System.Drawing.Color.Transparent;
            this.DescriptionPanel.Controls.Add(this.txtDataClassDesc);
            this.DescriptionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DescriptionPanel.Location = new System.Drawing.Point(27, 638);
            this.DescriptionPanel.Name = "DescriptionPanel";
            this.DescriptionPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.DescriptionPanel.Size = new System.Drawing.Size(1109, 52);
            this.DescriptionPanel.TabIndex = 9;
            // 
            // pnlFrame
            // 
            this.pnlFrame.BackColor = System.Drawing.Color.Transparent;
            this.pnlFrame.Controls.Add(this.lbToolBoxItem_Frame);
            this.pnlFrame.Controls.Add(this.chkFrameView);
            this.pnlFrame.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlFrame.Location = new System.Drawing.Point(1136, 32);
            this.pnlFrame.Name = "pnlFrame";
            this.pnlFrame.Size = new System.Drawing.Size(28, 658);
            this.pnlFrame.TabIndex = 8;
            // 
            // lbToolBoxItem_Frame
            // 
            this.lbToolBoxItem_Frame.BackColor = System.Drawing.Color.LightCoral;
            this.lbToolBoxItem_Frame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbToolBoxItem_Frame.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbToolBoxItem_Frame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbToolBoxItem_Frame.ForeColor = System.Drawing.Color.White;
            this.lbToolBoxItem_Frame.Location = new System.Drawing.Point(0, 24);
            this.lbToolBoxItem_Frame.Name = "lbToolBoxItem_Frame";
            this.lbToolBoxItem_Frame.Size = new System.Drawing.Size(28, 55);
            this.lbToolBoxItem_Frame.TabIndex = 9;
            this.lbToolBoxItem_Frame.Text = "프레임";
            this.lbToolBoxItem_Frame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbToolBoxItem_Frame.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_Frame_MouseDown);
            this.lbToolBoxItem_Frame.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_Frame_MouseMove);
            this.lbToolBoxItem_Frame.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_Frame_MouseUp);
            // 
            // chkFrameView
            // 
            this.chkFrameView.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkFrameView.BackgroundImage = global::JSFW.PREZI.Properties.Resources.FrameView;
            this.chkFrameView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chkFrameView.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkFrameView.FlatAppearance.BorderSize = 0;
            this.chkFrameView.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCoral;
            this.chkFrameView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkFrameView.Location = new System.Drawing.Point(0, 0);
            this.chkFrameView.Name = "chkFrameView";
            this.chkFrameView.Size = new System.Drawing.Size(28, 24);
            this.chkFrameView.TabIndex = 10;
            this.chkFrameView.UseVisualStyleBackColor = true;
            this.chkFrameView.CheckedChanged += new System.EventHandler(this.chkFrameView_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 32);
            this.label4.TabIndex = 30;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.Transparent;
            this.pnlContent.Controls.Add(this.lbItemDel);
            this.pnlContent.Controls.Add(this.lbToolBoxItem_File);
            this.pnlContent.Controls.Add(this.lbToolBoxItem_Image);
            this.pnlContent.Controls.Add(this.lbToolBoxItem_Text);
            this.pnlContent.Controls.Add(this.chkContentView);
            this.pnlContent.Controls.Add(this.panel2);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlContent.Location = new System.Drawing.Point(0, 32);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(27, 658);
            this.pnlContent.TabIndex = 7;
            // 
            // lbItemDel
            // 
            this.lbItemDel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbItemDel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbItemDel.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbItemDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbItemDel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbItemDel.Image = global::JSFW.PREZI.Properties.Resources.X;
            this.lbItemDel.Location = new System.Drawing.Point(0, 187);
            this.lbItemDel.Name = "lbItemDel";
            this.lbItemDel.Size = new System.Drawing.Size(27, 27);
            this.lbItemDel.TabIndex = 10;
            this.lbItemDel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbItemDel.Click += new System.EventHandler(this.lbItemDel_Click);
            // 
            // lbToolBoxItem_File
            // 
            this.lbToolBoxItem_File.BackColor = System.Drawing.Color.PaleGreen;
            this.lbToolBoxItem_File.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbToolBoxItem_File.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbToolBoxItem_File.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbToolBoxItem_File.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbToolBoxItem_File.Location = new System.Drawing.Point(0, 139);
            this.lbToolBoxItem_File.Name = "lbToolBoxItem_File";
            this.lbToolBoxItem_File.Size = new System.Drawing.Size(27, 48);
            this.lbToolBoxItem_File.TabIndex = 7;
            this.lbToolBoxItem_File.Text = "파일";
            this.lbToolBoxItem_File.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbToolBoxItem_File.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_File_MouseDown);
            this.lbToolBoxItem_File.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_File_MouseMove);
            this.lbToolBoxItem_File.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_File_MouseUp);
            // 
            // lbToolBoxItem_Image
            // 
            this.lbToolBoxItem_Image.BackColor = System.Drawing.Color.PaleGreen;
            this.lbToolBoxItem_Image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbToolBoxItem_Image.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbToolBoxItem_Image.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbToolBoxItem_Image.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbToolBoxItem_Image.Location = new System.Drawing.Point(0, 83);
            this.lbToolBoxItem_Image.Name = "lbToolBoxItem_Image";
            this.lbToolBoxItem_Image.Size = new System.Drawing.Size(27, 56);
            this.lbToolBoxItem_Image.TabIndex = 6;
            this.lbToolBoxItem_Image.Text = "이미지";
            this.lbToolBoxItem_Image.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbToolBoxItem_Image.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_Image_MouseDown);
            this.lbToolBoxItem_Image.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_Image_MouseMove);
            this.lbToolBoxItem_Image.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_Image_MouseUp);
            // 
            // lbToolBoxItem_Text
            // 
            this.lbToolBoxItem_Text.BackColor = System.Drawing.Color.PaleGreen;
            this.lbToolBoxItem_Text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbToolBoxItem_Text.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbToolBoxItem_Text.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbToolBoxItem_Text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbToolBoxItem_Text.Location = new System.Drawing.Point(0, 24);
            this.lbToolBoxItem_Text.Name = "lbToolBoxItem_Text";
            this.lbToolBoxItem_Text.Size = new System.Drawing.Size(27, 59);
            this.lbToolBoxItem_Text.TabIndex = 5;
            this.lbToolBoxItem_Text.Text = "텍스트";
            this.lbToolBoxItem_Text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbToolBoxItem_Text.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_Text_MouseDown);
            this.lbToolBoxItem_Text.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_Text_MouseMove);
            this.lbToolBoxItem_Text.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbToolBoxItem_Text_MouseUp);
            // 
            // chkContentView
            // 
            this.chkContentView.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkContentView.BackgroundImage = global::JSFW.PREZI.Properties.Resources.ContentView;
            this.chkContentView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chkContentView.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkContentView.FlatAppearance.BorderSize = 0;
            this.chkContentView.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.chkContentView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkContentView.Location = new System.Drawing.Point(0, 0);
            this.chkContentView.Name = "chkContentView";
            this.chkContentView.Size = new System.Drawing.Size(27, 24);
            this.chkContentView.TabIndex = 8;
            this.chkContentView.UseVisualStyleBackColor = true;
            this.chkContentView.CheckedChanged += new System.EventHandler(this.chkContentView_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbBackgroundImg);
            this.panel2.Controls.Add(this.lbBrush_Yellow);
            this.panel2.Controls.Add(this.lbBrush_Blue);
            this.panel2.Controls.Add(this.lbBrush_Red);
            this.panel2.Controls.Add(this.lbBrush_Black);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.lbPenSize_8);
            this.panel2.Controls.Add(this.lbPenSize_3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lbErase);
            this.panel2.Controls.Add(this.lbPen);
            this.panel2.Controls.Add(this.lbCursor);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 246);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(27, 412);
            this.panel2.TabIndex = 9;
            // 
            // lbBackgroundImg
            // 
            this.lbBackgroundImg.BackColor = System.Drawing.Color.Ivory;
            this.lbBackgroundImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBackgroundImg.BrushColor = System.Drawing.Color.Transparent;
            this.lbBackgroundImg.BrushWidth = 16F;
            this.lbBackgroundImg.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBackgroundImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBackgroundImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbBackgroundImg.Image = global::JSFW.PREZI.Properties.Resources.bgImg;
            this.lbBackgroundImg.IsElipse = false;
            this.lbBackgroundImg.Location = new System.Drawing.Point(0, 253);
            this.lbBackgroundImg.Name = "lbBackgroundImg";
            this.lbBackgroundImg.Size = new System.Drawing.Size(27, 27);
            this.lbBackgroundImg.TabIndex = 20;
            this.lbBackgroundImg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBackgroundImg.Click += new System.EventHandler(this.lbBackgroundImg_Click);
            // 
            // lbBrush_Yellow
            // 
            this.lbBrush_Yellow.BackColor = System.Drawing.Color.Ivory;
            this.lbBrush_Yellow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBrush_Yellow.BrushColor = System.Drawing.Color.Yellow;
            this.lbBrush_Yellow.BrushWidth = 16F;
            this.lbBrush_Yellow.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBrush_Yellow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBrush_Yellow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbBrush_Yellow.IsElipse = false;
            this.lbBrush_Yellow.Location = new System.Drawing.Point(0, 226);
            this.lbBrush_Yellow.Name = "lbBrush_Yellow";
            this.lbBrush_Yellow.Size = new System.Drawing.Size(27, 27);
            this.lbBrush_Yellow.TabIndex = 19;
            this.lbBrush_Yellow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBrush_Yellow.Click += new System.EventHandler(this.lbBrush_Yellow_Click);
            // 
            // lbBrush_Blue
            // 
            this.lbBrush_Blue.BackColor = System.Drawing.Color.Ivory;
            this.lbBrush_Blue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBrush_Blue.BrushColor = System.Drawing.Color.Blue;
            this.lbBrush_Blue.BrushWidth = 16F;
            this.lbBrush_Blue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBrush_Blue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBrush_Blue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbBrush_Blue.IsElipse = false;
            this.lbBrush_Blue.Location = new System.Drawing.Point(0, 199);
            this.lbBrush_Blue.Name = "lbBrush_Blue";
            this.lbBrush_Blue.Size = new System.Drawing.Size(27, 27);
            this.lbBrush_Blue.TabIndex = 18;
            this.lbBrush_Blue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBrush_Blue.Click += new System.EventHandler(this.lbBrush_Blue_Click);
            // 
            // lbBrush_Red
            // 
            this.lbBrush_Red.BackColor = System.Drawing.Color.Ivory;
            this.lbBrush_Red.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBrush_Red.BrushColor = System.Drawing.Color.Red;
            this.lbBrush_Red.BrushWidth = 16F;
            this.lbBrush_Red.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBrush_Red.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBrush_Red.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbBrush_Red.IsElipse = false;
            this.lbBrush_Red.Location = new System.Drawing.Point(0, 172);
            this.lbBrush_Red.Name = "lbBrush_Red";
            this.lbBrush_Red.Size = new System.Drawing.Size(27, 27);
            this.lbBrush_Red.TabIndex = 17;
            this.lbBrush_Red.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBrush_Red.Click += new System.EventHandler(this.lbBrush_Red_Click);
            // 
            // lbBrush_Black
            // 
            this.lbBrush_Black.BackColor = System.Drawing.Color.Ivory;
            this.lbBrush_Black.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBrush_Black.BrushColor = System.Drawing.Color.Black;
            this.lbBrush_Black.BrushWidth = 16F;
            this.lbBrush_Black.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbBrush_Black.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbBrush_Black.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbBrush_Black.IsElipse = false;
            this.lbBrush_Black.Location = new System.Drawing.Point(0, 145);
            this.lbBrush_Black.Name = "lbBrush_Black";
            this.lbBrush_Black.Size = new System.Drawing.Size(27, 27);
            this.lbBrush_Black.TabIndex = 15;
            this.lbBrush_Black.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBrush_Black.Click += new System.EventHandler(this.lbBrush_Black_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Ivory;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(0, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 5);
            this.label6.TabIndex = 16;
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPenSize_8
            // 
            this.lbPenSize_8.BackColor = System.Drawing.Color.Ivory;
            this.lbPenSize_8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPenSize_8.BrushColor = System.Drawing.Color.Black;
            this.lbPenSize_8.BrushWidth = 10F;
            this.lbPenSize_8.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbPenSize_8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbPenSize_8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbPenSize_8.IsElipse = true;
            this.lbPenSize_8.Location = new System.Drawing.Point(0, 113);
            this.lbPenSize_8.Name = "lbPenSize_8";
            this.lbPenSize_8.Size = new System.Drawing.Size(27, 27);
            this.lbPenSize_8.TabIndex = 14;
            this.lbPenSize_8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbPenSize_8.Click += new System.EventHandler(this.lbPenSize_8_Click);
            // 
            // lbPenSize_3
            // 
            this.lbPenSize_3.BackColor = System.Drawing.Color.Ivory;
            this.lbPenSize_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPenSize_3.BrushColor = System.Drawing.Color.Black;
            this.lbPenSize_3.BrushWidth = 6F;
            this.lbPenSize_3.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbPenSize_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbPenSize_3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbPenSize_3.IsElipse = true;
            this.lbPenSize_3.Location = new System.Drawing.Point(0, 86);
            this.lbPenSize_3.Name = "lbPenSize_3";
            this.lbPenSize_3.Size = new System.Drawing.Size(27, 27);
            this.lbPenSize_3.TabIndex = 13;
            this.lbPenSize_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbPenSize_3.Click += new System.EventHandler(this.lbPenSize_3_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Ivory;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(0, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 5);
            this.label1.TabIndex = 12;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbErase
            // 
            this.lbErase.BackColor = System.Drawing.Color.Ivory;
            this.lbErase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbErase.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbErase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbErase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbErase.Image = global::JSFW.PREZI.Properties.Resources.erase;
            this.lbErase.Location = new System.Drawing.Point(0, 54);
            this.lbErase.Name = "lbErase";
            this.lbErase.Size = new System.Drawing.Size(27, 27);
            this.lbErase.TabIndex = 10;
            this.lbErase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbErase.Click += new System.EventHandler(this.lbErase_Click);
            // 
            // lbPen
            // 
            this.lbPen.BackColor = System.Drawing.Color.Ivory;
            this.lbPen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPen.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbPen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbPen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbPen.Image = global::JSFW.PREZI.Properties.Resources.pen;
            this.lbPen.Location = new System.Drawing.Point(0, 27);
            this.lbPen.Name = "lbPen";
            this.lbPen.Size = new System.Drawing.Size(27, 27);
            this.lbPen.TabIndex = 9;
            this.lbPen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbPen.Click += new System.EventHandler(this.lbPen_Click);
            // 
            // lbCursor
            // 
            this.lbCursor.BackColor = System.Drawing.Color.Ivory;
            this.lbCursor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbCursor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbCursor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbCursor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lbCursor.Image = ((System.Drawing.Image)(resources.GetObject("lbCursor.Image")));
            this.lbCursor.Location = new System.Drawing.Point(0, 0);
            this.lbCursor.Name = "lbCursor";
            this.lbCursor.Size = new System.Drawing.Size(27, 27);
            this.lbCursor.TabIndex = 8;
            this.lbCursor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbCursor.Click += new System.EventHandler(this.lbCursor_Click);
            // 
            // pnlBringBackShort
            // 
            this.pnlBringBackShort.Controls.Add(this.btnFront);
            this.pnlBringBackShort.Controls.Add(this.btnCapture);
            this.pnlBringBackShort.Controls.Add(this.btnBack);
            this.pnlBringBackShort.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBringBackShort.Location = new System.Drawing.Point(842, 0);
            this.pnlBringBackShort.Name = "pnlBringBackShort";
            this.pnlBringBackShort.Size = new System.Drawing.Size(87, 32);
            this.pnlBringBackShort.TabIndex = 29;
            // 
            // btnFront
            // 
            this.btnFront.BackgroundImage = global::JSFW.PREZI.Properties.Resources.front;
            this.btnFront.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFront.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnFront.FlatAppearance.BorderSize = 0;
            this.btnFront.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFront.Location = new System.Drawing.Point(6, 6);
            this.btnFront.Name = "btnFront";
            this.btnFront.Size = new System.Drawing.Size(20, 20);
            this.btnFront.TabIndex = 28;
            this.btnFront.UseVisualStyleBackColor = true;
            this.btnFront.Click += new System.EventHandler(this.btnFront_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.White;
            this.btnCapture.BackgroundImage = global::JSFW.PREZI.Properties.Resources.Screenshot;
            this.btnCapture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCapture.FlatAppearance.BorderSize = 0;
            this.btnCapture.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapture.Location = new System.Drawing.Point(58, 6);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(21, 21);
            this.btnCapture.TabIndex = 26;
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackgroundImage = global::JSFW.PREZI.Properties.Resources.back;
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Location = new System.Drawing.Point(32, 6);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(20, 20);
            this.btnBack.TabIndex = 27;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnFadeClose);
            this.panel1.Controls.Add(this.pnlBringBackShort);
            this.panel1.Controls.Add(this.edit_ToolBar1);
            this.panel1.Controls.Add(this.chkDesignRunMode);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1164, 32);
            this.panel1.TabIndex = 6;
            // 
            // btnFadeClose
            // 
            this.btnFadeClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFadeClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnFadeClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFadeClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnFadeClose.Location = new System.Drawing.Point(1102, 5);
            this.btnFadeClose.Name = "btnFadeClose";
            this.btnFadeClose.Size = new System.Drawing.Size(56, 23);
            this.btnFadeClose.TabIndex = 32;
            this.btnFadeClose.Text = "닫기";
            this.btnFadeClose.UseVisualStyleBackColor = true;
            this.btnFadeClose.Visible = false;
            this.btnFadeClose.Click += new System.EventHandler(this.btnFadeClose_Click);
            // 
            // edit_ToolBar1
            // 
            this.edit_ToolBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.edit_ToolBar1.Location = new System.Drawing.Point(59, 0);
            this.edit_ToolBar1.MaximumSize = new System.Drawing.Size(783, 31);
            this.edit_ToolBar1.MinimumSize = new System.Drawing.Size(783, 31);
            this.edit_ToolBar1.Name = "edit_ToolBar1";
            this.edit_ToolBar1.Size = new System.Drawing.Size(783, 31);
            this.edit_ToolBar1.TabIndex = 0;
            this.edit_ToolBar1.DataChanged += new System.EventHandler<JSFW.PREZI.ToolbarChangedValueEvent>(this.edit_ToolBar1_DataChanged);
            // 
            // chkDesignRunMode
            // 
            this.chkDesignRunMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkDesignRunMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkDesignRunMode.BackgroundImage")));
            this.chkDesignRunMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chkDesignRunMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkDesignRunMode.FlatAppearance.BorderSize = 0;
            this.chkDesignRunMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDesignRunMode.Location = new System.Drawing.Point(27, 0);
            this.chkDesignRunMode.Name = "chkDesignRunMode";
            this.chkDesignRunMode.Size = new System.Drawing.Size(32, 32);
            this.chkDesignRunMode.TabIndex = 31;
            this.chkDesignRunMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDesignRunMode.UseVisualStyleBackColor = true;
            this.chkDesignRunMode.CheckedChanged += new System.EventHandler(this.chkDesignRunMode_CheckedChanged);
            // 
            // PreziDesignControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.BackGroundPanel);
            this.Controls.Add(this.DescriptionPanel);
            this.Controls.Add(this.pnlFrame);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(1007, 690);
            this.Name = "PreziDesignControl";
            this.Size = new System.Drawing.Size(1164, 690);
            this.BackGroundPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.designPanel1)).EndInit();
            this.DescriptionPanel.ResumeLayout(false);
            this.DescriptionPanel.PerformLayout();
            this.pnlFrame.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlBringBackShort.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Edit_ToolBar edit_ToolBar1;
        private Label lbToolBoxItem_File;
        private Label lbToolBoxItem_Image;
        private DesignPanel designPanel1;
        private System.Windows.Forms.Panel BackGroundPanel;
        private Transparent_Panel transparent_Panel1;
        private System.Windows.Forms.TextBox txtDataClassDesc;
        private System.Windows.Forms.Panel DescriptionPanel;
        private System.Windows.Forms.Panel pnlFrame;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlContent;
        private Label lbToolBoxItem_Text;
        private System.Windows.Forms.Panel pnlBringBackShort;
        private System.Windows.Forms.Button btnFront;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel panel1;
        private Label lbToolBoxItem_Frame;
        private System.Windows.Forms.CheckBox chkContentView;
        private System.Windows.Forms.CheckBox chkFrameView;
        private System.Windows.Forms.CheckBox chkDesignRunMode;
        private System.Windows.Forms.Button btnFadeClose;
        private System.Windows.Forms.Panel panel2;
        private Label lbPen;
        private Label lbCursor;
        private Label lbErase;
        private Label label1;
        private PaintLabel lbBrush_Black;
        private PaintLabel lbPenSize_8;
        private PaintLabel lbPenSize_3;
        private Label label6;
        private PaintLabel lbBrush_Yellow;
        private PaintLabel lbBrush_Blue;
        private PaintLabel lbBrush_Red;
        private Label lbItemDel;
        private PaintLabel lbBackgroundImg;
    }
}
