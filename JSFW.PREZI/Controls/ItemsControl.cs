using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*
 *  ItemCtrl  컨트롤은 다른 모양의 컨트롤로 변경 사용이 가능하다.
 *  이 컨트롤에 바인딩되는 데이타클래스는 DataClass이지만 변경이 가능하다.
 *  * 필요한 자료구조를 재정의해서 그에 맞는 컨트롤을 만들어서 바인딩처리 하면 된다. 
 *   
     */
namespace JSFW.PREZI.Controls
{
    public partial class ItemsControl<T> : UserControl 
        where T : Control, IItemControl<T>, new()
    {
        public T SelectedItem { get; protected set; }

        public event Func<T> ItemAddClicked = null;

        public event Action<bool,T> ItemRemoved = null;
         
        public event Action Saved = null;

        public ItemsControl()
        {
            InitializeComponent();
            this.Disposed += ItemsControl_Disposed;
        }
         
        public void ItemsClear()
        {
            for (int loop = flowLayoutPanel1.Controls.Count - 1; loop >= 0; loop--)
            {
                using (flowLayoutPanel1.Controls[loop])
                {
                    if (flowLayoutPanel1.Controls[loop].Equals(SelectedItem)) { SetItem(null); }

                    ((T)flowLayoutPanel1.Controls[loop]).ItemDbClick -= NewInstance_ItemDbClick;
                    ((T)flowLayoutPanel1.Controls[loop]).ItemSelected -= NewInstance_ItemSelected; 
                    flowLayoutPanel1.Controls.RemoveAt(loop);

                    if (ItemRemoved != null) ItemRemoved(false, flowLayoutPanel1.Controls[loop] as T);
                }
            }
        }

        public void Add(T itemCtrl)
        {
            if (itemCtrl != null)
            {
                flowLayoutPanel1.Controls.Add(itemCtrl);
                itemCtrl.ItemDbClick += NewInstance_ItemDbClick;
                itemCtrl.ItemSelected += NewInstance_ItemSelected; 
            }
        }

        private void ItemsControl_Disposed(object sender, EventArgs e)
        {
            SetItem(null);
        }

        private void SetItem(T item)
        {
            if (SelectedItem != null)
            {
                SelectedItem.BackColor = Color.FromArgb(255, 224, 192);
                SelectedItem.ForeColor = Color.Black;
            }

            SelectedItem = item;

            if (SelectedItem != null)
            {
                SelectedItem.BackColor = Color.Coral;
                SelectedItem.ForeColor = Color.White;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (T item in flowLayoutPanel1.Controls)
            { 
                item.HideCheckBox();
            }
            btnOK.SendToBack();
            btnCancel.SendToBack();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<T> rmitems = new List<T>();
            foreach (T item in flowLayoutPanel1.Controls)
            {
                if (item.IsSelected)
                {
                    rmitems.Add(item);
                }
            }

            for (int loop = rmitems.Count - 1; loop >= 0; loop--)
            {
                using (rmitems[loop])
                {
                    if (rmitems[loop].Equals(SelectedItem)) { SetItem(null); }

                    rmitems[loop].ItemDbClick -= NewInstance_ItemDbClick;
                    rmitems[loop].ItemSelected -= NewInstance_ItemSelected;
                     
                    flowLayoutPanel1.Controls.Remove(rmitems[loop]);

                    if (ItemRemoved != null) ItemRemoved(true, rmitems[loop]);
                }
            }
             
            foreach (T item in flowLayoutPanel1.Controls)
            {
                item.HideCheckBox();
            }

            OnSave();

            btnOK.SendToBack();
            btnCancel.SendToBack();
        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            if (ItemAddClicked != null)
            {
                T newInstance = ItemAddClicked();
                if (newInstance != null)
                {
                    Add(newInstance);
                    SetItem(newInstance);
                    flowLayoutPanel1.ScrollControlIntoView(newInstance);
                }
                else
                {
                    return;
                }
            }
            OnSave();
        }

        private void NewInstance_ItemSelected(object sender, ItemsSelectedEventArgs<T> e)
        {
            SetItem(e.Item);
        }

        private void NewInstance_ItemDbClick(object sender, ItemsSelectedEventArgs<T> e)
        {
            // 더블클릭 : e.Item
        }

        private void OnSave()
        {
            if (Saved != null)
            {
                Saved();
            }
        }

        private void btnDEL_Click(object sender, EventArgs e)
        {
            foreach (T item in flowLayoutPanel1.Controls)
            {
                item.ShowCheckBox();
            }
            btnOK.BringToFront();
            btnCancel.BringToFront();
        }
    }

    public class ItemsSelectedEventArgs<T> : EventArgs, IDisposable
        where T : class, new()
    {
        public T Item { get; protected set; }

        public ItemsSelectedEventArgs(T item)
        {
            Item = item;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    Item = null;
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~ItemsSelectedEventArgs() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public interface IItemControl<T> where T : class, new()
    {
        event EventHandler<ItemsSelectedEventArgs<T>> ItemSelected;
        event EventHandler<ItemsSelectedEventArgs<T>> ItemDbClick;

        bool IsSelected { get; }

        void HideCheckBox();
        void ShowCheckBox();

    }
}
