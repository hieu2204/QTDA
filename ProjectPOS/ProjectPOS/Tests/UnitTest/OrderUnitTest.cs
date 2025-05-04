using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectPOS.Views.Forms;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ProjectPOS.Controllers;

namespace ProjectPOS.Tests.UnitTest
{
    [TestClass]
    public class OrderUnitTest
    {
        // FakeProductController override phương thức GetToTalQuantity để luôn trả về 5
        public class FakeProductController : ProductController
        {
            public override int GetToTalQuantity(int productId)
            {
                return 5;
            }
        }
        [TestMethod]
        public void ValidateProductInput_InvalidQuantity_ReturnsFalse()
        {
            AddSell form = new AddSell();
            PrivateObject po = new PrivateObject(form);

            // Ép kiểu đúng với các control của Guna
            Guna2TextBox txtProduct = (Guna2TextBox)po.GetField("txtProduct");
            Guna2TextBox txtQuantity = (Guna2TextBox)po.GetField("txtQuantity");
            Guna2HtmlLabel lbCheckQuantity = (Guna2HtmlLabel)po.GetField("lbCheckQuantity");

            txtProduct.Tag = 1;
            txtQuantity.Text = "-1";

            bool result = (bool)po.Invoke("ValidateProductInput");

            Assert.IsFalse(result);
            Assert.AreEqual("Số lượng không hợp lệ!", lbCheckQuantity.Text);
        }

        [TestMethod]
        public void ValidateProductInput_QuantityExceedsStock_ReturnsFalse()
        {
            // Tạo instance của form
            AddSell form = new AddSell();
            PrivateObject po = new PrivateObject(form);

            // Thay thế đối tượng _productController bằng FakeProductController
            FakeProductController fakeController = new FakeProductController();
            po.SetField("_productController", fakeController);

            // Lấy các control với kiểu đúng (Guna2TextBox và Guna2HtmlLabel)
            Guna2TextBox txtProduct = (Guna2TextBox)po.GetField("txtProduct");
            Guna2TextBox txtQuantity = (Guna2TextBox)po.GetField("txtQuantity");
            Guna2HtmlLabel lbCheckQuantity = (Guna2HtmlLabel)po.GetField("lbCheckQuantity");

            // Thiết lập test:
            // - Đã chọn sản phẩm (gán Tag = 1)
            // - Nhập số lượng là "10" (vượt quá số lượng tồn kho 5)
            txtProduct.Tag = 1;
            txtQuantity.Text = "10";

            // Gọi phương thức ValidateProductInput thông qua PrivateObject
            bool result = (bool)po.Invoke("ValidateProductInput");

            // Kiểm tra kết quả
            Assert.IsFalse(result);
            Assert.AreEqual("Số lượng tồn kho không đủ! Chỉ còn 5 sản phẩm.", lbCheckQuantity.Text);
        }
        [TestMethod]
        public void ValidateProductInput_QuantityEqualsStock_ReturnsTrue()
        {
            // Tạo instance của form
            AddSell form = new AddSell();
            PrivateObject po = new PrivateObject(form);

            // Thay thế đối tượng _productController bằng FakeProductController
            FakeProductController fakeController = new FakeProductController();
            po.SetField("_productController", fakeController);

            // Lấy các control với kiểu đúng (Guna2TextBox và Guna2HtmlLabel)
            Guna2TextBox txtProduct = (Guna2TextBox)po.GetField("txtProduct");
            Guna2TextBox txtQuantity = (Guna2TextBox)po.GetField("txtQuantity");
            Guna2HtmlLabel lbCheckProduct = (Guna2HtmlLabel)po.GetField("lbCheckProduct");
            Guna2HtmlLabel lbCheckQuantity = (Guna2HtmlLabel)po.GetField("lbCheckQuantity");

            // Thiết lập test:
            // - Đã chọn sản phẩm (gán Tag = 1)
            // - Nhập số lượng là "5" (bằng số lượng tồn kho 5)
            txtProduct.Tag = 1;
            txtQuantity.Text = "5";

            // Gọi phương thức ValidateProductInput thông qua PrivateObject
            bool result = (bool)po.Invoke("ValidateProductInput");

            // Kiểm tra kết quả: phương thức trả về true và các thông báo lỗi được xóa (rỗng)
            Assert.IsTrue(result);
            Assert.AreEqual(string.Empty, lbCheckProduct.Text);
            Assert.AreEqual(string.Empty, lbCheckQuantity.Text);
        }
        [TestMethod]
        public void ValidateProductInput_NonNumericQuantity_ReturnsFalse()
        {
            // Tạo instance của form
            AddSell form = new AddSell();
            PrivateObject po = new PrivateObject(form);

            // Thay thế đối tượng _productController bằng FakeProductController
            FakeProductController fakeController = new FakeProductController();
            po.SetField("_productController", fakeController);

            // Lấy các control với kiểu đúng (Guna2TextBox và Guna2HtmlLabel)
            Guna2TextBox txtProduct = (Guna2TextBox)po.GetField("txtProduct");
            Guna2TextBox txtQuantity = (Guna2TextBox)po.GetField("txtQuantity");
            Guna2HtmlLabel lbCheckQuantity = (Guna2HtmlLabel)po.GetField("lbCheckQuantity");

            // Thiết lập test:
            // - Đã chọn sản phẩm (gán Tag = 1)
            // - Nhập số lượng không phải số: "abc"
            txtProduct.Tag = 1;
            txtQuantity.Text = "abc";

            // Gọi phương thức ValidateProductInput thông qua PrivateObject
            bool result = (bool)po.Invoke("ValidateProductInput");

            // Kiểm tra kết quả: phương thức trả về false và hiển thị thông báo lỗi "Số lượng không hợp lệ!"
            Assert.IsFalse(result);
            Assert.AreEqual("Số lượng không hợp lệ!", lbCheckQuantity.Text);
        }
    }
}
