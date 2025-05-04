using ProjectPOS.Controllers;
using ProjectPOS.Models;
using ProjectPOS.Utilities;
using ProjectPOS.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS
{
    public partial class ItemSupplier : UserControl
    {
        private SupplierModel _supplier = new SupplierModel();
        public event Action OnLoadClick;
        public ItemSupplier()
        {
            InitializeComponent();
        }
        public void Init(SupplierModel supplier)
        {
            if(supplier != null)
            {
                _supplier = supplier;
                txtID.Text = supplier.SupplierID.ToString();
                txtName.Text = supplier.SupplierName;
                txtPhone.Text = supplier.Phone;
                txtEmail.Text = supplier.Email;
                txtAddress.Text = supplier.Address;
            }
        }

        private void ptbUpdate_Click(object sender, EventArgs e)
        {
            UpdateSupplier updateSupplier = new UpdateSupplier();
            updateSupplier.Init(_supplier);
            updateSupplier._OnLoadSup += () =>
            {
                OnLoadClick?.Invoke();
            };
            updateSupplier.ShowDialog();
        }

        private void ptbDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = ShowScreen.ShowConfirmDialog(
       "Bạn có chắc chắn muốn lưu trữ nhà cung cấp này?",
       "Xác nhận"
   );

            if (result == DialogResult.Yes)
            {
                // Gọi Controller để lưu trữ nhà cung cấp
                SupplierController controller = new SupplierController();
                controller.ArchiveSupplier(_supplier.SupplierID);

                ShowScreen.ShowMessage("Nhà cung cấp đã được lưu trữ thành công!", "Thông báo");

                // Gọi event để cập nhật lại danh sách nhà cung cấp
                OnLoadClick?.Invoke();
            }
        }
    }
}
