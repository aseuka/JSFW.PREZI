using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JSFW.NPT.ASISTOBE.Controls;
using JSFW.NPT.ASISTOBE.Controls.Editor;
using JSFW.NPT.Dlls;
using JSFW.UI;

namespace JSFW.NPT.Projects.Process.Detail
{
    public partial class WorkDetailForm : Form
    {
        JSFW_Mover mv = new JSFW_Mover();

        ProjectInfo ProjectInfo { get; set; }
        ProcessInfo ProcessInfo { get; set; }

        WorkSlideItem SelectedWorkItem { get; set; }

        LinkPanel lnkPanel { get; set; }

        bool IsDataBinded = false;

        public WorkDetailForm()
        {
            InitializeComponent();

            DoubleBuffered = true;

            mv.Regist(new MV_R());
            mv.Regist(new MV_B());
            mv.SetParent(BackGroundPanel);
           
            lnkPanel = new LinkPanel();
            BackGroundPanel.Controls.Add(lnkPanel);
            lnkPanel.SetHostControl(mv.HostControl);
            lnkPanel.ChangedLinkTargetID += LnkPanel_ChangedLinkTargetID;

            this.Disposed += WorkDetailForm_Disposed;
        }

        private void LnkPanel_ChangedLinkTargetID(string linkID)
        {
            if (designPanel1.CurrentLinkLineID != linkID || linkID == "")
            {
                designPanel1.CurrentLinkLineID = linkID;
                designPanel1.Invalidate();
            }
        }

        private void WorkDetailForm_Disposed(object sender, EventArgs e)
        {
            mv.HostControl = null;

            using (lnkPanel) {
                if( lnkPanel != null)
                    lnkPanel.ChangedLinkTargetID -= LnkPanel_ChangedLinkTargetID;
            }
            lnkPanel = null;

          //  SelectedWorkSlideItem(null);
            //if (ProcessInfo != null) ProcessInfo.Dispose();

            ProcessInfo = null;
            ProjectInfo = null;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SelectedWorkSlideItem(null);
            base.OnFormClosing(e);
        }

        internal void SetProcess(ProjectInfo proj, ProcessInfo proci, string selectUnitID)
        {
            ProjectInfo = proj;
            ProcessInfo = proci; 
            DataBind(selectUnitID);  
        }
         
        /// <summary>
        /// 수정!
        /// </summary>
        bool IsEditing = false;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ("" + keyData).DebugWarning();

            if (keyData == Keys.Escape)
            {

                if (mv.HostControl != null)
                {
                    designPanel1_OpenViewAttributes(mv.HostControl as ASISTOBE.Controls.IATContent, ASISTOBE.Controls.ATEditStateEnum.End);
                }

                if (mv.HostControl != null && mv.HostControl.Parent != null)
                {
                    var prn = mv.HostControl.Parent;
                    mv.HostControl = prn; 
                }
                lnkPanel.SetHostControl(mv.HostControl); edit_ToolBar1.SetContent(mv.HostControl as IATContent);
                BackGroundPanel.Invalidate(true);
            }
            else if (keyData == (Keys.Delete | Keys.Control) && !IsEditing) // ( 기본판넬, MV_R, MV_B ) 말고 에디터가 떠있을때는... 수행하지 않음.
            {
                if (designPanel1.Equals(mv.HostControl) == false)
                {
                    if (mv.HostControl != null && mv.HostControl.Parent != null)
                    {
                        var ctrl = mv.HostControl;
                        ctrl.Parent.Controls.Remove(ctrl);
                        ASISTOBE.Controls.IATContent content = ctrl as ASISTOBE.Controls.IATContent;
                        using (content.EditControl)
                        {
                            if (content.EditControl != null)
                            {
                                BackGroundPanel.Controls.Remove(content.EditControl);
                            }
                        }
                        //if (!(ctrl is MockupItem_DataGrid))
                        //{
                        //    //C1FlexGrid를 지우면 에러가??? 왜 나는걸까? 
                        //    using (ctrl) { }
                        //}
                        mv.HostControl = null; 
                    }
                    BackGroundPanel.Invalidate(true);
                }

                lnkPanel.SetHostControl(mv.HostControl); edit_ToolBar1.SetContent(mv.HostControl as IATContent);
            }
            else
            {
                if (mv.HostControl is AT_TextControl)
                { 
                    if (keyData == (Keys.Shift | Keys.Control | Keys.Oemcomma))
                    {
                        // 폰트 작게
                        ((AT_TextControl)mv.HostControl).ChangeFont(-1f);
                    }
                    else if (keyData == (Keys.Shift | Keys.Control | Keys.OemPeriod))
                    {
                        // 폰트 크게
                        ((AT_TextControl)mv.HostControl).ChangeFont(+1f);
                    }
                    else if (keyData == (Keys.Control | Keys.B))
                    {
                        // 볼드..
                        ((AT_TextControl)mv.HostControl).ToggleFontBold();
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DataClear()
        {
            designPanel1.RootDirName = "";

            IsDataBinded = false;
            for (int loop = treeView1.Nodes.Count - 1; loop >= 0; loop--)
            {
                TreeNodeClear(treeView1.Nodes[loop]);
            }
            treeView1.Nodes.Clear();
        }

        private void TreeNodeClear(TreeNode tn)
        {
            for (int loop = tn.Nodes.Count - 1; loop >= 0; loop--)
            {
                TreeNodeClear(tn.Nodes[loop]);
            }
            tn.Tag = null;
            tn.Nodes.Clear(); 
        }
         

        private void DataBind(string selectUnitID = "")
        {
            DataClear();
            if (ProcessInfo != null)
            {
                Text = ProcessInfo.Unit.Name;

                designPanel1.RootDirName = ProcessInfo.GetFileRootDirectoryName();

                WorkSlideItem firstSelectedItem = null;
                foreach (var proc in ProcessInfo.Processes)
                {
                    if (proc.Type == ProcessUnit.ProcessType.Process) continue;

                    WorkSlideItem wi = new WorkSlideItem();
                    wi.SetID(proc.GUID, proc.Name); if( selectUnitID == proc.GUID) { firstSelectedItem = wi; }
                    WorkSlidePanel.Controls.Add(wi);
                    wi.Dock = DockStyle.Top;
                    wi.BringToFront();
                    wi.Click += Wi_Click; 
                }

                if (0 < WorkSlidePanel.Controls.Count)
                {
                    if (string.IsNullOrEmpty(selectUnitID.Trim()))
                    {
                        firstSelectedItem = WorkSlidePanel.Controls[WorkSlidePanel.Controls.Count - 1] as WorkSlideItem;
                    }
                    SelectedWorkSlideItem(firstSelectedItem);
                }

                // Tree Bind 
                TreeNode root = treeView1.Nodes.Add(ProjectInfo.Unit.Name);
                root.Name = ProjectInfo.Unit.GUID;
                root.Tag = ProjectInfo.Unit.ID;

                BindToModules(root, ProjectInfo);

                treeView1.ExpandAll();

                IsDataBinded = true;
            }
        }

        private void BindToModules(TreeNode node, ProjectInfo project)
        {
            foreach (var md in project.Modules)
            {
                TreeNode tn = node.Nodes.Add(string.Format("{1}[{0}]", md.ID, md.Name));
                tn.Name = md.GUID;
                tn.Tag = "MODULE";
                BindToScreenAndProcess(tn, md);
            }
        }
         
        private void BindToScreenAndProcess(TreeNode tn, ModuleUnit md)
        {
            var mdInfo = md.GetModuleInfo();

            TreeNode sctn = tn.Nodes.Add("화면"); 
            foreach (var sc in mdInfo.ScreenList)
            {
                TreeNode ttn = sctn.Nodes.Add(string.Format("{1}({0})", sc.ID, sc.Name));
                ttn.Name = sc.GUID;
                ttn.Tag = "SCREEN";
            }

            TreeNode pctn = tn.Nodes.Add("프로세스");
            foreach (var pc in mdInfo.ProcessList)
            {
                TreeNode ttn = pctn.Nodes.Add(string.Format("{1}({0})", pc.ID, pc.Name));
                ttn.Name = pc.GUID;
                ttn.Tag = "PROCESS";
            }
        }

        private void Wi_Click(object sender, EventArgs e)
        {
            mv.HostControl = null;
            lnkPanel.SetHostControl(mv.HostControl); edit_ToolBar1.SetContent(mv.HostControl as IATContent);
            SelectedWorkSlideItem(sender as WorkSlideItem);
        }

        private void SelectedWorkSlideItem(WorkSlideItem workSlideItem)
        {
            if (SelectedWorkItem == workSlideItem) return;
             
            if (SelectedWorkItem != null)
            {
                SelectedWorkItem.IsSelected = false;

                if (ProcessInfo.Processes != null)
                {
                    int idx = ProcessInfo.Processes.FindIndex(f => f.GUID == SelectedWorkItem.ID);
                    if (0 <= idx)
                    {
                        ProcessUnit unit = ProcessInfo.Processes[idx];
                        if (radioButton1.Checked)
                        {
                            // as is
                            ASISTOBE.AsisTobeData data = designPanel1.GetASISTOBEData();
                            if (data != null)
                            {
                                unit.ASIS = data.Serialize();
                                ProcessInfo.Save();
                                ProjectInfo.Save();
                            }
                        }
                        else
                        {
                            // to be
                            ASISTOBE.AsisTobeData data = designPanel1.GetASISTOBEData();
                            if (data != null)
                            {
                                unit.TOBE = data.Serialize();
                                ProcessInfo.Save();
                                ProjectInfo.Save();
                            }
                        }
                    }
                }
            }
             
            SelectedWorkItem = workSlideItem;

            if (SelectedWorkItem != null)
            {
                SelectedWorkItem.IsSelected = true;

                if (ProcessInfo.Processes != null)
                {
                    int idx = ProcessInfo.Processes.FindIndex(f => f.GUID == SelectedWorkItem.ID);
                    if (0 <= idx)
                    {
                        ProcessUnit unit = ProcessInfo.Processes[idx];
                        if (radioButton1.Checked)
                        {
                            // as is
                            ASISTOBE.AsisTobeData data = (""+unit.ASIS).DeSerialize<ASISTOBE.AsisTobeData>();
                            designPanel1.SetASISTOBEData(data, null);
                        }
                        else
                        {
                            // to be
                            ASISTOBE.AsisTobeData data = (""+unit.TOBE).DeSerialize<ASISTOBE.AsisTobeData>();
                            designPanel1.SetASISTOBEData(data, null);
                        }
                    }
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // AS IS
            mv.HostControl = null;
            lnkPanel.SetHostControl(mv.HostControl); edit_ToolBar1.SetContent(mv.HostControl as IATContent);
            if (radioButton1.Checked)
            {
                radioButton1.ForeColor = Color.White;

                if (ProcessInfo.Processes != null && IsDataBinded && SelectedWorkItem != null)
                {
                    int idx = ProcessInfo.Processes.FindIndex(f => f.GUID == SelectedWorkItem.ID);
                    if (0 <= idx)
                    {
                        ProcessUnit unit = ProcessInfo.Processes[idx];
                        if (radioButton1.Checked)
                        {
                            // as is
                            ASISTOBE.AsisTobeData tobe = designPanel1.GetASISTOBEData();
                            if (tobe != null)
                            {
                                unit.TOBE = tobe.Serialize(); 
                                ProcessInfo.Save();
                                ProjectInfo.Save();
                            }
                            ASISTOBE.AsisTobeData asis = unit.ASIS.DeSerialize<ASISTOBE.AsisTobeData>();
                            designPanel1.SetASISTOBEData(asis, designPanel1_OpenViewAttributes);
                        }
                        else
                        {
                            // to be
                            ASISTOBE.AsisTobeData asis = designPanel1.GetASISTOBEData();
                            if (asis != null)
                            {
                                unit.ASIS = asis.Serialize();
                                ProcessInfo.Save();
                                ProjectInfo.Save();
                            }

                            ASISTOBE.AsisTobeData tobe = unit.TOBE.DeSerialize<ASISTOBE.AsisTobeData>();
                            designPanel1.SetASISTOBEData(tobe, designPanel1_OpenViewAttributes);
                        }
                    }
                }
            }
            else
            {
                radioButton1.ForeColor = Color.LightGray; 
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // TO BE
            mv.HostControl = null;
            lnkPanel.SetHostControl(mv.HostControl); edit_ToolBar1.SetContent(mv.HostControl as IATContent);
            if (radioButton2.Checked)
            {
                radioButton2.ForeColor = Color.White;

                if (ProcessInfo.Processes != null && IsDataBinded && SelectedWorkItem != null)
                {
                    int idx = ProcessInfo.Processes.FindIndex(f => f.GUID == SelectedWorkItem.ID);
                    if (0 <= idx)
                    {
                        ProcessUnit unit = ProcessInfo.Processes[idx];
                        if (radioButton1.Checked)
                        {
                            // as is
                            ASISTOBE.AsisTobeData tobe = designPanel1.GetASISTOBEData();
                            if (tobe != null)
                            {
                                unit.TOBE = tobe.Serialize();
                                ProcessInfo.Save();
                                ProjectInfo.Save();
                            }
                            ASISTOBE.AsisTobeData asis = unit.ASIS.DeSerialize<ASISTOBE.AsisTobeData>();
                            designPanel1.SetASISTOBEData(asis, designPanel1_OpenViewAttributes);
                        }
                        else
                        {
                            // to be
                            ASISTOBE.AsisTobeData asis = designPanel1.GetASISTOBEData();
                            if (asis != null)
                            {
                                unit.ASIS = asis.Serialize();
                                ProcessInfo.Save();
                                ProjectInfo.Save();
                            }

                            ASISTOBE.AsisTobeData tobe = unit.TOBE.DeSerialize<ASISTOBE.AsisTobeData>();
                            designPanel1.SetASISTOBEData(tobe, designPanel1_OpenViewAttributes);
                        }
                    }
                }
            }
            else
            {
                radioButton2.ForeColor = Color.LightGray;
            }
        }
         
        bool isMouseDn = false;
        Point pt;
        #region 텍스트
        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            // Text
            isMouseDn = e.Button == MouseButtons.Left;
            pt = e.Location;
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            // Text
            if (isMouseDn)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    AsisTobeItemDragDropData dragObject = new AsisTobeItemDragDropData(new ASISTOBE.AsisTobeData() { TypeName = typeof(AT_TextControl).FullName, Text = "텍스트", Width = 80, Height = 21 });
                    try
                    {
                        DoDragDrop(dragObject, DragDropEffects.Copy);
                        isMouseDn = false;
                    }
                    finally
                    {
                        dragObject = null;
                    }
                }
            }
        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            // Text
            isMouseDn = false;
        }

        #endregion

        #region 이미지

        private void label3_MouseDown(object sender, MouseEventArgs e)
        {
            // Image
            isMouseDn = e.Button == MouseButtons.Left;
            pt = e.Location;
        }

        private void label3_MouseMove(object sender, MouseEventArgs e)
        {
            // Image
            if (isMouseDn)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    AsisTobeItemDragDropData dragObject = new AsisTobeItemDragDropData(new ASISTOBE.AsisTobeData() { TypeName = typeof(AT_ImageControl).FullName, Text = "이미지", Width = 48, Height = 48 });
                    try
                    {
                        DoDragDrop(dragObject, DragDropEffects.Copy);
                        isMouseDn = false;
                    }
                    finally
                    {
                        dragObject = null;
                    }
                }
            }
        }

        private void label3_MouseUp(object sender, MouseEventArgs e)
        {
            // Image
            isMouseDn = false;
        } 
        #endregion
        
        #region 그룹

        private void label4_MouseDown(object sender, MouseEventArgs e)
        {
            // group 
            isMouseDn = e.Button == MouseButtons.Left;
            pt = e.Location;
        }


        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
            // group 
            //AT_GroupControl
            if (isMouseDn)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    AsisTobeItemDragDropData dragObject = new AsisTobeItemDragDropData(new ASISTOBE.AsisTobeData() { TypeName = typeof(AT_GroupControl).FullName, Text = "그룹", Width = 60, Height = 21 });
                    try
                    {
                        DoDragDrop(dragObject, DragDropEffects.Copy);
                        isMouseDn = false;
                    }
                    finally
                    {
                        dragObject = null;
                    }
                }
            }
        }

        private void label4_MouseUp(object sender, MouseEventArgs e)
        {
            // group 
            isMouseDn = false;
        }

        #endregion

        #region 파일

        private void label5_MouseDown(object sender, MouseEventArgs e)
        {
            // 파일
            isMouseDn = e.Button == MouseButtons.Left;
            pt = e.Location;
        }

        private void label5_MouseMove(object sender, MouseEventArgs e)
        {
            // 파일
            if (isMouseDn)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    AsisTobeItemDragDropData dragObject = new AsisTobeItemDragDropData(new ASISTOBE.AsisTobeData() { TypeName = typeof(AT_FileControl).FullName, Text = "파일", Width = 60, Height = 21 });
                    try
                    {
                        DoDragDrop(dragObject, DragDropEffects.Copy);
                        isMouseDn = false;
                    }
                    finally
                    {
                        dragObject = null;
                    }
                }
            }
        }

        private void label5_MouseUp(object sender, MouseEventArgs e)
        {
            // 파일
            isMouseDn = false;
        } 
        #endregion

        private void designPanel1_OpenViewAttributes(ASISTOBE.Controls.IATContent content, ASISTOBE.Controls.ATEditStateEnum state)
        {
            //if (DesignView == DesignView.View)
            //{
            //    mv.HostControl = null;
            //    return;
            //}

            if (state == ASISTOBE.Controls.ATEditStateEnum.End || state == ASISTOBE.Controls.ATEditStateEnum.None)
            {
                bool isCompareResultTrue = false;
                if (mv.HostControl != (content as Control))
                {
                    if (mv.HostControl != null && mv.HostControl.Parent != null)
                    {
                        ASISTOBE.Controls.IATContent prevContent = mv.HostControl as ASISTOBE.Controls.IATContent;
                        if (prevContent != null)
                        {
                            if (prevContent.EditControl is Editor_AT_ImageControl)
                            {
                                ((Editor_AT_ImageControl)prevContent.EditControl).SetProjectInfo(null);
                            }
                            BackGroundPanel.Controls.Remove(prevContent.EditControl); 
                            IsEditing = false;
                        }
                    }
                }
                else
                {
                    // 더블클릭 > 마우스 다운 이벤트가 발생하여
                    // EditState.Begin > EditState.None 이벤트 생겨 mv조정자가 앞으로 나오는 경우.
                    // 같은지 비교하고, 에디터가 활성화 되어 있으면 mv조정자를 뒤로 숨겨줌
                    if (content != null && content.EditControl != null && content.EditControl.Visible)
                    {
                        isCompareResultTrue = true;
                    }
                }
                mv.HostControl = content as Control; 

                if (isCompareResultTrue) {
                    mv.SendToBack(); 
                }

                lnkPanel.SetHostControl(mv.HostControl); edit_ToolBar1.SetContent(mv.HostControl as IATContent);
            }
            else if (state == ASISTOBE.Controls.ATEditStateEnum.Begin)
            {
                if (mv.HostControl != null && mv.HostControl.Parent != null)
                {
                    ASISTOBE.Controls.IATContent prevContent = mv.HostControl as ASISTOBE.Controls.IATContent;
                    if (prevContent != null)
                    {
                        if (prevContent.EditControl is Editor_AT_ImageControl)
                        {
                            ((Editor_AT_ImageControl)prevContent.EditControl).SetProjectInfo(null);
                        }
                        BackGroundPanel.Controls.Remove(prevContent.EditControl);
                        IsEditing = false;
                    }
                }

                mv.HostControl = content as Control; 

                if (content.EditControl != null && mv.HostControl != null && mv.HostControl.Parent != null)
                {
                    if (content.EditControl is Editor_AT_ImageControl)
                    {
                        ((Editor_AT_ImageControl)content.EditControl).SetProjectInfo(ProjectInfo);
                    }
                    else if (content.EditControl is ScreenOpener)
                    {
                        ((ScreenOpener)content.EditControl).OpenDesigner(ProjectInfo);
                        return;
                    }
                    else if (content.EditControl is ProcessOpener)
                    {
                        ((ProcessOpener)content.EditControl).OpenDesigner(ProjectInfo);
                        return;
                    }
                    

                    BackGroundPanel.Controls.Add(content.EditControl);
                    Point pt = mv.HostControl.Parent.PointToScreen(mv.HostControl.Location);
                    content.EditControl.Dock = DockStyle.None;
                    content.EditControl.Location = BackGroundPanel.PointToClient(pt); 

                    content.EditControl.Visible = true;
                    content.EditControl.BringToFront();
                    content.EditControl.Focus();

                    // 백그라운드 밖으로 나가면 안쪽으로 옮겨준다.
                    if (designPanel1.Bounds.Contains(content.EditControl.Bounds) == false)
                    {
                        if (designPanel1.Bounds.Right < content.EditControl.Bounds.Right)
                            content.EditControl.Left -= (content.EditControl.Bounds.Right - designPanel1.Bounds.Right);
                        if (designPanel1.Bounds.Bottom < content.EditControl.Bounds.Bottom)
                            content.EditControl.Top -= (content.EditControl.Bounds.Bottom - designPanel1.Bounds.Bottom);
                    }
                    IsEditing = true;
                }

                lnkPanel.SetHostControl(mv.HostControl); edit_ToolBar1.SetContent(mv.HostControl as IATContent);
            }
        }

         
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDn = false;
        }
         
        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDn)
            {
                int x = e.Location.X - pt.X;
                int y = e.Location.Y - pt.Y;
                int z = (int)Math.Sqrt(Math.Pow((double)Math.Abs(x), 2d) + Math.Pow((double)Math.Abs(y), 2d));

                if (4 < z)
                {
                    TreeNode tn = treeView1.GetNodeAt(e.Location);
                    string gubun = "" + tn.Tag;

                    if (gubun == "MODULE")
                    {
                        AsisTobeItemDragDropData dragObject = new AsisTobeItemDragDropData(new ASISTOBE.AsisTobeData()
                        {
                            TypeName = typeof(AT_GroupControl).FullName,
                            Text = tn.Text,

                            IsReadOnly = true,
                            Width = 100,
                            Height = 21
                        });
                        try
                        {
                            DoDragDrop(dragObject, DragDropEffects.Copy);
                            isMouseDn = false;
                        }
                        finally
                        {
                            dragObject = null;
                        }
                    }
                    else if (gubun == "SCREEN")
                    {
                        AsisTobeItemDragDropData dragObject = new AsisTobeItemDragDropData(new ASISTOBE.AsisTobeData()
                        {
                            TypeName = typeof(AT_ScreenControl).FullName,
                            ScreenID = tn.Name,
                            Text = string.Format("{0} {1}", tn.Parent.Parent.Text, tn.Text),
                            Width = 80,
                            Height = 21
                        });
                        try
                        {
                            DoDragDrop(dragObject, DragDropEffects.Copy);
                            isMouseDn = false;
                        }
                        finally
                        {
                            dragObject = null;
                        }
                    }
                    else if (gubun == "PROCESS")
                    {
                        AsisTobeItemDragDropData dragObject = new AsisTobeItemDragDropData(new ASISTOBE.AsisTobeData()
                        {
                            TypeName = typeof(AT_ProcessControl).FullName,
                            ProcessID = tn.Name,
                            Text = string.Format("{0} {1}", tn.Parent.Parent.Text, tn.Text),
                            Width = 80,
                            Height = 21
                        });
                        try
                        {
                            DoDragDrop(dragObject, DragDropEffects.Copy);
                            isMouseDn = false;
                        }
                        finally
                        {
                            dragObject = null;
                        }
                    }
                    else
                    {
                        isMouseDn = false;
                    }
                }
            } 
        }
         
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        { 
            isMouseDn = e.Button == MouseButtons.Left;
            pt = e.Location;
        }

        private void edit_ToolBar1_DataChanged(object sender, ToolbarChangedValueEvent e)
        {
            // mv.HostControl에 설정.
            IATContent cont = mv.HostControl as IATContent;
            if (cont != null)
            {
                cont.SetFontStyleAndBorder(e.FontSize, e.FontBold, e.FontUnderLine, e.TextAlign, e.ForeColor, e.BackColor, e.Border);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mv.HostControl != null && !( mv.HostControl is DesignPanel ) )
                mv.HostControl.BringToFront(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mv.HostControl != null && !(mv.HostControl is DesignPanel))
                mv.HostControl.SendToBack();
        }

        private void btnScreenShot_Click(object sender, EventArgs e)
        {
            // 캡쳐
            mv.HostControl = null;
            try
            {
                designPanel1.Dock = DockStyle.None;
                Rectangle rect = new Rectangle();
                for (int loop = 0; loop < designPanel1.Controls.Count; loop++)
                {
                    if (rect.Width < designPanel1.Controls[loop].Bounds.Right)
                        rect.Width = designPanel1.Controls[loop].Bounds.Right;

                    if (rect.Height < designPanel1.Controls[loop].Bounds.Bottom)
                        rect.Height = designPanel1.Controls[loop].Bounds.Bottom; 
                }
                designPanel1.Width = rect.Width + 10;
                designPanel1.Height = rect.Height + 10;
                designPanel1.BorderStyle = BorderStyle.None;
                designPanel1.ControlShot(-1);
            }
            finally
            {
                designPanel1.BorderStyle = BorderStyle.FixedSingle; 
                designPanel1.Dock = DockStyle.Fill;
            }
        }

        public static Bitmap CaptureFormImage(Control ctrl)
        {
            Bitmap image = new Bitmap(ctrl.Width, ctrl.Height);
            ctrl.DrawToBitmap(image, new Rectangle(new Point(0, 0), image.Size));
            return image;
        }

        public static void CaptureAndSaveFormImage(Form form, string filename)
        {
            CaptureFormImage(form).Save(filename);
        }
    }
}
