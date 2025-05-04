using ProjectPOS.Controllers;
using ProjectPOS.Models;
using ProjectPOS.Servies;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace ProjectPOS.Views.Forms
{
    public partial class Login : Form
    {
        private string hashpass = string.Empty;
        private UserController _user = new UserController();
        private int failedLoginAttempts = 0; // Đếm số lần nhập sai
        private DateTime lockEndTime;
        private Timer lockTimer;
        private Dictionary<string, Tuple<string, DateTime>> otpStorage = new Dictionary<string, Tuple<string, DateTime>>();

        public Login()
        {
            InitializeComponent();
            pnForgot.Visible = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsAccountLocked())
            {
                MessageBox.Show($"Tài khoản bị khóa đến {lockEndTime:HH:mm:ss}. Vui lòng thử lại sau.");
                return;
            }

            if (string.IsNullOrEmpty(txtUser.Text))
            {
                lbCheckUser.Text = "Vui lòng nhập tài khoản.";
                lbCheckUser.ForeColor = Color.Red;
                return;
            }
            if (txtUser.Text.Length > 50)
            {
                lbCheckUser.Text = "Tên tài khoản không được vượt quá 50 ký tự.";
                lbCheckUser.ForeColor = Color.Red;
                return;
            }
            lbCheckUser.Text = string.Empty;

            if (string.IsNullOrEmpty(txtPass.Text))
            {
                lbCheckPass.Text = "Vui lòng nhập mật khẩu.";
                lbCheckPass.ForeColor = Color.Red;
                return;
            }
            lbCheckPass.Text = string.Empty;

            string username = txtUser.Text.Trim().ToLower();
            string passwordHash = HashPass.HashPassword(txtPass.Text).ToLower();
            List<UserModel> users = _user.GetAllUser();

            var user = users.FirstOrDefault(u => u.UserName.ToLower() == username);

            if (user == null)
            {
                lbCheckUser.Text = "Tài khoản không tồn tại.";
                lbCheckUser.ForeColor = Color.Red;
                return;
            }

            if (user.PasswordHash.ToLower() != passwordHash)
            {
                failedLoginAttempts++;
                lbCheckPass.Text = "Mật khẩu không đúng.";
                lbCheckPass.ForeColor = Color.Red;

                if (failedLoginAttempts >= 3)
                {
                    LockAccount();
                }
                return;
            }

            // Nếu đến đây thì đăng nhập thành công
            failedLoginAttempts = 0;
            Function function = new Function();
            function.init(user);
            function.CheckRole(user);
            function.ShowDialog();
            this.Close();
        }

        private bool IsAccountLocked()
        {
            return DateTime.Now < lockEndTime;
        }

        private void LockAccount()
        {
            lockEndTime = DateTime.Now.AddMinutes(1); // Khóa trong 5 phút
            btnLogin.Enabled = false;
            failedLoginAttempts = 0;

            lockTimer = new Timer();
            lockTimer.Interval = 1000; // 1 giây
            lockTimer.Tick += LockTimer_Tick;
            lockTimer.Start();
        }

        private void LockTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = lockEndTime - DateTime.Now;
            if (remainingTime.TotalSeconds <= 0)
            {
                lockTimer.Stop();
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
            }
            else
            {
                btnLogin.Text = $"Đợi {remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
            }
        }
        private void txtPass_IconRightClick(object sender, EventArgs e)
        {
            if (txtPass.PasswordChar == '\0')
            {
                txtPass.PasswordChar = '*';
                txtPass.IconRight = Properties.Resources.eye;
            }
            else
            {
                txtPass.PasswordChar = '\0';
                txtPass.IconRight = Properties.Resources.view;
            }
        }

        private void btnForgotPass_Click(object sender, EventArgs e)
        {
            pnForgot.Visible = true;
            
        }
        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // OTP 6 số
        }

        private bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("hieuhoangg2204@gmail.com", "lulggrjcumjjkcfp");
                    smtp.EnableSsl = true;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("your-email@gmail.com");
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = false;

                    smtp.Send(mail);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi email: " + ex.Message);
                return false;
            }
        }

        private void btnAuthOTP_Click(object sender, EventArgs e)
        {
            string email = txtForgotEmail.Text.Trim();
            string enteredOtp = txtOTP.Text.Trim();

            if (!otpStorage.ContainsKey(email))
            {
                MessageBox.Show("OTP không tồn tại hoặc đã hết hạn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var otpData = otpStorage[email];

            if (DateTime.Now > otpData.Item2) // Kiểm tra thời gian hết hạn
            {
                otpStorage.Remove(email);
                MessageBox.Show("OTP đã hết hạn. Vui lòng yêu cầu mã mới.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (otpData.Item1 != enteredOtp) // Kiểm tra mã OTP
            {
                MessageBox.Show("OTP không đúng. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác thực OTP thành công, cho phép nhập mật khẩu mới
            txtNewPassword.Enabled = true;
            btnUpdateNewPass.Enabled = true;
        }

        private void btnUpdateNewPass_Click(object sender, EventArgs e)
        {
            string email = txtForgotEmail.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mã hóa mật khẩu mới
            string hashedPass = HashPass.HashPassword(newPassword);

            // Cập nhật mật khẩu vào database
            UpdatePasswordByEmail(email, hashedPass);
                MessageBox.Show("Mật khẩu đã được cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                otpStorage.Remove(email); // Xóa OTP khỏi bộ nhớ
        }
        public static void UpdatePasswordByEmail(string email, string newPassword)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdatePasswordByEmail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ptbExit_Click(object sender, EventArgs e)
        {
            pnForgot.Visible = false;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string email = txtForgotEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email đã đăng ký!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra email trong danh sách users đã tải trước đó
            List<UserModel> users = _user.GetAllUser();
            UserModel user = users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                MessageBox.Show("Email này không tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tạo mã OTP ngẫu nhiên
            string otpCode = GenerateOTP();
            DateTime expiryTime = DateTime.Now.AddMinutes(5);

            // Lưu OTP vào bộ nhớ tạm thời
            otpStorage[email] = Tuple.Create(otpCode, expiryTime);

            // Gửi email OTP
            bool isSent = SendEmail(email, "Mã OTP đặt lại mật khẩu", $"Mã OTP của bạn là: {otpCode}. Hết hạn sau 5 phút.");
            if (isSent)
            {
                MessageBox.Show("Mã OTP đã được gửi tới email của bạn.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Gửi email thất bại. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtNewPassword_IconRightClick(object sender, EventArgs e)
        {
            if (txtNewPassword.PasswordChar == '\0')
            {
                txtNewPassword.PasswordChar = '*';
                txtNewPassword.IconRight = Properties.Resources.eye;
            }
            else
            {
                txtNewPassword.PasswordChar = '\0';
                txtNewPassword.IconRight = Properties.Resources.view;
            }
        }
    }
}
