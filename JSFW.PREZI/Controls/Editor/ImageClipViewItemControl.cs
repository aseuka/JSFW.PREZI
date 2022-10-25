using System;
using System.Windows.Forms;
using JSFW.PREZI.Controls;

namespace JSFW.PREZI.Editor
{
    public partial class ImageClipViewItemControl : UserControl
    {
        internal ClipImage Clip { get; set; }

        public event EventHandler<ItemsSelectedEventArgs<ImageClipViewItemControl>> ItemSelected = null;
        public event EventHandler<ItemsSelectedEventArgs<ImageClipViewItemControl>> ItemDbClick = null;

        public ImageClipViewItemControl()
        {
            InitializeComponent(); 
            this.Disposed += ImageClipViewItemControl_Disposed;

            pictureBox1.MouseClick += PictureBox1_MouseClick;
            label1.MouseClick += Label1_MouseClick;
            pictureBox1.MouseDoubleClick += PictureBox1_MouseDoubleClick;
            label1.MouseDoubleClick += Label1_MouseDoubleClick;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (checkBox1.Visible)
                checkBox1.Checked = !checkBox1.Checked;

            if (ItemSelected != null)
            {
                using (var args = new ItemsSelectedEventArgs<ImageClipViewItemControl>(this))
                {
                    ItemSelected(this, args);
                }
            }
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClick(e);
        }
         
        private void Label1_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClick(e);
        }


        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (ItemDbClick != null)
            {
                using (var args = new ItemsSelectedEventArgs<ImageClipViewItemControl>(this))
                {
                    ItemDbClick(this, args);
                }
            }
        }
         
        private void Label1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnMouseDoubleClick(e);
        }

        private void PictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnMouseDoubleClick(e);
        }

        private void ImageClipViewItemControl_Disposed(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = null;
        }

        public void SetClipImage(ClipImage img)
        {
            Clip = img;
            DataBind();
        }

        private void DataBind()
        {
            DataClear();
            if (Clip != null)
            {
                label1.Text = Clip.Name;
                // 이미지를 복사 후... 
                string temp = System.IO.Path.GetTempFileName();
                //temp = System.IO.Path.Combine(temp, System.IO.Path.GetFileName(Clip.ImageLocation));
                if (System.IO.File.Exists(Clip.ImageLocation))
                {
                    System.IO.File.Copy(Clip.ImageLocation, temp, true);
                } 
                pictureBox1.ImageLocation = temp;
            }
        }

        private void DataClear()
        {
            pictureBox1.ImageLocation = null;
            label1.Text = "";
        }
        
        #region 삭제 처리용.  
        public bool IsDeleteSelected { get { return checkBox1.Checked; } }

        public void ShowDeleteCheckBox()
        {
            checkBox1.Checked = false;
            checkBox1.Visible = true;
        }

        public void HideDeleteCheckBox()
        {
            checkBox1.Checked = false;
            checkBox1.Visible = false;
        }
        #endregion
    } 
}
