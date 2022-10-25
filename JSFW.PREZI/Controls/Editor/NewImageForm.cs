using System;
using System.Drawing;
using System.Windows.Forms;

namespace JSFW.PREZI.Editor
{
    public partial class NewImageForm : Form
    {
        public string ImageName { get; set; }
        public string ImageTag { get; set; }
        public string ImageLocation { get; set; }

        // 클립보드로 붙여넣은 이미지
        public bool IsPastImage { get; set; }
        public Image Image { get { return pictureBox1.Image; } set { pictureBox1.Image = value; } }

        public NewImageForm()
        {
            InitializeComponent();

            this.Disposed += NewImageForm_Disposed;
        }

        private void NewImageForm_Disposed(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            pictureBox1.ImageLocation = null;
            base.OnFormClosing(e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        { 
            if (string.IsNullOrEmpty(textBox1.Text.Trim())) {
                "이미지 이름이 필요합니다.".Alert();
                return;
            }

            if (IsPastImage == false)
            {
                if (string.IsNullOrEmpty(pictureBox1.ImageLocation.Trim()))
                {
                    "이미지 경로가 필요합니다.".Alert();
                    return;
                }
            }

            //if (File.Exists(pictureBox1.ImageLocation) == false)
            //{ }
            using (Image) { }
            Image = null;
             
            ImageName = textBox1.Text.Trim();
            ImageTag = textBox2.Text.Trim(); 
            ImageLocation = pictureBox1.ImageLocation; 
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Open Image!
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지파일 (*.jpg, *.jpeg, *.gif, *.png) | *.jpg; *.jpeg; *.gif; *.png";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 임시경로에 파일을 옮겨놓고?  
                    pictureBox1.ImageLocation = ofd.FileName;
                    pictureBox1.Refresh();

                    textBox1.Focus(); 
                }
            } 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (isTextBox2_KeyEdit == false)
            {
                textBox2.Text = textBox1.Text;
            }
        }

        bool isTextBox2_KeyEdit = false;
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            isTextBox2_KeyEdit = true;
        }
    }
}
