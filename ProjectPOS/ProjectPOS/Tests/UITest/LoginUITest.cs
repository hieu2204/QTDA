using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.Custom;

namespace ProjectPOS.Tests.UITests
{
    [TestClass]
    public class LoginUITest
    {
        private Application _app; //  Lưu trữ ứng dụng ProjectPOS.exe.
        private Window _window; // Lưu trữ cửa sổ Login
        // [TestInitialize] → Chạy trước mỗi test case.
        [TestInitialize]
        public void Setup()
        {
            _app = Application.Launch(@"D:\\.NET\\ProjectPOS\\ProjectPOS\\bin\\Debug\\ProjectPOS.exe"); //  Chạy ProjectPOS.exe.
            Thread.Sleep(3000); // Đợi 3 giây để đảm bảo UI tải xong.
            _window = _app.GetWindow("Login"); // Lấy cửa sổ Login.
            _window.WaitWhileBusy(); // Chờ cửa sổ không còn bận (loading).
        }
        // Xác định đây là một test case.
        [TestMethod]
        public void Test_ValidLogin()
        {
            Assert.IsNotNull(_window, "Không tìm thấy cửa sổ Login."); // Nếu cửa sổ không mở, test fail ngay lập tức
            // Tìm txtUserPanel, txtPassPanel, btnLoginPanel bằng AutomationId. 
            // Lấy Panel chứa Guna UI TextBox
            var txtUserPanel = _window.Get<Panel>(SearchCriteria.ByAutomationId("txtUser"));
            var txtPassPanel = _window.Get<Panel>(SearchCriteria.ByAutomationId("txtPass"));
            var btnLoginPanel = _window.Get<Panel>(SearchCriteria.ByAutomationId("btnLogin"));
            btnLoginPanel.Click();

            // Nếu một trong ba Panel không tồn tại, test sẽ fail.
            Assert.IsNotNull(txtUserPanel, "Không tìm thấy Panel txtUser.");
            Assert.IsNotNull(txtPassPanel, "Không tìm thấy Panel txtPass.");
            Assert.IsNotNull(btnLoginPanel, "Không tìm thấy Button btnLogin.");

            // Lấy WinFormTextBox bên trong Panel
            var txtUser = txtUserPanel.Get<WinFormTextBox>(SearchCriteria.All);
            var txtPass = txtPassPanel.Get<WinFormTextBox>(SearchCriteria.All);

            // txtUserPanel.Get<WinFormTextBox>(SearchCriteria.All) để lấy TextBox bên trong Panel.
            // Kiểm tra có lấy được hay không.
            Assert.IsNotNull(txtUser, "Không tìm thấy WinFormTextBox bên trong txtUser.");
            Assert.IsNotNull(txtPass, "Không tìm thấy WinFormTextBox bên trong txtPass.");

            // Nhập thông tin đăng nhập
            txtUser.Enter("shadow");
            txtPass.Enter("12345");
            btnLoginPanel.Click();

            // Chờ form Function mở
            /*
             * Đợi 2 giây để Form Function mở.
             * Tìm cửa sổ có tiêu đề chứa "Function".
             * Nếu không tìm thấy, báo lỗi "Đăng nhập thất bại, không mở được Function.".
             */
            Thread.Sleep(2000);
            // Kiểm tra form Function đã mở chưa
            var functionWindow = _app.GetWindows().FirstOrDefault(w => w.Title.Contains("Function"));
            Assert.IsNotNull(functionWindow, "Đăng nhập thất bại, không mở được Function.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _app?.Close(); // → Đóng ProjectPOS.exe.
        }
        private void DoLogin(string username, string password)
        {
            var txtUserPanel = _window.Get<Panel>(SearchCriteria.ByAutomationId("txtUser"));
            var txtPassPanel = _window.Get<Panel>(SearchCriteria.ByAutomationId("txtPass"));
            var btnLoginPanel = _window.Get<Panel>(SearchCriteria.ByAutomationId("btnLogin"));

            var txtUser = txtUserPanel.Get<WinFormTextBox>(SearchCriteria.All);
            var txtPass = txtPassPanel.Get<WinFormTextBox>(SearchCriteria.All);

            txtUser.Text = username;
            txtPass.Text = password;
            btnLoginPanel.Click();
            Thread.Sleep(2000); // Chờ UI xử lý
        }
        [TestMethod]

        public void Test_WrongPassword()
        {
            DoLogin("shadow", "sai_password");

            // Kiểm tra vẫn ở form Login (chưa mở Function)
            var functionWindow = _app.GetWindows().FirstOrDefault(w => w.Title.Contains("Function"));
            Assert.IsNull(functionWindow, "Form Function không nên mở vì mật khẩu sai.");

            var lblUser = _window.Get<UIItem>(SearchCriteria.ByAutomationId("lbCheckPass"));
            Assert.IsTrue(lblUser.Name.Contains("Mật khẩu không đúng"));
        }
        [TestMethod]
        public void Test_UsernameNotExist()
        {
            DoLogin("khong_ton_tai", "12345");

            var functionWindow = _app.GetWindows().FirstOrDefault(w => w.Title.Contains("Function"));
            Assert.IsNull(functionWindow, "Form Function không nên mở vì tài khoản không tồn tại.");

            var lblUser = _window.Get<UIItem>(SearchCriteria.ByAutomationId("lbCheckUser"));
            Assert.IsTrue(lblUser.Name.Contains("Tài khoản không tồn tại"));
        }
        [TestMethod]
        public void Test_EmptyUsername()
        {
            DoLogin("", "12345");

            var lblUser = _window.Get<UIItem>(SearchCriteria.ByAutomationId("lbCheckUser"));
            Assert.IsTrue(lblUser.Name.Contains("Vui lòng nhập tài khoản"));
        }
        [TestMethod]
        public void Test_EmptyPassword()
        {
            DoLogin("shadow", "");

            var lblPass = _window.Get<UIItem>(SearchCriteria.ByAutomationId("lbCheckPass"));
            Assert.IsTrue(lblPass.Name.Contains("Vui lòng nhập mật khẩu"));
        }
        [TestMethod]
        public void Test_AccountLockedAfter3Attempts()
        {
            for (int i = 0; i < 3; i++)
            {
                DoLogin("shadow", "sai_password");
                Thread.Sleep(1000); // Chờ UI cập nhật
            }

            // Kiểm tra nội dung text của btnLogin sau khi bị khóa
            var btnLogin = _window.Get<UIItem>(SearchCriteria.ByAutomationId("btnLogin"));
            Assert.IsNotNull(btnLogin, "Không tìm thấy button đăng nhập.");

            // Kiểm tra nội dung button có chứa thông báo về tài khoản bị khóa
            Assert.IsTrue(btnLogin.Name.Contains("Tài khoản bị khóa") || btnLogin.Name.Contains("Đợi"),
                "Nội dung button không hiển thị thông báo khóa tài khoản.");
        }
        [TestMethod]
        public void Test_LoginAfterAccountUnlock()
        {
            for (int i = 0; i < 3; i++)
            {
                DoLogin("shadow", "sai_password");
                Thread.Sleep(1000); // Chờ UI cập nhật
            }

            // Kiểm tra nội dung text của btnLogin sau khi bị khóa
            var btnLogin = _window.Get<UIItem>(SearchCriteria.ByAutomationId("btnLogin"));
            Assert.IsNotNull(btnLogin, "Không tìm thấy button đăng nhập.");

            // Kiểm tra nội dung button có chứa thông báo về tài khoản bị khóa
            Assert.IsTrue(btnLogin.Name.Contains("Tài khoản bị khóa") || btnLogin.Name.Contains("Đợi"),
                "Nội dung button không hiển thị thông báo khóa tài khoản.");
            // Bước 3: Đợi 1 phút (hoặc mở khóa thủ công)
            Thread.Sleep(60000); // Đợi 1 phút

            // Bước 4: Đăng nhập với mật khẩu đúng
            DoLogin("shadow", "12345");

            // Bước 5: Kiểm tra màn hình chức năng (Function)
            var functionWindow = _app.GetWindows().FirstOrDefault(w => w.Title.Contains("Function"));
            Assert.IsNotNull(functionWindow, "Màn hình chức năng không được mở sau khi đăng nhập lại.");
        }

    }
}
