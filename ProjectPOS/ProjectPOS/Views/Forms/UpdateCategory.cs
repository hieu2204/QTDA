using ProjectPOS.Controllers;
using ProjectPOS.Models;
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
    public partial class UpdateCategory : Form
    {
        private CategoryModel _category = new CategoryModel();
        public event Action _OnLoadCate;
        private CategoryController _categoryController = new CategoryController();
        public UpdateCategory()
        {
            InitializeComponent();
        }
        public void Init(CategoryModel category)
        {
            if(category != null)
            {
                _category = category;
                txtID.Text = category.id.ToString();
                txtName.Text = category.name;
                txtDescription.Text = category.description;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int categoryId = int.Parse(txtID.Text); // Lấy ID của danh mục

            bool IsVali = Validation.ValidationUpdateCategory(txtName.Text, categoryId, lbCheckName);
            if (!IsVali)
            {
                return;
            }

            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn cập nhật danh mục?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                _categoryController.UpdateCategory(categoryId, txtName.Text, txtDescription.Text);
                ShowScreen.ShowMessage("Cập nhật thông tin danh mục thành công", "Thông báo");
                _OnLoadCate?.Invoke();
                this.Close();
            }
            else
            {
                Console.WriteLine("Cancel update category");
            }
        }
    }
}
