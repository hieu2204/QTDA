using ProjectPOS.Controllers;
using ProjectPOS.Models;
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
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjectPOS.Views.Forms
{
    public partial class UpdateUser : Form
    {
        private UserModel _user = new UserModel();
        //private string relativeURL = "";
        private string absolutePath = "";
        private UserController _userController = new UserController();
        public event Action _OnLoadUser;
        public UpdateUser()
        {
            InitializeComponent();
            Init(_user);
        }
        public void Init(UserModel user)
        {
            List<KeyValuePair<string, int>> statusList = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("Đang làm", 1),
                new KeyValuePair<string, int>("Nghỉ việc", 0)
            };
            if (user != null)
            {
                _user = user;
                txtID.Text = user.Id.ToString();
                txtUser.Text = user.UserName;
                txtPass.Text = user.PasswordHash;
                txtEmail.Text = user.Email;
                txtPhone.Text = user.Phone;
                cboGender.SelectedItem = user.Gender;
                cboStatus.DataSource = statusList;
                cboStatus.DisplayMember = "Key";
                cboStatus.ValueMember = "Value";
                cboStatus.SelectedValue = user.Status;
                cboRole.SelectedItem = user.Role;
                txtSalary.Text = user.Salary.ToString();
                //dtpBirthDate.Value = user.BirthDate.Value;
                if (user.BirthDate == null)
                {
                    dtpBirthDate.CustomFormat = " ";  // Ẩn ngày
                    dtpBirthDate.Format = DateTimePickerFormat.Custom;
                }
                else
                {
                    dtpBirthDate.Format = DateTimePickerFormat.Short;
                    dtpBirthDate.Value = user.BirthDate.Value;
                }
                txtAddress.Text = user.Address;
                //dtpCreateAt.Value = user.CreateAt;
                //dtpUpdateAt.Value = user.UpdateAt;
                // Xử lý CreateAt
                if (user.CreateAt == DateTime.MinValue)
                {
                    dtpCreateAt.Value = DateTime.Now; // Giá trị mặc định hợp lệ
                }
                else
                {
                    dtpCreateAt.Value = user.CreateAt;
                }

                // Xử lý UpdateAt
                if (user.UpdateAt == DateTime.MinValue)
                {
                    dtpUpdateAt.Value = DateTime.Now; // Giá trị mặc định hợp lệ
                }
                else
                {
                    dtpUpdateAt.Value = user.UpdateAt;
                }

                txtName.Text = user.Name;
                if (!string.IsNullOrEmpty(user.ImageURL))
                {
                    absolutePath = Path.Combine(Application.StartupPath, user.ImageURL);
                    // Kiểm tra file có tồn tại không trước khi load
                    if (File.Exists(absolutePath))
                    {
                        ptbAvatar.Image = Image.FromFile(absolutePath);
                    }
                    else
                    {
                        MessageBox.Show("Ảnh không tồn tại: " + absolutePath);
                    }
                }
            }
        }

        private void ptbCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsVali = Validation.ValidationUpdateUser(
                int.Parse(txtID.Text), txtPass.Text, txtName.Text, txtEmail.Text, txtPhone.Text, txtAddress.Text, txtSalary.Text,
                lbCheckPass, lbCheckName, lbCheckPhone, lbCheckSalary, lbCheckEmail, lbCheckAddress);
            if (!IsVali)
            {
                return;
            }
            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn cập nhật nhân viên?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                int status = (int)cboStatus.SelectedValue;
                if (txtPass.Text.ToLower().Equals(_user.PasswordHash.ToLower()))
                {
                    _userController.UpdateUser(int.Parse(txtID.Text),txtPass.Text, txtName.Text, txtEmail.Text,
                        txtPhone.Text, cboGender.SelectedItem.ToString(), dtpBirthDate.Value, txtAddress.Text,
                        absolutePath, double.Parse(txtSalary.Text), status, cboRole.SelectedItem.ToString());
                }
                else
                {
                    _userController.UpdateUser(int.Parse(txtID.Text),HashPass.HashPassword(txtPass.Text), txtName.Text, txtEmail.Text,
                        txtPhone.Text, cboGender.SelectedItem.ToString(), dtpBirthDate.Value, txtAddress.Text,
                        absolutePath, double.Parse(txtSalary.Text), status, cboRole.SelectedItem.ToString());
                }

                ShowScreen.ShowMessage("Cập nhật thông tin nhân viên thành công", "Thông báo");
                _OnLoadUser?.Invoke();
                this.Close();
            }
            else
            {
                Console.WriteLine("Cancel add employee");
            }
        }

        private void btnUpdateImage_Click(object sender, EventArgs e)
        {
            // Mở hộp thoại chọn file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                absolutePath = openFileDialog.FileName;
                ptbAvatar.Image = Image.FromFile(absolutePath);
            }
        }

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {
            FormatNumber.FormatAsNumber(txtSalary);
        }
    }
}
