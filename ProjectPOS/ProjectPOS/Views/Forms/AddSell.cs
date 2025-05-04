using ProjectPOS.Controllers;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using ProjectPOS.Utilities;
using System.Globalization;
using TestStack.White.InputDevices;

namespace ProjectPOS.Views.Forms
{
    public partial class AddSell : Form
    {
        private UserModel _user = new UserModel();
        private SellController _sellController = new SellController();
        private List<ProductDTO> _products = new List<ProductDTO>();
        private List<CustomerDTO> _customers = new List<CustomerDTO>();
        private ProductController _productController = new ProductController();
        private CustomerController _customerController = new CustomerController();
        private object _previousQuantityValue;
        public Action OnLoadSell;

        public AddSell()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            txtID.Text = _sellController.GetNextSellID().ToString();
            LoadDataProduct();
            LoadDataCustomer();
            SetupDataGridView();
            lbCheckProduct.Text = lbCheckQuantity.Text = String.Empty;
        }
        public void LoadEmployee(UserModel user)
        {
            _user = user;
            if (user != null)
            {
                txtUser.Text = user.Name;
                txtUser.Tag = user.Id;
            }
        }
        public void LoadDataProduct()
        {
            _products = _productController.GetListProduct();
            if (_products != null)
            {
                lstProduct.DataSource = null;
                lstProduct.DataSource = _products;
            }
            lstProduct.Visible = false;
        }
        public void LoadDataCustomer()
        {
            _customers = _customerController.GetCusList();
            if(_customers != null)
            {
                lstCustomer.DataSource = null;
                lstCustomer.DataSource = _customers;
            }
            lstCustomer.Visible = false;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtProduct_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtProduct.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                // Khi không có từ khóa, hiển thị toàn bộ sản phẩm
                //lstProduct.DataSource = _products;
                lstProduct.Visible = false;
            }
            else
            {
                lbCheckProduct.Text = string.Empty;
                // Lọc danh sách sản phẩm theo từ khóa
                var filteredProduct = _products.Where(p => p.ProductID.ToString().Contains(keyword)
                                                            || p.ProductName.ToLower().Contains(keyword)
                                                            || p.CategoryName.ToLower().Contains(keyword))
                                                .ToList();
                lstProduct.DataSource = filteredProduct;
                // Hiển thị ListBox nếu có kết quả, ẩn nếu không
                lstProduct.Visible = filteredProduct.Count > 0;
            }
        }

        private void lstProduct_Click(object sender, EventArgs e)
        {
            if (lstProduct.SelectedItem != null)
            {
                ProductDTO selectedProduct = (ProductDTO)lstProduct.SelectedItem;
                // Gán dữ liệu
                txtProduct.Text = selectedProduct.ProductName;
                txtProduct.Tag = selectedProduct.ProductID;
                Console.WriteLine("Price: " + selectedProduct.Price);
                txtPrice.Text = selectedProduct.Price.ToString();
            }
            lstProduct.Visible = false;
        }


        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            FormatNumber.FormatAsNumber(txtPrice);
        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtCustomer.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                // Khi không có từ khóa, hiển thị toàn bộ khách hàng
                //lstCustomer.DataSource = _customers;
                lstCustomer.Visible = false;
            }
            else
            {
                // Lọc danh sách khách hàng theo từ khóa
                var filteredCustomer = _customers.Where(c => c.CustomerID.ToString().Contains(keyword)
                                                              || c.CustomerName.ToLower().Contains(keyword)
                                                              || c.Phone.ToLower().Contains(keyword))
                                                   .ToList();
                lstCustomer.DataSource = filteredCustomer;
                // Hiển thị ListBox nếu có kết quả, ẩn nếu không
                lstCustomer.Visible = filteredCustomer.Count > 0;
            }
        }

        private void lstCustomer_Click(object sender, EventArgs e)
        {
            if (lstCustomer.SelectedItem != null)
            {
                CustomerDTO selectedCustomer = (CustomerDTO)lstCustomer.SelectedItem;
                // Gán dữ liệu
                txtCustomer.Text = selectedCustomer.CustomerName;
                txtCustomer.Tag = selectedCustomer.CustomerID;
                Console.WriteLine("CustomerID: " + txtCustomer.Tag);
                Console.WriteLine("LoyaltyPoints: " + selectedCustomer.LoyaltyPoints);
                ApplyPromotions();
            }
            lstCustomer.Visible = false;
        }
        private void SetupDataGridView()
        {
            // Tổng số cột: 7 (0:ProductID, 1:ProductName, 2:Quantity, 3:GiftQuantity, 4:UnitPrice, 5:TotalPrice, 6:IsGift)
            dgvCartDetail.AutoGenerateColumns = false;
            dgvCartDetail.ColumnCount = 7;
            dgvCartDetail.ColumnHeadersHeight = 50;

            // Cột 0: ProductID (ẩn)
            dgvCartDetail.Columns[0].Name = "ProductID";
            dgvCartDetail.Columns[0].HeaderText = "Mã sản phẩm";
            dgvCartDetail.Columns[0].Visible = false;

            // Cột 1: ProductName
            dgvCartDetail.Columns[1].Name = "ProductName";
            dgvCartDetail.Columns[1].HeaderText = "Tên sản phẩm";
            dgvCartDetail.Columns[1].ReadOnly = true;

            // Cột 2: Số lượng mua
            dgvCartDetail.Columns[2].Name = "Quantity";
            dgvCartDetail.Columns[2].HeaderText = "Số lượng mua";
            dgvCartDetail.Columns[2].ValueType = typeof(int);
            dgvCartDetail.Columns["Quantity"].DefaultCellStyle.Format = "N0";

            // Cột 3: Số lượng tặng (GiftQuantity)
            dgvCartDetail.Columns[3].Name = "GiftQuantity";
            dgvCartDetail.Columns[3].HeaderText = "Số lượng tặng";
            dgvCartDetail.Columns[3].ValueType = typeof(int);
            dgvCartDetail.Columns["GiftQuantity"].DefaultCellStyle.Format = "N0";
            // Bạn có thể đặt cột này read-only nếu muốn
            dgvCartDetail.Columns["GiftQuantity"].ReadOnly = true;

            // Cột 4: UnitPrice
            dgvCartDetail.Columns[4].Name = "UnitPrice";
            dgvCartDetail.Columns[4].HeaderText = "Giá bán";
            dgvCartDetail.Columns[4].ValueType = typeof(decimal);
            dgvCartDetail.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";

            // Cột 5: TotalPrice
            dgvCartDetail.Columns[5].Name = "TotalPrice";
            dgvCartDetail.Columns[5].HeaderText = "Thành tiền";
            dgvCartDetail.Columns[5].ValueType = typeof(decimal);
            dgvCartDetail.Columns["TotalPrice"].DefaultCellStyle.Format = "N0";
            dgvCartDetail.Columns["TotalPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Cột 6: IsGift (dùng cho xử lý nội bộ, có thể ẩn đi)
            dgvCartDetail.Columns[6].Name = "IsGift";
            dgvCartDetail.Columns[6].HeaderText = "Quà tặng";
            dgvCartDetail.Columns[6].ValueType = typeof(bool);
            dgvCartDetail.Columns[6].Visible = false;

            dgvCartDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCartDetail.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvCartDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvCartDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCartDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCartDetail.ScrollBars = ScrollBars.Vertical;
        }

        private bool ValidateProductInput()
        {
            bool isValid = true;
            lbCheckProduct.Text = string.Empty;
            lbCheckQuantity.Text = string.Empty;

            // Kiểm tra sản phẩm
            if (txtProduct.Tag == null)
            {
                lbCheckProduct.Text = "Vui lòng chọn sản phẩm!";
                lbCheckProduct.ForeColor = Color.Red;
                isValid = false;
            }

            // Kiểm tra số lượng
            if (!int.TryParse(txtQuantity.Text, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture , out int quantity) || quantity <= 0)
            {
                lbCheckQuantity.Text = "Số lượng không hợp lệ!";
                lbCheckQuantity.ForeColor = Color.Red;
                isValid = false;
            }

            // Kiểm tra số lượng tồn kho
            if (isValid && txtProduct.Tag != null)
            {
                int productId = (int)txtProduct.Tag;
                int stockQuantity = _productController.GetToTalQuantity(productId);
                if (quantity > stockQuantity)
                {
                    lbCheckQuantity.Text = $"Số lượng tồn kho không đủ! Chỉ còn {stockQuantity} sản phẩm.";
                    lbCheckQuantity.ForeColor = Color.Red;
                    isValid = false;
                }
            }
            return isValid;
        }

        private void CalculateTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvCartDetail.Rows)
            {
                if (row.IsNewRow) continue;

                // Chỉ tính các hàng không phải quà tặng
                if ((bool)row.Cells["IsGift"].Value)
                    continue;

                var value = row.Cells["TotalPrice"].Value;
                if (value != null && value != DBNull.Value && decimal.TryParse(value.ToString(), out decimal price))
                {
                    total += price;
                }
            }
            txtFinalPrice.Text = total.ToString("N0") + " VND";
        }

        private void ApplyPromotions()
        {
            // Lấy danh sách khuyến mãi
            var promotions = new PromotionController().GetAllPromotion();
            decimal sumOriginal = 0;

            // Duyệt qua từng dòng sản phẩm trong giỏ hàng (không tính quà tặng)
            foreach (DataGridViewRow row in dgvCartDetail.Rows)
            {
                if (row.IsNewRow || (bool)row.Cells["IsGift"].Value)
                    continue;

                if (!int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int quantity))
                    continue;
                if (!decimal.TryParse(row.Cells["UnitPrice"].Value?.ToString(), out decimal unitPrice))
                    continue;

                // Tính giá gốc của dòng sản phẩm
                decimal originalPrice = quantity * unitPrice;
                decimal finalPrice = originalPrice;
                int totalGiftQuantity = 0;

                // Duyệt qua các khuyến mãi áp dụng cho sản phẩm này (giả sử promo.ProductID trùng với ProductID của dòng)
                foreach (PromotionDTO promo in promotions.Where(p => p.ProductID == (int)row.Cells["ProductID"].Value))
                {
                    // Áp dụng giảm giá theo phần trăm
                    if (promo.DiscountType.Equals("Percentage", StringComparison.OrdinalIgnoreCase) && promo.DiscountValue.HasValue)
                    {
                        // Nếu có điều kiện ngưỡng giảm giá, kiểm tra trước khi áp dụng
                        if (!promo.MinTotalAmount.HasValue || originalPrice >= promo.MinTotalAmount.Value)
                        {
                            finalPrice -= originalPrice * (promo.DiscountValue.Value / 100);
                        }
                    }
                    // Áp dụng giảm giá theo tiền mặt
                    else if (promo.DiscountType.Equals("Amount", StringComparison.OrdinalIgnoreCase) && promo.DiscountValue.HasValue)
                    {
                        if (!promo.MinTotalAmount.HasValue || originalPrice >= promo.MinTotalAmount.Value)
                        {
                            finalPrice -= promo.DiscountValue.Value;
                        }
                    }
                    // Tính số lượng quà tặng (nếu có)
                    if (promo.GiftProductID.HasValue && promo.MinQuantity.HasValue && promo.GiftQuantity.HasValue)
                    {
                        // Cộng dồn số quà tặng từ nhiều khuyến mãi
                        totalGiftQuantity += (quantity / promo.MinQuantity.Value) * promo.GiftQuantity.Value;
                    }
                }

                if (finalPrice < 0)
                    finalPrice = 0;

                // Cập nhật giá và số lượng quà tặng cho dòng sản phẩm
                row.Cells["TotalPrice"].Value = finalPrice;
                row.Cells["GiftQuantity"].Value = totalGiftQuantity;

                // Cộng dồn tổng giá đã giảm cho các sản phẩm
                sumOriginal += finalPrice;
            }

            // Áp dụng khuyến mãi toàn đơn (order-level promotions)
            decimal orderDiscount = 0;
            var orderPromos = promotions.Where(p => p.AppliesToOrder == 1);
            foreach (PromotionDTO promo in orderPromos)
            {
                if (promo.MinTotalAmount.HasValue && sumOriginal >= promo.MinTotalAmount.Value)
                {
                    if (promo.DiscountType.Equals("Percentage", StringComparison.OrdinalIgnoreCase) && promo.DiscountValue.HasValue)
                    {
                        orderDiscount += sumOriginal * (promo.DiscountValue.Value / 100);
                    }
                    else if (promo.DiscountType.Equals("Amount", StringComparison.OrdinalIgnoreCase) && promo.DiscountValue.HasValue)
                    {
                        orderDiscount += promo.DiscountValue.Value;
                    }
                }
            }

            decimal finalTotal = sumOriginal - orderDiscount;
            if (finalTotal < 0)
                finalTotal = 0;

            // Nếu khách hàng được chọn, trừ điểm tích lũy
            if (txtCustomer.Tag != null)
            {
                int customerId = (int)txtCustomer.Tag;
                CustomerDTO customer = _customers.FirstOrDefault(c => c.CustomerID == customerId);
                if (customer != null && customer.LoyaltyPoints > 0)
                {
                    // Ví dụ: mỗi 1 điểm tích lũy giảm 1.000 VND
                    decimal loyaltyDiscount = customer.LoyaltyPoints * 1000m;
                    finalTotal -= loyaltyDiscount;
                    if (finalTotal < 0)
                        finalTotal = 0;
                }
            }

            // Cập nhật giao diện hiển thị tổng giá
            txtOriginalPrice.Text = sumOriginal.ToString("N0") + " VND";
            txtFinalPrice.Text = finalTotal.ToString("N0") + " VND";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateProductInput())
                return;

            int productId = (int)txtProduct.Tag;
            string productName = txtProduct.Text;
            int quantity = int.Parse(txtQuantity.Text, NumberStyles.Any, CultureInfo.CurrentCulture);
            decimal price = decimal.Parse(txtPrice.Text);
            decimal totalPrice = quantity * price;
            bool isExist = false;

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa (chỉ cập nhật nếu không phải quà tặng)
            foreach (DataGridViewRow row in dgvCartDetail.Rows)
            {
                if (row.IsNewRow) continue;

                var cellValue = row.Cells["ProductID"].Value;
                if (cellValue != null && cellValue != DBNull.Value && int.TryParse(cellValue.ToString(), out int existingProductId))
                {
                    // Cập nhật số lượng và thành tiền chỉ với các sản phẩm mua (không tính quà tặng)
                    if (existingProductId == productId && !(bool)row.Cells["IsGift"].Value)
                    {
                        int currentQuantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                        row.Cells["Quantity"].Value = currentQuantity + quantity;
                        row.Cells["TotalPrice"].Value = (currentQuantity + quantity) * price;
                        isExist = true;
                        break;
                    }
                }
            }

            // Nếu chưa có sản phẩm trong giỏ hàng, thêm mới
            if (!isExist)
            {
                // Thêm hàng với GiftQuantity mặc định = 0 và IsGift = false
                dgvCartDetail.Rows.Add(productId, productName, quantity, 0, price, totalPrice, false);
            }

            // Gọi ApplyPromotions để cập nhật giảm giá và số lượng tặng nếu có
            ApplyPromotions();

            txtProduct.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
            txtProduct.Tag = null;
            lbCheckProduct.Text = string.Empty;
            lbCheckQuantity.Text = string.Empty;
        }
        private void ApplyGiftProduct(int productId, int quantity)
        {
            var promotions = new PromotionController().GetAllPromotion();

            foreach (PromotionDTO promo in promotions)
            {
                
                if (promo.ProductID == productId && promo.GiftProductID.HasValue
                    && promo.MinQuantity.HasValue && promo.GiftQuantity.HasValue)
                {
                    int giftProductId = promo.GiftProductID.Value;
                    int minQuantity = promo.MinQuantity.Value;
                    int giftQuantity = promo.GiftQuantity.Value;
                    int freeItems = (quantity / minQuantity) * giftQuantity;

                    if (freeItems > 0)
                    {
                        // Tìm tên sản phẩm quà tặng
                        string giftProductName = _products.FirstOrDefault(p => p.ProductID == giftProductId)?.ProductName ?? "Sản phẩm không xác định";
                        giftProductName += " - Quà tặng"; // Thêm chữ "Quà tặng"

                        dgvCartDetail.Rows.Add(giftProductId, giftProductName, freeItems, 0, 0, true);
                    }
                }
            }
        }


        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            FormatNumber.FormatAsNumber(txtQuantity);
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstProduct.SelectedItem != null)
            {
                lstProduct_Click(sender, e); // Gọi lại sự kiện Click để chọn sản phẩm
                e.SuppressKeyPress = true;  // Ngăn tiếng "beep"
            }
        }

        private void txtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstCustomer.SelectedItem != null)
            {
                lstCustomer_Click(sender, e); // Gọi lại sự kiện Click để chọn sản phẩm
                e.SuppressKeyPress = true;  // Ngăn tiếng "beep"
            }
        }

        private void txtProduct_Leave(object sender, EventArgs e)
        {
            // Nếu focus không chuyển sang lstProduct thì ẩn ListBox
            if (!lstProduct.Focused)
            {
                lstProduct.Visible = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu có hàng nào được chọn
            if (dgvCartDetail.SelectedRows.Count > 0)
            {
                // Xóa từng hàng được chọn
                foreach (DataGridViewRow row in dgvCartDetail.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        dgvCartDetail.Rows.Remove(row);
                    }
                }
                // Cập nhật lại các giá trị, ví dụ tổng hóa đơn
                ApplyPromotions();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void dgvCartDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCartDetail.Columns[e.ColumnIndex].Name == "Quantity")
            {
                // Lấy giá trị mới nhập vào
                var cellValue = dgvCartDetail.Rows[e.RowIndex].Cells["Quantity"].Value;
                string input = cellValue?.ToString() ?? string.Empty;

                // Kiểm tra nếu không phải số hoặc số ≤ 0
                if (!int.TryParse(input, out int newQuantity) || newQuantity <= 0)
                {
                    MessageBox.Show("Số lượng không hợp lệ! Giá trị sẽ quay lại số lượng trước đó.",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvCartDetail.Rows[e.RowIndex].Cells["Quantity"].Value = _previousQuantityValue;
                }
                else
                {
                    // Kiểm tra số lượng tồn kho
                    int productId = Convert.ToInt32(dgvCartDetail.Rows[e.RowIndex].Cells["ProductID"].Value);
                    int stockQuantity = _productController.GetToTalQuantity(productId);
                    if (newQuantity > stockQuantity)
                    {
                        MessageBox.Show($"Số lượng tồn kho không đủ! Chỉ còn {stockQuantity} sản phẩm. Giá trị sẽ quay lại số lượng trước đó.",
                                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvCartDetail.Rows[e.RowIndex].Cells["Quantity"].Value = _previousQuantityValue;
                    }
                }

                // Cập nhật lại cột TotalPrice dựa trên số lượng và đơn giá
                DataGridViewRow row = dgvCartDetail.Rows[e.RowIndex];
                if (int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int quantity) &&
                    decimal.TryParse(row.Cells["UnitPrice"].Value?.ToString(), out decimal unitPrice))
                {
                    row.Cells["TotalPrice"].Value = quantity * unitPrice;
                }
                // Gọi lại ApplyPromotions để cập nhật tổng hóa đơn và các khuyến mãi
                ApplyPromotions();
            }
        }

        private void dgvCartDetail_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvCartDetail.Columns[e.ColumnIndex].Name == "Quantity")
            {
                _previousQuantityValue = dgvCartDetail.Rows[e.RowIndex].Cells["Quantity"].Value;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu giỏ hàng không có sản phẩm nào (không tính dòng mới)
            if (dgvCartDetail.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("Vui lòng thêm sản phẩm vào giỏ hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int paymentStatus = cmbPaymentStatus.SelectedItem.ToString().Equals("Tiền mặt") ? 1 : 2;
            Console.WriteLine("Payment: " + paymentStatus);
            // Lấy các tham số cần thiết từ giao diện:
            int userId = (int)txtUser.Tag;
            int? customerId = txtCustomer.Tag != null ? (int?)txtCustomer.Tag : null;
            decimal totalAmount = Decimal.Parse(txtOriginalPrice.Text.Replace(" VND", "").Replace(",", ""));
            decimal finalTotalAmount = Decimal.Parse(txtFinalPrice.Text.Replace(" VND", "").Replace(",", ""));
            SellController controller = new SellController();
            controller.SaveSellOrder(userId, totalAmount, finalTotalAmount, dgvCartDetail, customerId, paymentStatus);
            MessageBox.Show("Đơn hàng đã được lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            OnLoadSell?.Invoke();
        }
    }
}
