using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectPOS.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guna.UI2.WinForms;
using Moq;
using System.Text.RegularExpressions;

namespace ProjectPOS.Tests.UnitTest
{
    [TestClass]
    public class IconUnitTest
    {
        [TestMethod]
        public void TxtPass_IconRightClick_TogglesPasswordChar()
        {
            // Arrange
            Login form = new Login();
            PrivateObject po = new PrivateObject(form);

            Guna2TextBox txtPass = (Guna2TextBox)po.GetField("txtPass");

            // Giả sử ban đầu, mật khẩu được che (PasswordChar = '*')
            txtPass.PasswordChar = '*';
            // Giả sử IconRight ban đầu là Resources.eye, sau khi click nó chuyển thành Resources.view

            // Act: Gọi sự kiện IconRightClick
            po.Invoke("txtPass_IconRightClick", new object[] { txtPass, EventArgs.Empty });

            // Assert: Kiểm tra rằng PasswordChar đã chuyển thành '\0'
            Assert.AreEqual('\0', txtPass.PasswordChar);

            // Act: Gọi lại sự kiện IconRightClick để chuyển lại
            po.Invoke("txtPass_IconRightClick", new object[] { txtPass, EventArgs.Empty });

            // Assert: PasswordChar trở lại '*'
            Assert.AreEqual('*', txtPass.PasswordChar);
        }

    }
}
