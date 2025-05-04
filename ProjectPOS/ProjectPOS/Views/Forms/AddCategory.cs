using ProjectPOS.Controllers;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS.Views.Forms
{
    public partial class AddCategory : Form
    {
        public event Action _OnLoadCate;
        private CategoryController _categoryController = new CategoryController();
        public AddCategory()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsVali = Validation.ValidationUpdateCategory(txtName.Text, 0, lbCheckName); // ID = 0 vì thêm mới
            if (!IsVali)
            {
                return;
            }

            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn thêm danh mục?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                _categoryController.InsertCategory(txtName.Text, txtDescription.Text);
                ShowScreen.ShowMessage("Thêm thông tin danh mục thành công", "Thông báo");
                _OnLoadCate?.Invoke();
                txtName.Text = txtDescription.Text = String.Empty;
            }
            else
            {
                Console.WriteLine("Cancel add category");
            }
        }
    }
}
