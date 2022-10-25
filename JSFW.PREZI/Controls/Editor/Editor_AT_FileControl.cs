using System;
using System.Windows.Forms;
using System.IO;

namespace JSFW.PREZI.Editor
{
    public partial class Editor_AT_FileControl : UserControl
    {
        bool IsComplite = false;
        public event Action<bool> EditComplited = null;
 
        public string FileName { get; private set; }

        public string FilePath { get; private set; }

        public Editor_AT_FileControl()
        {
            InitializeComponent();
        }

        public void SetFileInfo(string fileName, string filePath)
        {
            FileName = textBox1.Text = fileName;
            FilePath = linkLabel1.Text = filePath;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(("" + NewFilePath).Trim()))
            {
                string temp = JSFW_PREZI_CONST.ProjectFilesDirectoryName + "\\" + Guid.NewGuid().ToString("N") + Path.GetExtension(NewFilePath);
                // 파일 이동.. C:\JSFW\NPT\TEST\Process\PROC002\Files\
                File.Copy(NewFilePath, temp, true);
                FileName = textBox1.Text;
                FilePath = temp;
                IsComplite = true;
            }
            OnLeave(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsComplite = false;
            OnLeave(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (EditComplited != null) EditComplited(IsComplite);
        }

        string NewFileName { get; set; }
        string NewFilePath { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            // openFile Dialog 
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    IsComplite = true;
                    textBox1.Text = NewFileName = Path.GetFileName(ofd.FileName);
                    linkLabel1.Text = NewFilePath = ofd.FileName;
                    btnApply_Click(btnApply, e);
                }
            }
        }

        internal void ClearNewFileName()
        {
            NewFilePath = "";
            NewFileName = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 링크 오픈..
            if (string.IsNullOrEmpty((linkLabel1.Text).Trim()) || (linkLabel1.Text).ToUpper() == "EMPTY")
            {
                return;
            }
 
            if (("" + linkLabel1.Text).Trim().StartsWith(JSFW_PREZI_CONST.ProjectFilesDirectoryName))
            {  
                Open((""+ linkLabel1.Text));
            } 
            else
            {
                if (string.IsNullOrEmpty(("" + NewFilePath).Trim()))
                {
                    return;
                }
                System.Diagnostics.Process.Start(NewFilePath);
            } 
        }
         
        public void Open(string filename)
        {
            string Extention = Path.GetExtension(filename);
            string Dir = Path.GetDirectoryName(filename) + "\\";
            string GUID = Path.GetFileNameWithoutExtension(filename);

            switch (Extention.ToLower())
            {
                // 백업해야 할 문서 파일인 경우!!
                case ".txt":
                case ".doc":
                case ".xlsx":
                case ".xls":
                case ".ppt":
                case ".pptx":
                    {
                        string dt = DateTime.Now.ToString("yyyyMMdd");
                        string backupFileName = Dir + GUID + "__" + dt + Extention;

                        if (File.Exists(backupFileName) == false)
                        {
                            File.Copy(filename, backupFileName, true);
                        }
                    }
                    break;
            }

            if (Exists(filename))
            {
                // 파일 수정을 해야 하니.. 그게 맞겠넹.
                System.Diagnostics.Process.Start(filename);
            }
        }

        internal bool Exists(string filename)
        {
            return File.Exists(filename);
        }
    }
}
