using Guna.UI2.WinForms;
using ProjectPOS.Controllers;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Forms;
using ProjectPOS.Models;

namespace ProjectPOS.Views.Forms
{
    public partial class AddStock : Form
    {
        private StockController _stockController = new StockController();
        private ProductController _productController = new ProductController();
        private List<ProductDTO> _products = new List<ProductDTO>();
        private SupplierController _supplierController = new SupplierController();
        private List<SupplierDTO> _suppliers = new List<SupplierDTO>();
        private UserModel _user = new UserModel();
        private object oldValue;
        public Action _OnLoadPurchase;
        public AddStock()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            txtID.Text = _stockController.GetNextInvoiceID().ToString();
            LoadDataProduct();
            LoadDataSupplier();
            SetupDataGridView();
        }
        public void LoadEmployee(UserModel user)
        {
            _user = user;
            if(user != null)
            {
                txtUser.Text = user.Name;
                txtUser.Tag = user.Id;
            }
        }
        public void LoadDataProduct()
        {
            _products = _productController.GetListProduct();
            if(_products != null)
            {
                lstProduct.DataSource = null;
                lstProduct.DataSource = _products;
            }
            lstProduct.Visible = false;
        }
        public void LoadDataSupplier()
        {
            _suppliers = _supplierController.GetSuppliers();
            if( _suppliers != null )
            {
                lstSupplier.DataSource = null;
                lstSupplier.DataSource = _suppliers;
                lstSupplier.DisplayMember = "SupplierName";
                lstSupplier.ValueMember = "SupplierID";
            }
            lstSupplier.Visible = false;
        }
        private void txtProduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProduct.Text))
            {
                lbCheckProduct.Text = String.Empty;
            }
            string keyword = txtProduct.Text.Trim().ToLower();
            // Lọc danh sách sản phẩm theo từ khóa
            var filteredProduct = _products.Where(p=>p.ProductID.ToString().Contains(keyword) 
                                                        || p.ProductName.ToLower().Contains(keyword) 
                                                        || p.CategoryName.ToLower().Contains(keyword)).ToList();
            lstProduct.DataSource = null;
            lstProduct.DataSource = filteredProduct;
            // Nếu có kết quả thì hiển thị ListBox, nếu không thì ẩn
            lstProduct.Visible = filteredProduct.Count > 0;
        }

        private void ptbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void lstProduct_Click(object sender, EventArgs e)
        {
            if (lstProduct.SelectedItem != null)
            {
                ProductDTO selectedProduct = (ProductDTO)lstProduct.SelectedItem;
                // Gán dữ liệu
                txtProduct.Text = selectedProduct.ProductName;
                txtCategory.Text = selectedProduct.CategoryName;
                txtSupplier.Text = selectedProduct.SupplierName;
                txtProduct.Tag = selectedProduct.ProductID;
                if (!string.IsNullOrEmpty(selectedProduct.ImageURL))
                {
                    ptbProduct.Image = Image.FromFile(selectedProduct.ImageURL);
                }
                else
                {
                    ptbProduct.Image = null;
                }
            }
            lstProduct.Visible = false;
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                lbCheckPrice.Text = String.Empty;
            }
            FormatNumber.FormatAsNumber(txtPrice);

        }
        private void txtSupplier_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSupplier.Text))
            {
                lbCheckSupplier.Text = String.Empty;
            }
            string keyword = txtSupplier.Text.Trim().ToLower();
            var filteredSupplier = _suppliers.Where(s=>s.SupplierName.ToLower().Contains(keyword)).ToList();
            lstSupplier.DataSource = null;
            lstSupplier.DataSource = filteredSupplier;
            lstSupplier.DisplayMember = "SupplierName";
            lstSupplier.ValueMember = "SupplierID";
            lstSupplier.Visible = filteredSupplier.Count > 0;
        }

        private void lstSupplier_Click(object sender, EventArgs e)
        {
            if(lstSupplier.SelectedItem != null)
            {
                SupplierDTO selectedSupplier = (SupplierDTO)lstSupplier.SelectedItem;
                txtSupplier.Text = selectedSupplier.SupplierName;
                txtSupplier.Tag = selectedSupplier.SupplierID;
                lstSupplier.Visible = false;
            }
        }
        private void AddProductToCart(ProductDTO product, int quantity, decimal unitPrice, SupplierDTO supplier)
        {
            decimal totalPrice = quantity * unitPrice;
            bool isUpdated = false;
            foreach (DataGridViewRow row in dgvCartDetail.Rows)
            {
                if (row.Cells["ProductID"].Value != null && row.Cells["SupplierName"].Value != null)
                {
                    int existingProductID = Convert.ToInt32(row.Cells["ProductID"].Value);
                    string existingSupplier = row.Cells["SupplierName"].Value.ToString();

                    // Nếu sản phẩm + nhà cung cấp đã có trong giỏ hàng
                    if (existingProductID == product.ProductID && existingSupplier == supplier.SupplierName)
                    {
                        // Cập nhật số lượng
                        int existingQuantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                        row.Cells["Quantity"].Value = existingQuantity + quantity;

                        // Cập nhật tổng tiền
                        decimal existingTotalPrice = Convert.ToDecimal(row.Cells["TotalPrice"].Value);
                        row.Cells["TotalPrice"].Value = existingTotalPrice + totalPrice;

                        isUpdated = true;
                        break;
                    }
                }
            }
            // Nếu chưa tồn tại, thêm hàng mới
            if (!isUpdated)
            {
                dgvCartDetail.Rows.Add(
                    product.ProductID,
                    product.ProductName,
                    product.CategoryName,
                    supplier.SupplierID,
                    supplier.SupplierName,
                    quantity,
                    unitPrice,
                    totalPrice
                );
            }
        }
        private void SetupDataGridView()
        {
            dgvCartDetail.AutoGenerateColumns = false;
            dgvCartDetail.ColumnCount = 8;
            dgvCartDetail.ColumnHeadersHeight = 50;
            dgvCartDetail.RowTemplate.Height = 40;

            dgvCartDetail.Columns[0].Name = "ProductID";
            dgvCartDetail.Columns[0].HeaderText = "Mã sản phẩm";
            dgvCartDetail.Columns[0].Visible = false;

            dgvCartDetail.Columns[1].Name = "ProductName";
            dgvCartDetail.Columns[1].HeaderText = "Tên sản phẩm";
            dgvCartDetail.Columns[1].ReadOnly = true;

            dgvCartDetail.Columns[2].Name = "CategoryName";
            dgvCartDetail.Columns[2].HeaderText = "Loại sản phẩm";
            dgvCartDetail.Columns[3].ReadOnly = true;

            dgvCartDetail.Columns[3].Name = "SupplierID";
            dgvCartDetail.Columns[3].HeaderText = "Mã nhà cung cấp";
            dgvCartDetail.Columns[3].Visible = false;

            dgvCartDetail.Columns[4].Name = "SupplierName"; 
            dgvCartDetail.Columns[4].HeaderText = "Nhà cung cấp";
            dgvCartDetail.Columns[4].ReadOnly = true;

            dgvCartDetail.Columns[5].Name = "Quantity";
            dgvCartDetail.Columns[5].HeaderText = "Số lượng";
            dgvCartDetail.Columns[5].ValueType = typeof(int);
            dgvCartDetail.Columns["Quantity"].DefaultCellStyle.Format = "N0";

            dgvCartDetail.Columns[6].Name = "UnitPrice";
            dgvCartDetail.Columns[6].HeaderText = "Giá nhập";
            dgvCartDetail.Columns[6].ValueType = typeof(decimal);
            dgvCartDetail.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";

            dgvCartDetail.Columns[7].Name = "TotalPrice";
            dgvCartDetail.Columns[7].HeaderText = "Thành tiền";
            dgvCartDetail.Columns[7].ValueType = typeof(decimal);
            dgvCartDetail.Columns["TotalPrice"].DefaultCellStyle.Format = "N0";
            dgvCartDetail.Columns["TotalPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // 💡 Mặc định fill toàn bảng khi khởi tạo
            dgvCartDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCartDetail.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvCartDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvCartDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCartDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCartDetail.ScrollBars = ScrollBars.Vertical;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool IsValid = Validation.ValidationAddCartStock(txtProduct.Text, txtCategory.Text, txtSupplier.Text, txtQuantity.Text, txtPrice.Text, lbCheckProduct,
                lbCheckCategory, lbCheckSupplier, lbCheckQuantity, lbCheckPrice);
            if (!IsValid)
            {
                return;
            }
            if(txtProduct.Tag == null)
            {
                ShowScreen.ShowMessage("Vui lòng chọn sản phẩm từ danh sách?");
                return;
            }
            if (txtSupplier.Tag == null)
            {
                ShowScreen.ShowMessage("Vui lòng chọn nhà cung cấp từ danh sách?");
                return;
            }
            int productID = (int)txtProduct.Tag; // Lấy productID từ Tag
            int supplierID = (int)txtSupplier.Tag; // Lấy SupplierID từ Tag
            var selectedProduct = _products.FirstOrDefault(p=> p.ProductID == productID);
            if(selectedProduct == null)
            {
                ShowScreen.ShowMessage("Không tìm thấy sản phẩm?");
                return;
            }
            var selectedSupplier = _suppliers.FirstOrDefault(s => s.SupplierID == supplierID);
            if (selectedSupplier == null)
            {
                ShowScreen.ShowMessage("Không tìm thấy nhà cung cấp?");
                return;
            }
            int quantity = int.Parse(txtQuantity.Text, NumberStyles.AllowThousands, CultureInfo.CurrentCulture);
            decimal price = decimal.Parse(txtPrice.Text, NumberStyles.Currency, CultureInfo.CurrentCulture);
            AddProductToCart(selectedProduct, quantity, price, selectedSupplier);
            // 🔄 Cập nhật lại chế độ cột sau khi thêm dữ liệu
            dgvCartDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            txtProduct.Text = txtSupplier.Text = txtPrice.Text = txtCategory.Text = txtQuantity.Text = string.Empty;
            ptbProduct.Image = null;
            lstProduct.Visible = lstSupplier.Visible = false;
            UpdateTotalAmount();
        }
        public void UpdateTotalAmount()
        {
            decimal totalAmount = 0;
            if (dgvCartDetail.Rows.Count < 0)
            {
                return;
            }
            foreach (DataGridViewRow row in dgvCartDetail.Rows)
            {
                totalAmount += Convert.ToDecimal(row.Cells["TotalPrice"].Value);
            }
            txtTotalAmount.Text = totalAmount.ToString();
            FormatNumber.FormatAsNumber(txtTotalAmount);
        }
        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtQuantity.Text))
            {
                lbCheckQuantity.Text = String.Empty;
            }
            FormatNumber.FormatAsNumber(txtQuantity);
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCategory.Text))
            {
                lbCheckCategory.Text = String.Empty;
            }
        }

        private void dgvCartDetail_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            oldValue = dgvCartDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        }

        private void dgvCartDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dgvCartDetail.Columns["Quantity"].Index)
            {
                try
                {
                    int quantity = Convert.ToInt32(dgvCartDetail.Rows[e.RowIndex].Cells["Quantity"].Value);
                    if (quantity <= 0)
                    {
                        ShowScreen.ShowMessage("Số lượng phải lớn hơn 0");
                        dgvCartDetail.Rows[e.RowIndex].Cells["Quantity"].Value = oldValue;
                    }
                }
                catch
                {
                    ShowScreen.ShowMessage("Số lượng không hợp lệ");
                    dgvCartDetail.Rows[e.RowIndex].Cells["Quantity"].Value = oldValue; // khôi phục lại giá trị cũ
                }
            }
            if (e.ColumnIndex == dgvCartDetail.Columns["UnitPrice"].Index) 
            {
                try
                {
                    decimal price = Convert.ToDecimal(dgvCartDetail.Rows[e.RowIndex].Cells["UnitPrice"].Value);
                    if(price < 0)
                    {
                        ShowScreen.ShowMessage("Giá không thể nhỏ hơn 0!");
                        dgvCartDetail.Rows[e.RowIndex].Cells["UnitPrice"].Value = oldValue;
                    }
                }
                catch
                {
                    ShowScreen.ShowMessage("Giá không hợp lệ!");
                    dgvCartDetail.Rows[e.RowIndex].Cells["UnitPrice"].Value = oldValue; // Khôi phục lại giá trị cũ
                }
            }
            if(e.ColumnIndex == dgvCartDetail.Columns["Quantity"].Index || e.ColumnIndex == dgvCartDetail.Columns["UnitPrice"].Index)
            {
                int quantity = Convert.ToInt32(dgvCartDetail.Rows[e.RowIndex].Cells["Quantity"].Value);
                int price = Convert.ToInt32(dgvCartDetail.Rows[e.RowIndex].Cells["UnitPrice"].Value);
                dgvCartDetail.Rows[e.RowIndex].Cells["TotalPrice"].Value = quantity * price;
            }
            UpdateTotalAmount();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgvCartDetail.CurrentRow != null)
            {
                DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có muốn xóa sản phẩm khỏi giỏ không?");
                if(result == DialogResult.Yes)
                {
                    dgvCartDetail.Rows.Remove(dgvCartDetail.CurrentRow);
                } 
            }
            UpdateTotalAmount();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if(dgvCartDetail.Rows.Cast<DataGridViewRow>().All(r=>r.IsNewRow))
            {
                dgvCartDetail.DataSource = null;
                ShowScreen.ShowMessage("Vui lòng nhập sản phẩm vào giỏ");
                return;
            }
            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn nhập hàng?");
            if(result == DialogResult.Yes)
            {
                _stockController.SubmitStockReceipt(int.Parse(txtUser.Tag.ToString()), decimal.Parse(txtTotalAmount.Text), dgvCartDetail);
                ShowScreen.ShowMessage("Nhập hàng thành công");
                this.Close();
                _OnLoadPurchase?.Invoke();
            }
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstProduct.SelectedItem != null)
            {
                lstProduct_Click(sender, e); // Gọi lại sự kiện Click để chọn sản phẩm
                e.SuppressKeyPress = true;  // Ngăn tiếng "beep"
            }
        }

        private void txtSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstSupplier.SelectedItem != null)
            {
                lstSupplier_Click(sender, e); // Gọi lại sự kiện Click để chọn sản phẩm
                e.SuppressKeyPress = true;  // Ngăn tiếng "beep"
            }
        }

    }
}
