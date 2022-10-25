using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using JSFW.PREZI.Controls;
using JSFW.PREZI.Controls.Editor;

namespace JSFW.PREZI.Editor
{
    // Editor는 오픈될때 프로젝트 정보 관리 객체를 받아서 이미지 목록 바인딩을 하게 된다. 

    public partial class Editor_AT_ImageControl : UserControl
    {
        bool IsComplite = false;
        public event Action<bool> EditComplited = null;

        ClipImageManger cImg = new ClipImageManger();

        public string ImageName
        {
            get; set;
        }

        public string ImageFilePath
        {
            get; set;
        }

        public ImageClipViewItemControl SelectedItem { get; private set; }

        public Editor_AT_ImageControl()
        {
            InitializeComponent();
        }
        
        private void btnNewDrawing_Click(object sender, EventArgs e)
        {
            // 그림을 새로 그림! 
            using (var edit = new Editor_NewDrawingPicture())
            {
                if (edit.ShowDialog(this) == DialogResult.OK)
                { 
                    ImageClipViewItemControl img = new ImageClipViewItemControl();
                    ClipImage clip = new ClipImage();
                    clip.ImageLocation = edit.ImageLocation;
                    clip.Name = edit.ImageName;
                    clip.Tag = edit.ImageTag;
                    img.SetClipImage(clip);
                    flowLayoutPanel1.Controls.Add(img);
                    img.ItemDbClick += Img_ItemDbClick;
                    img.ItemSelected += Img_ItemSelected;
                    // 이미지에 등록!
                    cImg.Add(clip);
                    cImg.Save();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (NewImageForm nif = new NewImageForm())
            {
                if (nif.ShowDialog(this) == DialogResult.OK)
                {
                    // 이미지 아이템으로... 
                    ImageClipViewItemControl img = new ImageClipViewItemControl();
                    ClipImage clip = new ClipImage();
                    clip.ImageLocation = nif.ImageLocation;
                    clip.Name = nif.ImageName;
                    clip.Tag = nif.ImageTag; 
                    img.SetClipImage(clip);
                    flowLayoutPanel1.Controls.Add(img);
                    img.ItemDbClick += Img_ItemDbClick;
                    img.ItemSelected += Img_ItemSelected;
                    // 이미지에 등록!
                    cImg.Add(clip);
                    cImg.Save();
                }
            }
        }

        private void Img_ItemSelected(object sender, ItemsSelectedEventArgs<ImageClipViewItemControl> e)
        {
            SetItem(e.Item);
        }
         
        private void SetItem(ImageClipViewItemControl item)
        {
            if (SelectedItem != null)
            {
                SelectedItem.BackColor = Color.White;
                SelectedItem.ForeColor = Color.Black;
            }

            SelectedItem = item;

            if (SelectedItem != null)
            {
                SelectedItem.BackColor = Color.OrangeRed;
                SelectedItem.ForeColor = Color.White;
            }
        }

        private void Img_ItemDbClick(object sender, ItemsSelectedEventArgs<ImageClipViewItemControl> e)
        {
            // 아이템 선택! 
            ImageName = e.Item.Clip.Name;
            ImageFilePath = e.Item.Clip.ImageLocation;

            IsComplite = true;
            OnLeave(EventArgs.Empty);
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (EditComplited != null) EditComplited(IsComplite);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            foreach (ImageClipViewItemControl item in flowLayoutPanel1.Controls)
            {
                item.ShowDeleteCheckBox();
            }
            btnOK.BringToFront();
            btnCancel.BringToFront();
        }
         
        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (ImageClipViewItemControl item in flowLayoutPanel1.Controls)
            {
                item.HideDeleteCheckBox();
            }

            btnAdd.BringToFront();
            btnDel.BringToFront();
        }
         
        private void btnOK_Click(object sender, EventArgs e)
        {
            // 삭제완료... 
            List<ImageClipViewItemControl> dels = new List<ImageClipViewItemControl>();
            foreach (ImageClipViewItemControl item in flowLayoutPanel1.Controls)
            {
                if (item.IsDeleteSelected)
                    dels.Add(item);
            }

            for (int loop = dels.Count - 1; loop >= 0; loop--)
            {
                using (dels[loop])
                { 
                    dels[loop].ItemDbClick -= Img_ItemDbClick;
                    dels[loop].ItemSelected -= Img_ItemSelected;
                    cImg.Remove(dels[loop].Clip);                     
                    flowLayoutPanel1.Controls.Remove(dels[loop]);
                }
            }
            cImg.Save();

            foreach (ImageClipViewItemControl item in flowLayoutPanel1.Controls)
            {
                item.HideDeleteCheckBox();
            }

            btnAdd.BringToFront();
            btnDel.BringToFront();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            IsComplite = false;
            OnLeave(e);
        }

        internal void CleanImages()
        {
             
        }

        internal void RefreshImages()
        {
            cImg.Load();
            DataBind();
        }

        internal void DataBind()
        {
            DataClear();
            if (cImg != null)
            {
                foreach (var clip in cImg.Images)
                {
                    ImageClipViewItemControl img = new ImageClipViewItemControl();
                    img.SetClipImage(clip);
                    flowLayoutPanel1.Controls.Add(img);
                    img.ItemDbClick += Img_ItemDbClick;
                    img.ItemSelected += Img_ItemSelected;
                }
            }
        }


        private void DataClear()
        {
            for (int loop = flowLayoutPanel1.Controls.Count - 1; loop >= 0; loop--)
            {
                using (flowLayoutPanel1.Controls[loop]) { }
            }
            flowLayoutPanel1.Controls.Clear();
        } 
    }
}
