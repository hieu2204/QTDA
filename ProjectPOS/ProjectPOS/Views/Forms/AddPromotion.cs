using Castle.DynamicProxy.Generators.Emitters;
using ProjectPOS.Controllers;
using ProjectPOS.Models.DTOs;
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
    public partial class AddPromotion : Form
    {
        private List<ProductDTO> _products = new List<ProductDTO>();
        private ProductController _productController = new ProductController();
        private PromotionDTO _promotionDTO = new PromotionDTO();
        private PromotionController _promotionController = new PromotionController();
        public Action OnLoadPromotion;

        public AddPromotion()
        {
            InitializeComponent();
            cboDiscountType_SelectedIndexChanged(this, EventArgs.Empty);
            LoadDataProduct();
            lbCheckName.Text = lbCheckDiscountValue.Text = lbCheckMinBill.Text = lbCheckEndDate.Text = lbCheckGiftProduct.Text = lbCheckGiftQuantity.Text = lbCheckMinQuantity.Text = lbCheckProduct.Text = String.Empty;
        }

        private void ptbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void LoadDataProduct()
        {
            _products = _productController.GetListProduct();
            if (_products != null)
            {
                lstProduct.DataSource = null;
                lstProduct.DataSource = _products;
                lstGiftProduct.DataSource = null;
                lstGiftProduct.DataSource = _products;
            }
            lstProduct.Visible = false;
            lstGiftProduct.Visible = false;
        }
        private void txtProduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProduct.Text))
            {
                lbCheckProduct.Text = String.Empty;
            }
            string keyword = txtProduct.Text.Trim().ToLower();
            // Lọc danh sách sản phẩm theo từ khóa
            var filteredProduct = _products.Where(p => p.ProductID.ToString().Contains(keyword)
                                                        || p.ProductName.ToLower().Contains(keyword)
                                                        || p.CategoryName.ToLower().Contains(keyword)).ToList();
            lstProduct.DataSource = null;
            lstProduct.DataSource = filteredProduct;
            // Nếu có kết quả thì hiển thị ListBox, nếu không thì ẩn
            lstProduct.Visible = filteredProduct.Count > 0;
        }
        private void cboDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if(cboDiscountType.SelectedIndex == 0)
            {
                pnDiscountValue.Visible = true;
                pnGift.Visible = false;
                txtProduct.Text = txtMinQuantity.Text = txtGiftProduct.Text = txtGiftQuantity.Text = String.Empty;
                txtDiscountValue.PlaceholderText = "0 - 100%";
            }
            else if(cboDiscountType.SelectedIndex == 1)
            {
                pnDiscountValue.Visible = true;
                pnGift.Visible = false;
                txtProduct.Text = txtMinQuantity.Text = txtGiftProduct.Text = txtGiftQuantity.Text = String.Empty;
                txtDiscountValue.PlaceholderText = "VNĐ";
            }
            else
            {
                pnDiscountValue.Visible = false;
                pnGift.Visible = true;
                txtDiscountValue.Text = String.Empty;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra tiêu đề khuyến mãi
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                lbCheckName.Text = "Tiêu đề không được để trống!";
                lbCheckName.ForeColor = Color.Red;
                return;
            }

            // Kiểm tra ngày bắt đầu và ngày kết thúc
            if (dtpStartDate.Value > dtpEndDate.Value)
            {
                lbCheckEndDate.Text = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!";
                lbCheckEndDate.ForeColor = Color.Red;
                return;
            }

            // Lấy loại giảm giá từ combobox
            string discountType = cboDiscountType.SelectedItem?.ToString();
            if (discountType == null)
            {
                MessageBox.Show("Vui lòng chọn loại giảm giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xử lý loại bỏ dấu phân cách hàng nghìn trước khi parse
            string NormalizeInput(string input) => input.Replace(",", "").Replace(".", "").Trim();

            // Lấy dữ liệu nhập vào
            int? productID = int.TryParse(txtProduct.Tag?.ToString(), out int pID) ? pID : (int?)null;
            int? giftProductID = int.TryParse(txtGiftProduct.Tag?.ToString(), out int gID) ? gID : (int?)null;
            int? minQuantity = int.TryParse(NormalizeInput(txtMinQuantity.Text), NumberStyles.Integer, CultureInfo.InvariantCulture, out int minQ) ? minQ : (int?)null;
            int? giftProductQuantity = int.TryParse(NormalizeInput(txtGiftQuantity.Text), NumberStyles.Integer, CultureInfo.InvariantCulture, out int giftQ) ? giftQ : (int?)null;
            decimal? discountValue = decimal.TryParse(NormalizeInput(txtDiscountValue.Text), NumberStyles.Float, CultureInfo.InvariantCulture, out decimal discVal) ? discVal : (decimal?)null;
            decimal? minBillAmount = decimal.TryParse(NormalizeInput(txtMinBillAmount.Text), NumberStyles.Float, CultureInfo.InvariantCulture, out decimal minBill) ? minBill : (decimal?)null;

            // Kiểm tra điều kiện áp dụng
            if (discountType.Equals("Percentage", StringComparison.OrdinalIgnoreCase) || discountType.Equals("Amount", StringComparison.OrdinalIgnoreCase))
            {
                txtProduct.Text = txtMinQuantity.Text = txtGiftQuantity.Text = txtGiftProduct.Text = String.Empty;
                minQuantity = giftProductQuantity = null;

                if (!discountValue.HasValue || discountValue <= 0 || (discountType == "Percentage" && discountValue > 100))
                {
                    lbCheckDiscountValue.Text = "Giá trị giảm giá không hợp lệ!";
                    lbCheckDiscountValue.ForeColor = Color.Red;
                    return;
                }

                if (!minBillAmount.HasValue || minBillAmount <= 0)
                {
                    lbCheckMinBill.Text = "Điều kiện áp dụng giảm giá phải lớn hơn 0!";
                    lbCheckMinBill.ForeColor = Color.Red;
                    return;
                }
            }
            else if (discountType.Equals("GiftProduct", StringComparison.OrdinalIgnoreCase))
            {
                txtDiscountValue.Text = txtMinBillAmount.Text = String.Empty;

                if (!productID.HasValue)
                {
                    lbCheckProduct.Text = "Vui lòng nhập sản phẩm cần mua để áp dụng điều kiện.";
                    lbCheckProduct.ForeColor = Color.Red;
                    return;
                }

                if (!minQuantity.HasValue || minQuantity <= 0)
                {
                    lbCheckMinQuantity.Text = "Số lượng tối thiểu phải lớn hơn 0.";
                    lbCheckMinQuantity.ForeColor = Color.Red;
                    return;
                }

                int totalQuantity = _productController.GetToTalQuantity(productID.Value);
                if (totalQuantity < minQuantity)
                {
                    lbCheckMinQuantity.Text = "Số lượng sản phẩm chưa đủ điều kiện khuyến mãi!";
                    lbCheckMinQuantity.ForeColor = Color.Red;
                    return;
                }

                if (!giftProductID.HasValue)
                {
                    lbCheckGiftProduct.Text = "Vui lòng nhập sản phẩm cần tặng.";
                    lbCheckGiftProduct.ForeColor = Color.Red;
                    return;
                }

                if (!giftProductQuantity.HasValue || giftProductQuantity <= 0)
                {
                    lbCheckGiftQuantity.Text = "Số lượng quà tặng không hợp lệ!";
                    lbCheckGiftQuantity.ForeColor = Color.Red;
                    return;
                }

                int totalGiftStock = _productController.GetToTalQuantity(giftProductID.Value);
                if (totalGiftStock < giftProductQuantity)
                {
                    lbCheckGiftQuantity.Text = "Không đủ số lượng quà tặng trong kho!";
                    lbCheckGiftQuantity.ForeColor = Color.Red;
                    return;
                }
            }


            // Đánh dấu áp dụng trên hóa đơn nếu không có sản phẩm cụ thể
            int appliesToOrder = (discountType.Equals("Percentage", StringComparison.OrdinalIgnoreCase) || discountType.Equals("Amount", StringComparison.OrdinalIgnoreCase)) ? 1 : 0;
            
            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn thêm khuyến mãi không?");
            if(result == DialogResult.Yes)
            {
                _promotionController.AddPromotion(txtName.Text, txtDescription.Text, discountType, dtpStartDate.Value, dtpEndDate.Value,
                productID, minQuantity, appliesToOrder, giftProductID, giftProductQuantity, minBillAmount, discountValue);
                ShowScreen.ShowMessage("Thêm khuyến mãi thành công");
                OnLoadPromotion?.Invoke();
                this.Close();
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            lbCheckName.Text = String.Empty;
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            lbCheckEndDate.Text = String.Empty;
        }

        private void txtDiscountValue_TextChanged(object sender, EventArgs e)
        {
            lbCheckDiscountValue.Text = String.Empty;
            FormatNumber.FormatAsNumber(txtDiscountValue);
        }

        private void txtMinQuantity_TextChanged(object sender, EventArgs e)
        {
            lbCheckMinQuantity.Text = String.Empty;
            FormatNumber.FormatAsNumber(txtMinQuantity);
        }

        private void txtGiftProduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProduct.Text))
            {
                lbCheckGiftProduct.Text = String.Empty;
            }
            string keyword = txtGiftProduct.Text.Trim().ToLower();
            // Lọc danh sách sản phẩm theo từ khóa
            var filteredProduct = _products.Where(p => p.ProductID.ToString().Contains(keyword)
                                                        || p.ProductName.ToLower().Contains(keyword)
                                                        || p.CategoryName.ToLower().Contains(keyword)).ToList();
            lstGiftProduct.DataSource = null;
            lstGiftProduct.DataSource = filteredProduct;
            // Nếu có kết quả thì hiển thị ListBox, nếu không thì ẩn
            lstGiftProduct.Visible = filteredProduct.Count > 0;
        }

        private void txtGiftQuantity_TextChanged(object sender, EventArgs e)
        {
            lbCheckGiftQuantity.Text = String.Empty;
            FormatNumber.FormatAsNumber(txtGiftQuantity);
        }

        private void lstProduct_Click(object sender, EventArgs e)
        {
            if (lstProduct.SelectedItem != null)
            {
                ProductDTO selectedProduct = (ProductDTO)lstProduct.SelectedItem;
                // Gán dữ liệu
                txtProduct.Text = selectedProduct.ProductName;
                
                txtProduct.Tag = selectedProduct.ProductID;
                Console.WriteLine("Product: " + txtProduct.Tag);
                
            }
            lstProduct.Visible = false;
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstProduct.SelectedItem != null)
            {
                lstProduct_Click(sender, e); // Gọi lại sự kiện Click để chọn sản phẩm
                e.SuppressKeyPress = true;  // Ngăn tiếng "beep"
            }
        }

        private void lstGiftProduct_Click(object sender, EventArgs e)
        {
            if (lstGiftProduct.SelectedItem != null)
            {
                ProductDTO selectedProduct = (ProductDTO)lstGiftProduct.SelectedItem;
                // Gán dữ liệu
                txtGiftProduct.Text = selectedProduct.ProductName;
                txtGiftProduct.Tag = selectedProduct.ProductID;
            }
            lstGiftProduct.Visible = false;
        }

        private void txtGiftProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lstGiftProduct.SelectedItem != null)
            {
                lstGiftProduct_Click(sender, e); // Gọi lại sự kiện Click để chọn sản phẩm
                e.SuppressKeyPress = true;  // Ngăn tiếng "beep"
            }
        }

        private void txtMinBillAmount_TextChanged(object sender, EventArgs e)
        {
            lbCheckMinBill.Text = String.Empty;
            FormatNumber.FormatAsNumber(txtMinBillAmount);
        }
    }
}
