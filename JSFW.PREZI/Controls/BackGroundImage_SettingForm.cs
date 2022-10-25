using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSFW.PREZI.Controls
{
    public partial class BackGroundImage_SettingForm : Form
    {
        public string TargetDataID { get; set; }

        public bool IsDirty { get; private set; }

        public bool IsDelete { get; private set; }

        public string ImageFilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TargetDataID))
                {
                    return "";
                }
                else
                {
                    return JSFW_PREZI_CONST.ProjectFilesDrawImageDirectoryName + $"\\{TargetDataID}_BACKGROUND.png";
                }
            }
        }

        public string OpenFilePath { get; set; }

        public BackGroundImage_SettingForm()
        {
            InitializeComponent();            
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            using (pictureBox1.Image)
            {
                pictureBox1.ImageLocation = null;
            }
            pictureBox1.Image = null;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (File.Exists(ImageFilePath))
            {
                pictureBox1.ImageLocation = ImageFilePath;
            }
            OpenFilePath = "";
            IsDirty = IsDelete = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OpenFilePath = "";
            IsDirty = IsDelete = false;

            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            IsDirty = true;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "배경 이미지 불러오기";
                ofd.Filter = "Image File|*.JPG;*.BMP;*.ICO;*.EMF;*.GIF;";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (pictureBox1.Image)
                    {
                        pictureBox1.ImageLocation = null;
                    }
                    pictureBox1.Image = null;

                    OpenFilePath = ofd.FileName;
                    pictureBox1.ImageLocation = ofd.FileName;
                    IsDelete = false;
                }
            }
        }

        private void btnDelImage_Click(object sender, EventArgs e)
        {
            IsDirty = true;
            pictureBox1.ImageLocation = "";
            IsDelete = true;
        }
    }
}
