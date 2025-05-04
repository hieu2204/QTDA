using ProjectPOS.Controllers;
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
using ProjectPOS.Servies;

namespace ProjectPOS.Views.Forms
{
    public partial class AddUser : Form
    {
        private string relativeURL = String.Empty;
        private string absolutePath = String.Empty;
        private UserController _userController = new UserController();
        public event Action _OnLoadUser;
        public AddUser()
        {
            InitializeComponent();
        }

        private void ptbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            // Mở hộp thoại chọn file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                absolutePath = openFileDialog.FileName;
                ptbAvatar.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            IUserServiceWrapper userServiceWrapper = new UserServiceWrapper(); // Hoặc inject từ Dependency Injection
            Validation validation = new Validation(userServiceWrapper);
            // Tạo danh sách trạng thái
            bool IsVali = validation.ValidationAddUser(txtUser.Text, txtPass.Text,txtName.Text, txtEmail.Text, txtPhone.Text, txtAddress.Text, txtSalary.Text,
                lbCheckUser, lbCheckPass, lbCheckName, lbCheckPhone, lbCheckSalary, lbCheckEmail, lbCheckAddress);
            if (!IsVali)
            {
                return;
            }
            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn thêm nhân viên?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                //Console.WriteLine("Cbogender: " + cboGender.SelectedItem.ToString());
                //Console.WriteLine("CboRole: " + cboRole.SelectedItem.ToString());
                _userController.InsertUser(txtUser.Text, HashPass.HashPassword(txtPass.Text), txtName.Text, txtEmail.Text, txtPhone.Text, cboGender.SelectedItem.ToString(),
                    dtpBirthDate.Value, txtAddress.Text, absolutePath, double.Parse(txtSalary.Text, NumberStyles.AllowThousands, CultureInfo.GetCultureInfo("en-US")), cboRole.SelectedItem.ToString());
                ShowScreen.ShowMessage("Thêm thông tin nhân viên thành công", "Thông báo");
                txtUser.Text = txtPass.Text = txtName.Text = txtPhone.Text = txtEmail.Text = txtAddress.Text = txtSalary.Text = String.Empty;
                ptbAvatar.Image = null;
                _OnLoadUser?.Invoke();
            }
            else
            {
                Console.WriteLine("Cancel add employee");
            }
        }

        private void txtPass_IconRightClick(object sender, EventArgs e)
        {
            if(txtPass.PasswordChar == '\0')
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

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {
            FormatNumber.FormatAsNumber(txtSalary);
        }

    }
}
