using ProjectPOS.Controllers;
using ProjectPOS.Servies;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS.Views.Forms
{
    public partial class AddCustomer : Form
    {
        private CustomerController _customerController = new CustomerController();
        public event Action _OnLoadCus;
        public AddCustomer()
        {
            InitializeComponent();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isValid = Validation.ValidationAddCustomer(
        txtName.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text,
        lbCheckName, lbCheckPhone, lbCheckEmail, lbCheckAddress);

            if (!isValid)
            {
                return;
            }

            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn thêm khách hàng?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                _customerController.InsertCus(txtName.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text);

                ShowScreen.ShowMessage("Thêm thông tin khách hàng thành công", "Thông báo");
                _OnLoadCus?.Invoke();

                txtName.Text = txtPhone.Text = txtEmail.Text = txtAddress.Text = String.Empty;
            }
            else
            {
                Console.WriteLine("Cancel add customer");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
