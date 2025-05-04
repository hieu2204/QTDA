using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectPOS.Models;
using ProjectPOS.Views.Forms;
using Guna.UI2.WinForms;
using System.Windows.Forms;


namespace ProjectPOS.Tests.UnitTest
{
    [TestClass]
    public class ProductUnitTest
    {
        [TestMethod]
        public void TxtCategory_TextChanged_WithMatchingKeyword_FiltersCategories()
        {
            // Arrange: Tạo instance của form AddProduct
            AddProduct form = new AddProduct();
            PrivateObject po = new PrivateObject(form);

            // Lấy các control từ form (giả sử txtCategory và listCategory là kiểu TextBox và ListBox)
            //TextBox txtCategory = (TextBox)po.GetField("txtCategory");
            Guna2TextBox txtCategory = (Guna2TextBox)po.GetField("txtCategory");
            ListBox listCategory = (ListBox)po.GetField("listCategory");


            // Tạo danh sách danh mục test
            List<CategoryModel> testCategories = new List<CategoryModel>
            {
                new CategoryModel { id = 1, name = "Electronics" },
                new CategoryModel { id = 2, name = "Books" },
                new CategoryModel { id = 3, name = "Clothes" }
            };
            // Gán danh sách test cho biến private 'categories' trong form
            po.SetField("categories", testCategories);

            // Act: Giả lập nhập từ khóa "book" (không phân biệt chữ hoa chữ thường)
            txtCategory.Text = "book";
            // Gọi thủ công event handler (tham số sender và EventArgs có thể truyền đơn giản)
            po.Invoke("txtCategory_TextChanged", new object[] { txtCategory, EventArgs.Empty });

            // Assert:
            // Kiểm tra rằng ListBox được hiển thị và DataSource chỉ có 1 phần tử (Books)
            var filtered = (List<CategoryModel>)listCategory.DataSource;
         
            Assert.AreEqual(1, filtered.Count, "Danh sách lọc phải có 1 danh mục.");
            Assert.AreEqual("Books", filtered[0].name, "Danh mục lọc phải có tên 'Books'.");
        }
        [TestMethod]
        public void ListCategory_Click_SetsTxtCategoryAndSelectedID()
        {
            // Arrange: Tạo instance của form AddProduct
            AddProduct form = new AddProduct();
            PrivateObject po = new PrivateObject(form);

            // Lấy các control cần thiết
            Guna2TextBox txtCategory = (Guna2TextBox)po.GetField("txtCategory");
            ListBox listCategory = (ListBox)po.GetField("listCategory");

            // Tạo danh sách danh mục test
            List<CategoryModel> testCategories = new List<CategoryModel>
            {
                new CategoryModel { id = 1, name = "Electronics" },
                new CategoryModel { id = 2, name = "Books" }
            };

            // Gán DataSource cho ListBox và cập nhật giá trị hiển thị
            listCategory.DataSource = testCategories;
            listCategory.DisplayMember = "name";
            listCategory.ValueMember = "id";

            // Giả lập người dùng chọn danh mục "Books"
            listCategory.SelectedItem = testCategories[1];

            // Act: Gọi event handler listCategory_Click
            po.Invoke("listCategory_Click", new object[] { listCategory, EventArgs.Empty });

            // Assert: txtCategory.Text nên bằng "Books" và biến selectedID được cập nhật thành 2
            Assert.AreEqual("Books", txtCategory.Text, "TextBox txtCategory cần được cập nhật với tên danh mục đã chọn.");
            int selectedID = (int)po.GetField("selectedID");
            Assert.AreEqual(2, selectedID, "selectedID cần bằng ID của danh mục đã chọn.");
        }
    }
}
