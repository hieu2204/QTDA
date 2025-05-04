using ProjectPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectPOS.Utilities;
using ProjectPOS.Controllers;
using Guna.UI2.WinForms;
using ProjectPOS.Servies;
namespace ProjectPOS.Views.Forms
{
    public partial class UpdateCustomer : Form
    {
        private CustomerModel _customer;
        private CustomerController _customerController = new CustomerController();
        public event Action _OnLoadCus;
        public UpdateCustomer()
        {
            InitializeComponent();
        }

        public void init(CustomerModel customer)
        {
            // Tạo danh sách trạng thái
            List<KeyValuePair<string, int>> statusList = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("Đang hoạt động", 1),
                new KeyValuePair<string, int>("Không hoạt động", 0)
            };
            if (customer != null)
            {
                _customer = customer;
                txtID.Text = customer.ID.ToString();
                txtName.Text = customer.Name;
                txtEmail.Text = customer.Email;
                txtAddress.Text = customer.Address;
                txtLoyalty.Text = customer.LoyaltyPoint.ToString();
                cboStatus.DataSource = statusList;
                cboStatus.DisplayMember = "Key";
                cboStatus.ValueMember = "Value";
                txtPhone.Text = customer.Phone;
                dtpCreate.Value = customer.CreateAt;
                dtpUpdate.Value = customer.UpdateAt;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validation.ValidationInfoCustomer(txtName.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text, txtLoyalty.Text, int.Parse(txtID.Text),
     lbCheckName, lbCheckPhone, lbCheckEmail, lbCheckAddress, lbCheckLoyaltyPoint))
            {
                return;
            }


            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn cập nhật khách hàng?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                int status = (int)cboStatus.SelectedValue;
                _customerController.UpdateCus(txtID.Text, txtName.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text, status, int.Parse(txtLoyalty.Text));
                ShowScreen.ShowMessage("Cập nhật thông tin khách hàng thành công", "Thông báo");
                _OnLoadCus?.Invoke();
                this.Close();
            }
            else
            {
                Console.WriteLine("Cancel update customer");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
