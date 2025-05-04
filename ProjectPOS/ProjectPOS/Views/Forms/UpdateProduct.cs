using ProjectPOS.Controllers;
using ProjectPOS.Models;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS.Views.Forms
{
    public partial class UpdateProduct : Form
    {
        private string absolutePath = string.Empty;
        private ProductDTO _product = new ProductDTO();
        private CategoryController _categoryController = new CategoryController();
        private List<CategoryModel> categories = new List<CategoryModel>();
        private ProductController _productController = new ProductController();
        private int selectedID = -1;
        public event Action _OnLoadProduct;

        public UpdateProduct()
        {
            InitializeComponent();
            listCategory.Visible = false;
            
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void Init(ProductDTO product)
        {
            if (product != null)
            {
                _product = product;
                txtID.Text = product.ProductID.ToString();
                txtName.Text = product.ProductName;
                txtPrice.Text = product.Price.ToString();
                txtDescription.Text = product.Description;
                
                txtQuantity.Text = product.StockQuantity.ToString();
                Console.WriteLine("Supplier Name: " + product.SupplierName);
                txtSupplier.Text = product.SupplierName;
                dtpCreateAt.Value = product.CreateAt;
                dtpUpdateAt.Value = product.UpdateAt;
                if (!string.IsNullOrEmpty(product.ImageURL))
                {
                    absolutePath = Path.Combine(Application.StartupPath, product.ImageURL);
                    // Kiểm tra file có tồn tại không trước khi load
                    if (File.Exists(absolutePath))
                    {
                        ptbImageProduct.Image = Image.FromFile(absolutePath);
                    }
                    else
                    {
                        MessageBox.Show("Ảnh không tồn tại: " + absolutePath);
                    }
                }
                txtCategory.Text = product.CategoryName;
                LoadDataCategory();

                listCategory.Visible = false;
            }
        }
        public void LoadDataCategory()
        {
            categories = _categoryController.GetAllCategoryName();
            listCategory.DataSource = null;
            listCategory.DataSource = categories;
            listCategory.DisplayMember = "Name";
            listCategory.ValueMember = "id";
            var selectedCategory = categories.FirstOrDefault(c => c.name == txtCategory.Text);

            if (selectedCategory != null)
            {
                listCategory.SelectedValue = selectedCategory.id;
                selectedID = selectedCategory.id;
            }
            Console.WriteLine("Load Category: " + selectedID);
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

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {
            FormatNumber.FormatAsNumber(txtPrice);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            bool IsVali = Validation.ValidationAddProduct(txtName.Text, txtCategory.Text, txtPrice.Text, lbCheckName, lbCheckCategory, lbCheckPrice);
            if (!IsVali)
            {
                return;
            }
            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn cập nhật sản phẩm?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                _productController.UpdateProduct(txtName.Text, selectedID, double.Parse(txtPrice.Text, NumberStyles.AllowThousands, CultureInfo.GetCultureInfo("en-US")), txtDescription.Text, absolutePath, int.Parse(txtID.Text));
                ShowScreen.ShowMessage("Cập nhật thông tin sản phẩm thành công", "Thông báo");
                _OnLoadProduct?.Invoke();
                this.Close();
            }
            else
            {
                Console.WriteLine("Cancel update product");
            }
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
            //Ẩn danh sách nếu chưa nhập ký tự nào
            if (string.IsNullOrEmpty(searchText))
            {
                listCategory.Visible = false;
                return;
            }

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

        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && listCategory.SelectedItem != null)
            {
                listCategory_Click(sender, e); // Gọi lại sự kiện Click để chọn sản phẩm
                e.SuppressKeyPress = true;  // Ngăn tiếng "beep"
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            FormatNumber.FormatAsNumber(txtQuantity);
        }
    }
}
