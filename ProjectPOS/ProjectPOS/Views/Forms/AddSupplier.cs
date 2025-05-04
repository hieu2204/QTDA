using ProjectPOS.Controllers;
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
    public partial class AddSupplier : Form
    {
        private SupplierController _supplierController = new SupplierController();
        public event Action _OnLoadSup;
        public AddSupplier()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsValid = Validation.ValditionInfoSup(txtName.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text, 0,
    lbCheckName, lbCheckPhone, lbCheckEmail, lbCheckAddress); // 0 = Thêm mới

            if (!IsValid)
            {
                return;
            }

            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn thêm nhà cung cấp?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                _supplierController.InsertSup(txtName.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text);
                ShowScreen.ShowMessage("Thêm thông tin nhà cung cấp thành công", "Thông báo");
                _OnLoadSup?.Invoke();

                // Xóa dữ liệu nhập sau khi thêm
                txtName.Text = txtEmail.Text = txtPhone.Text = txtAddress.Text = string.Empty;
            }
            else
            {
                Console.WriteLine("Cancel add supplier");
            }

        }
    }
}
