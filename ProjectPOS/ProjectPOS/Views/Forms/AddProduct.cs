using Guna.UI2.WinForms;
using ProjectPOS.Controllers;
using ProjectPOS.Models;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS.Views.Forms
{
    public partial class AddProduct : Form
    {
        private CategoryController _categoryController = new CategoryController();
        private List<CategoryModel> categories = new List<CategoryModel>();
        private string absolutePath = string.Empty;
        private int selectedID = -1;
        private ProductController _productController = new ProductController();
        public event Action _OnLoadProduct;

        public AddProduct()
        {
            InitializeComponent();
            LoadDataCategory();
            listCategory.Visible = false;

        }
        public void LoadDataCategory()
        {
            categories = _categoryController.GetAllCategoryName();
            listCategory.DataSource = null;
            listCategory.DataSource = categories;
            listCategory.DisplayMember = "Name";
            listCategory.ValueMember = "id";

        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
        {

            string searchText = txtCategory.Text.Trim().ToLower();
            if (categories == null || categories.Count == 0)
            {
                listCategory.Visible = false;
                return;
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                lbCheckCategory.Text = string.Empty;
            }
            // Ẩn danh sách nếu chưa nhập ký tự nào
            //if (string.IsNullOrEmpty(searchText))
            //{
            //    listCategory.Visible = false;
            //    return;
            //}

            // LINQ Method Syntax contains xác định xem từ khóa có phải là 1 phần của list không -> true
            var filtered = categories
                .Where(c => c.name.ToLower().Contains(searchText))
                .ToList();

            // Gán lại DataSource cho listCategory
            listCategory.DataSource = null; // Xóa DataSource cũ trước
            listCategory.DataSource = filtered;
            listCategory.DisplayMember = "Name"; // Hiển thị tên danh mục
            listCategory.ValueMember = "id";


            // Nếu có kết quả, hiển thị danh sách; nếu không, ẩn nó đi
            listCategory.Visible = filtered.Any();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCategory_Leave(object sender, EventArgs e)
        {
            if (!listCategory.Focused) // Chỉ ẩn nếu ListBox không được focus
            {
                listCategory.Visible = false;
            }
            string selectedText = listCategory.GetItemText(listCategory.SelectedItem);
            Console.WriteLine("Leave: " + selectedText);
            selectedID = Convert.ToInt32(listCategory.SelectedValue);
            Console.WriteLine("Leave ID: " + selectedID);
        }

        private void listCategory_Click(object sender, EventArgs e)
        {
            if (listCategory.SelectedItem != null)
            {
                string selectedText = listCategory.GetItemText(listCategory.SelectedItem);
                Console.WriteLine("Selected Category: " + selectedText); // Debug
                selectedID = Convert.ToInt32(listCategory.SelectedValue);
                Console.WriteLine("Selected Value: " + selectedID); // Debug


                txtCategory.Text = selectedText;
                listCategory.Visible = false;
            }
            else
            {
                Console.WriteLine("No item selected.");
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                absolutePath = openFileDialog.FileName;
                ptbImageProduct.Image = Image.FromFile(absolutePath);

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsVali = Validation.ValidationAddProduct(txtName.Text, txtCategory.Text, txtPrice.Text, lbCheckName, lbCheckCategory, lbCheckPrice);
            if (!IsVali)
            {
                return;
            }
            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn thêm sản phẩm?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                _productController.InsertProduct(txtName.Text, selectedID, double.Parse(txtPrice.Text, NumberStyles.AllowThousands, CultureInfo.GetCultureInfo("en-US")), txtDescription.Text, absolutePath);
                //ShowScreen.ShowMessage("Thêm thông tin sản phẩm thành công", "Thông báo");
                // Tạo và hiển thị thông báo thành công
                MessageBox.Show(
                "Thêm thông tin sản phẩm thành công", // Nội dung thông báo
                "Thông báo", // Tiêu đề của thông báo
                MessageBoxButtons.OK, // Chỉ có nút OK
                MessageBoxIcon.Information // Biểu tượng thông báo thành công
                );

                _OnLoadProduct?.Invoke();
                txtName.Text = txtDescription.Text = txtCategory.Text = txtPrice.Text = String.Empty;
                ptbImageProduct.Image = null;
                listCategory.Visible = false;
            }
            else
            {
                Console.WriteLine("Cancel add customer");
            }

        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            FormatNumber.FormatAsNumber(txtPrice);
        }

        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && listCategory.SelectedItem != null)
            {
                listCategory_Click(sender, e); // Gọi lại sự kiện Click để chọn sản phẩm
                e.SuppressKeyPress = true;  // Ngăn tiếng "beep"
            }
        }
    }
}
