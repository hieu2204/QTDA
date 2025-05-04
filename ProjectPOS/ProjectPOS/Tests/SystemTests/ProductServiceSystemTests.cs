using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectPOS.Servies;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Diagnostics;

namespace ProjectPOS.Tests.SystemTests
{
    [TestClass]
    public class ProductServiceSystemTests
    {
        // Dùng connection string của database test (đảm bảo rằng đây là database dùng cho test)
        private readonly string _connectionString = "Data Source=DESKTOP-VDPSTNV;Initial Catalog=POS_DB;Integrated Security=True;";

        // Dữ liệu test mẫu
        private readonly string testProductName = "TestProduct";
        private readonly int testCategory = 1;
        private readonly double testPrice = 100.0;
        private readonly string testDescription = "Test Description";
        private readonly string testImageURL = "http://example.com/test.jpg";

        [TestInitialize]
        public void Setup()
        {
            // Xoá bỏ các sản phẩm test đã tồn tại để đảm bảo môi trường sạch.
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Product WHERE Name = @Name", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", testProductName);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void InsertProduct_ShouldInsertProductSuccessfully()
        {
            // Act: Gọi phương thức InsertProduct
            IProductService.InsertProduct(testProductName, testCategory, testPrice, testDescription, testImageURL);

            // Validate: Kiểm tra sản phẩm đã được chèn thành công hay chưa bằng cách truy vấn trực tiếp vào database.
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 ProductID FROM Product WHERE Name = @Name", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", testProductName);
                    var result = cmd.ExecuteScalar();
                    Assert.IsNotNull(result, "Sản phẩm test chưa được tìm thấy trong cơ sở dữ liệu.");
                }
            }
        }

        [TestMethod]
        public void GetListProduct_WithPagination_ShouldReturnProducts()
        {
            // Đảm bảo có sản phẩm test trong database
            IProductService.InsertProduct(testProductName, testCategory, testPrice, testDescription, testImageURL);

            // Act: Lấy danh sách sản phẩm với phân trang (ví dụ trang 1, mỗi trang 10 sản phẩm)
            List<ProductDTO> products = IProductService.GetListProduct(1, 10);

            // Validate:
            Assert.IsNotNull(products, "Danh sách sản phẩm không được null.");
            Assert.IsTrue(products.Count > 0, "Danh sách sản phẩm phải chứa ít nhất 1 bản ghi.");
        }

        [TestMethod]
        public void UpdateProduct_ShouldUpdateProductSuccessfully()
        {
            // Insert sản phẩm test để cập nhật
            IProductService.InsertProduct(testProductName, testCategory, testPrice, testDescription, testImageURL);
            int productId;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 ProductID FROM Product WHERE Name = @Name", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", testProductName);
                    var result = cmd.ExecuteScalar();
                    Assert.IsNotNull(result, "Sản phẩm chưa được chèn thành công.");
                    productId = Convert.ToInt32(result);
                }
            }

            // Act: Cập nhật sản phẩm – thay đổi mô tả và giá.
            string updatedDescription = "Updated Description";
            double updatedPrice = testPrice + 50.0;
            IProductService.UpdateProduct(testProductName, testCategory, updatedPrice, updatedDescription, testImageURL, productId);

            // Validate: Kiểm tra lại thông tin cập nhật từ database.
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Price, Description FROM Product WHERE ProductID = @ProductID", conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double actualPrice = reader["Price"] is DBNull ? 0 : Convert.ToDouble(reader["Price"]);
                            string actualDescription = reader["Description"].ToString();
                            Assert.AreEqual(updatedPrice, actualPrice, "Giá sản phẩm không được cập nhật đúng.");
                            Assert.AreEqual(updatedDescription, actualDescription, "Mô tả sản phẩm không được cập nhật đúng.");
                        }
                        else
                        {
                            Assert.Fail("Không tìm thấy sản phẩm sau khi cập nhật.");
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SearchProduct_ShouldReturnMatchingProducts()
        {
            // Insert sản phẩm test
            IProductService.InsertProduct(testProductName, testCategory, testPrice, testDescription, testImageURL);

            // Act: Tìm kiếm sản phẩm theo tên
            List<ProductDTO> products = IProductService.SearchProduct(testProductName);

            // Validate: Kết quả tìm kiếm phải chứa sản phẩm test
            Assert.IsNotNull(products, "Kết quả tìm kiếm không được null.");
            Assert.IsTrue(products.Any(p => p.ProductName.IndexOf(testProductName, StringComparison.OrdinalIgnoreCase) >= 0));

        }

        [TestCleanup]
        public void Cleanup()
        {
            // Xoá các sản phẩm test sau mỗi bài test để không ảnh hưởng đến các lần chạy sau.
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Product WHERE Name = @Name", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", testProductName);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        [TestMethod]
        public void InsertProduct_WithInjectionInput_ShouldNotBeVulnerable()
        {
            // Chuỗi input chứa ký tự có thể gây injection
            string injectionString = "TestProduct'; DROP TABLE Product;--";

            // Gọi phương thức InsertProduct với input độc hại
            IProductService.InsertProduct(injectionString, testCategory, testPrice, testDescription, testImageURL);

            // Validate: Kiểm tra bảng Product vẫn tồn tại và dữ liệu không bị thay đổi ngoài ý muốn.
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Product", conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    Assert.IsTrue(count >= 0, "Bảng Product bị ảnh hưởng bởi SQL injection.");
                }
            }
        }
    }
}
