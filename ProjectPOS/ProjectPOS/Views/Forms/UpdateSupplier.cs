using ProjectPOS.Controllers;
using ProjectPOS.Models;
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
using System.Xml.Linq;

namespace ProjectPOS.Views.Forms
{
    public partial class UpdateSupplier : Form
    {
        private SupplierModel _supplier = new SupplierModel();
        private SupplierController _supplierController = new SupplierController();
        public event Action _OnLoadSup;
        public UpdateSupplier()
        {
            InitializeComponent();
        }
        public void Init(SupplierModel supplier)
        {
            List<KeyValuePair<string, int>> statusList = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("Đang hợp tác", 1),
                new KeyValuePair<string, int>("Ngưng hợp tác", 0)
            };
            if (supplier != null)
            {
                _supplier = supplier;
                txtID.Text = supplier.SupplierID.ToString();
                txtName.Text = supplier.SupplierName;
                txtPhone.Text = supplier.Phone;
                txtAddress.Text = supplier.Address;
                txtEmail.Text = supplier.Email;
                cboStatus.DataSource = statusList;
                cboStatus.DisplayMember = "Key";
                cboStatus.ValueMember = "Value";
                dtpCreateAt.Value = supplier.CreateAt;
                dtpUpdateAt.Value = supplier.UpdateAt;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsValid = Validation.ValditionInfoSup(txtName.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text,
        int.Parse(txtID.Text), lbCheckName, lbCheckPhone, lbCheckEmail, lbCheckAddress);

            if (!IsValid)
            {
                return;
            }

            DialogResult result = ShowScreen.ShowConfirmDialog("Bạn có chắc chắn muốn cập nhật nhà cung cấp?", "Xác nhận");
            if (result == DialogResult.Yes)
            {
                int status = (int)cboStatus.SelectedValue;
                _supplierController.UpdateSupplier(int.Parse(txtID.Text), txtName.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text, status);
                ShowScreen.ShowMessage("Cập nhật thông tin nhà cung cấp thành công", "Thông báo");
                _OnLoadSup?.Invoke();
                this.Close();
            }
            else
            {
                Console.WriteLine("Cancel update supplier");
            }
        }
    }
}
