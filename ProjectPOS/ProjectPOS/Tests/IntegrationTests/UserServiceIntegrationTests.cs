using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectPOS.Servies;
using ProjectPOS.Models;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ProjectPOS.Tests.IntegrationTests
{
    [TestClass]
    public class UserServiceIntegrationTests
    {
        // Sử dụng connection string của database test (đảm bảo rằng đây là database dùng cho test)
        private readonly string _connectionString = "Data Source=DESKTOP-VDPSTNV;Initial Catalog=POS_DB;Integrated Security=True;";

        // Dữ liệu test mẫu
        private readonly string testUsername = "testuser";
        private readonly string testPassword = "testpass";
        private readonly string testName = "Test User";
        private readonly string testEmail = "testuser@example.com";
        private readonly string testPhone = "1234567890";
        private readonly string testGender = "M";
        private readonly DateTime testBirth = new DateTime(1990, 1, 1);
        private readonly string testAddress = "Test Address";
        private readonly string testImage = "http://example.com/image.jpg";
        private readonly double testSalary = 1000.0;
        private readonly string testRole = "User";

        [TestInitialize]
        public void Setup()
        {
            // Xoá dữ liệu test nếu đã tồn tại
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE Username = @Username", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", testUsername);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void InsertUser_ShouldInsertUserSuccessfully()
        {
            // Chèn người dùng test bằng phương thức InsertUser
            IUserService.InsertUser(testUsername, testPassword, testName, testEmail, testPhone, testGender, testBirth, testAddress, testImage, testSalary, testRole);

            // Kiểm tra xem người dùng có tồn tại hay không bằng phương thức IsUsernameExit
            bool exists = IUserService.IsUsernameExit(testUsername);
            Assert.IsTrue(exists, "Người dùng test phải tồn tại sau khi chèn.");
        }

        [TestMethod]
        public void GetTotalPage_ShouldReturnNonNegativeValue()
        {
            // Giả sử mỗi trang chứa 10 người dùng
            int pageSize = 10;
            int totalPage = IUserService.GetTotalPage(pageSize);
            Assert.IsTrue(totalPage >= 0, "Tổng số trang không được âm.");
        }

        [TestMethod]
        public void GetListUser_ShouldReturnListContainingTestUser()
        {
            // Đảm bảo có người dùng test trong database
            IUserService.InsertUser(testUsername, testPassword, testName, testEmail, testPhone, testGender, testBirth, testAddress, testImage, testSalary, testRole);

            // Lấy danh sách người dùng có phân trang (ví dụ trang 1, mỗi trang 10 bản ghi)
            List<UserModel> users = IUserService.GetListUser(1, 10);
            Assert.IsNotNull(users, "Danh sách người dùng không được null.");
            Assert.IsTrue(users.Count > 0, "Danh sách người dùng không được rỗng.");

            // Kiểm tra xem người dùng test có tồn tại trong danh sách hay không
            bool testUserExists = users.Any(u => u.UserName.Equals(testUsername, StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(testUserExists, "Danh sách người dùng phải chứa người dùng test.");
        }

        [TestMethod]
        public void SearchUsers_ShouldReturnMatchingTestUser()
        {
            // Chèn người dùng test
            IUserService.InsertUser(testUsername, testPassword, testName, testEmail, testPhone, testGender, testBirth, testAddress, testImage, testSalary, testRole);

            // Tìm kiếm người dùng theo tên
            List<UserModel> users = IUserService.SearchUsers(testName);
            Assert.IsNotNull(users, "Danh sách kết quả tìm kiếm không được null.");
            Assert.IsTrue(users.Any(u => u.UserName.Equals(testUsername, StringComparison.OrdinalIgnoreCase)), "Kết quả tìm kiếm phải chứa người dùng test.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Sau mỗi test, xoá người dùng test khỏi database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE Username = @Username", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", testUsername);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
