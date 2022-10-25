using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JSFW.PREZI.Controls;

namespace JSFW.PREZI
{
    public partial class Demo_Transparent_PanelForm : Form
    {
        public Demo_Transparent_PanelForm()
        {
            InitializeComponent();
            transparent_Panel1.Parent = this;
            AllowDrop = true;
            AllowTransparency = true;
        }
         
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            TransparentDragDropEventArgs dropData = new TransparentDragDropEventArgs(true, "" );
            dropData.Text = @"드래그 드롭
-- 테스트 1, 2, 3
-- 가나다.";

            DoDragDrop(dropData, DragDropEffects.Copy);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("버튼");
        }

        private void Demo_Transparent_PanelForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                Point pt = PointToClient(new Point(e.X, e.Y));
                Button btn = new Button()
                {
                    Location = pt,
                    Text = "" + e.Data.GetData(typeof(string))
                };
                Controls.Add(btn);
                btn.Click += Btn_Click;
            }
        }

        private void Demo_Transparent_PanelForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == DragDropEffects.Copy)
            {
                e.Effect = e.AllowedEffect;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("드랍 버튼 클릭");
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            TransparentDragDropEventArgs dropData = new TransparentDragDropEventArgs(false, typeof(Controls.Label).FullName) { Text = "버튼 텍스트" };
            DoDragDrop(dropData, DragDropEffects.Copy);
        }

        private void Btn_Click1(object sender, EventArgs e)
        {
            MessageBox.Show("드랍 버튼 클릭!!!");
        }
    }
     
}
