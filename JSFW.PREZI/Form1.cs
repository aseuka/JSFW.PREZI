using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JSFW.PREZI.Controls; 

namespace JSFW.PREZI
{
    public partial class Form1 : Form
    {
        List<DataClass> datas = new List<DataClass>(); 

        public Form1()
        {
            InitializeComponent();

            JSFW_PREZI_CONST.CheckProjectDirectory(); 
            DataLoad(); 
        }

        internal void DataLoad()
        {
            DataClear();

            string fileName = JSFW_PREZI_CONST.ProjectRootDirectoryName;
            fileName += @"\DataManager.xml";

            if (File.Exists(fileName))
            {
                string xml = File.ReadAllText(fileName);
                List<DataClass> tmp = xml.DeSerialize<List<DataClass>>();
                datas.Clear();
                datas.AddRange(tmp.ToArray());
                tmp = null;

                foreach (var data in datas)
                {
                    ItemCtrl itemCtrl = new ItemCtrl();
                    itemCtrl.SetDataClass(data); 
                    testlistctrl1.Add(itemCtrl);
                    itemCtrl.ItemDbClick += ItemCtrl_ItemDbClick;
                }
            }
        }

        private void DataClear()
        {
            testlistctrl1.ItemsClear(); 
        }

        internal void DataSave()
        {
            string fileName = JSFW_PREZI_CONST.ProjectRootDirectoryName;
            fileName += @"\DataManager.xml";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            string xml = datas.Serialize(); 
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            System.IO.File.AppendAllText(fileName, xml); 
        }

        private void ItemCtrl_ItemDbClick(object sender, ItemsSelectedEventArgs<ItemCtrl> e)
        {
            using (NewItemForm nf = new NewItemForm())
            {
                nf.SetNameText(e.Item.Text);
                if (nf.ShowDialog(this) == DialogResult.OK)
                {
                    if (nf.IsDirty)
                    {
                        e.Item.Data.FullDirectoryName = nf.NameText;
                        e.Item.Data.Name = nf.NameText;
                        e.Item.Text = nf.NameText;
                        DataSave();
                        nf.HasRaiseViewEvent = true;
                    }

                    if (nf.HasRaiseViewEvent)
                    { 
                        // 상세보기. 
                        // 디자이너 띄우기.  
                        using (PreziDesignForm pdf = new PreziDesignForm())
                        {
                            pdf.FormClosed += (fs, fe) =>
                            {
                                this.Show();
                            };

                            pdf.Shown += (fs, fe) =>
                            {
                                this.Hide();
                            };

                            pdf.SetDataClass(e.Item.Data, DataSave);
                            pdf.ShowDialog(this);
                        }
                    }
                }
            }
        }

        private ItemCtrl testlistctrl1_Added()
        {
            ItemCtrl itemCtrl = null;
            using (NewItemForm nf = new NewItemForm() { IsNew = true })
            {
                if (nf.ShowDialog() == DialogResult.OK)
                {
                    itemCtrl = new ItemCtrl();
                    DataClass data = new DataClass() { Name = nf.NameText , FullDirectoryName = nf.NameText };
                    datas.Add(data);
                    itemCtrl.SetDataClass(data);
                    itemCtrl.ItemDbClick += ItemCtrl_ItemDbClick;
                }
            }
            return itemCtrl;
        }

        private void testlistctrl1_ItemRemoved(bool dataSync, ItemCtrl rmItemCtrl)
        {
            if (rmItemCtrl != null)
            {
                rmItemCtrl.ItemDbClick -= ItemCtrl_ItemDbClick;
                if(dataSync) // ItemClear 할때 싱크하지 않음.
                    datas.Remove(rmItemCtrl.Data);
            }
        }

        private void testlistctrl1_Saved()
        {
            // datas.저장
            DataSave();
        }
    }

    /// <summary>
    /// 리스트 목록 컨트롤.
    /// </summary>
    public class TESTLISTCTRL : ItemsControl<ItemCtrl>
    {
    }

    public class DataClass : ICloneable
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string DesignSource { get; set; }

        public List<Frame> Frames { get; set; }

        /// <summary>
        /// 서브 클래인 경우 - 컨트롤로 오픈시 닫기버튼 처리
        /// </summary>
        public bool IsSubDataClass { get; set; }

        public string FullDirectoryName { get; set; }

        public DataClass()
        {
            ID = Guid.NewGuid().ToString("N");
            Frames = new List<Frame>();
        }

        public object Clone()
        {
            DataClass nData = new DataClass();
            nData.Name = Name;
            nData.FullDirectoryName = FullDirectoryName;
            nData.Description = Description;
            nData.DesignSource = DesignSource;

            for (int loop = Frames.Count - 1; loop >= 0; loop--)
            {
                nData.Frames.Add(Frames[loop].Clone() as Frame);
            }

            return nData;
        }
    }

    public class ClipImageManger
    {
        /*
            ClipImages
        */
        public List<ClipImage> Images { get; set; }

        public ClipImageManger()
        {
            Images = new List<ClipImage>();
        }

        public void Add(ClipImage clip)
        {
            if (Images != null)
            {
                string fileName = Path.GetFileName(clip.ImageLocation);

                if (File.Exists(clip.ImageLocation))
                {
                    File.Copy(clip.ImageLocation, JSFW_PREZI_CONST.ProjectImagesDirectoryName + "\\" + fileName, true);
                    clip.ImageLocation = JSFW_PREZI_CONST.ProjectImagesDirectoryName + "\\" + fileName;
                    Images.Add(clip);
                }
            }
            else
            {
                "이미지 목록을 읽어오지 못했음.".AlertWarning();
            }
        }

        public void Save()
        {
            string fileName = JSFW_PREZI_CONST.ProjectRootDirectoryName;
            fileName += @"\ClipImages.xml";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            string xml = Images.Serialize();
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            System.IO.File.AppendAllText(fileName, xml);
        }

        public void Load()
        {
            string fileName = JSFW_PREZI_CONST.ProjectRootDirectoryName;
            fileName += @"\ClipImages.xml";

            if (File.Exists(fileName))
            {
                string xml = File.ReadAllText(fileName);
                List<ClipImage> tmp = xml.DeSerialize<List<ClipImage>>();
                Images.Clear();
                Images.AddRange(tmp.ToArray());
                tmp = null; 
            }
        }

        internal void Remove(ClipImage clip)
        {
            if(Images != null ) Images.Remove(clip);
        }
    }

    public class ClipImage : ICloneable
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string ImageLocation { get; set; }

        public ClipImage()
        {
            ID = Guid.NewGuid().ToString("N");
        }

        public object Clone()
        {
            ClipImage nImg = new ClipImage();
            nImg.Name = Name;
            nImg.Tag = Tag;
            nImg.ImageLocation = ImageLocation; 
            return nImg;
        }
    }



    public class PreziDataClass : ICloneable
    {
        /// <summary>
        /// 컨트롤 타입
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 이름
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 텍스트 또는 쿼리
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 이미지 경로 ( 클립 아트 )
        /// </summary>
        public enum FileType
        {
            IMAGE,
            Text,
            Excel,
            PPT,
            ETC
        }

        public string FilePath { get; set; }

        public List<PreziDataClass> InnerDatas { get; set; }

        public PreziDataClass()
        {
            ID = Guid.NewGuid().ToString("N");
            InnerDatas = new List<PreziDataClass>();
            Links = new List<LinkData>();
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public List<LinkData> Links { get; set; }
        public int Thumb_Left { get; set; }
        public int Thumb_Top { get; set; }

        public bool IsReadOnly { get; set; }

        /// <summary>
        /// 화면ID = GUID 
        /// </summary>
        public string ScreenID { get; set; }

        /// <summary>
        /// 프로세스ID = GUID 
        /// </summary>
        public string ProcessID { get; set; }
        /// <summary>
        /// 텍스트 컨트롤에 폰트사이즈, 볼드체... 
        /// </summary>
        public float FontSize { get; set; }
        /// <summary>
        /// 텍스트 컨트롤에 폰트사이즈, 볼드체... 
        /// </summary>
        public bool FontBold { get; set; }
        public bool FontUnderLine { get; set; }
        public HorizontalAlignment FontAlign { get; set; }
        public string ForeColor { get; set; } = ColorTranslator.ToHtml(Color.Black);
        public string BackColor { get; set; } = ColorTranslator.ToHtml(Color.White);
        public AT_Border Border { get; set; }

        public object Clone()
        {
            PreziDataClass nData = new PreziDataClass();
            nData.TypeName = TypeName;
            nData.Name = Name;
            nData.Text = Text;
            nData.FilePath = FilePath;

            nData.Width = Width;
            nData.Height = Height;
            nData.Left = Left;
            nData.Top = Top;

            nData.Thumb_Left = Thumb_Left;
            nData.Thumb_Top = Thumb_Top;

            nData.IsReadOnly = IsReadOnly;
            nData.ScreenID = ScreenID;
            nData.ProcessID = ProcessID;

            nData.FontSize = FontSize;
            nData.FontBold = FontBold;
            nData.FontUnderLine = FontUnderLine;
            nData.FontAlign = FontAlign;
            nData.ForeColor = ForeColor;
            nData.BackColor = BackColor;
            nData.Border = Border;
            nData.Data = Data?.Clone() as DataClass;

            return nData;
        }

        DataClass Data { get; set; } 
    }


    public class LinkData
    {
        public string TargetID { get; set; } 

        public LinkData()
        {

        }
    }

    public class JSFW_PREZI_CONST
    {
        /// <summary>
        /// C:\JSFW\PREZI
        /// </summary>
        public static readonly string ProjectRootDirectoryName = @"C:\JSFW\PREZI";

        public static readonly string ProjectImagesDirectoryName = ProjectRootDirectoryName + @"\Images";

        public static string ProjectFilesDirectoryName = ProjectRootDirectoryName + @"\Files";
        internal static void CheckProjectDirectory()
        {
            if (Directory.Exists(ProjectRootDirectoryName) == false)
                Directory.CreateDirectory(ProjectRootDirectoryName);

            if (Directory.Exists(ProjectImagesDirectoryName) == false)
                Directory.CreateDirectory(ProjectImagesDirectoryName);

            if (Directory.Exists(ProjectFilesDirectoryName) == false)
                Directory.CreateDirectory(ProjectFilesDirectoryName);
        }
    }


}
