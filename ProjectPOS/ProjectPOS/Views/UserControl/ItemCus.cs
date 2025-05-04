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
using ProjectPOS.Views.Forms;
using ProjectPOS.Controllers;
using ProjectPOS.Utilities;
namespace ProjectPOS
{
    public partial class ItemCus : UserControl
    {
        private CustomerModel _customer;
        public event Action OnLoadClick;
        private CustomerController _customerController = new CustomerController();
        public ItemCus()
        {
            InitializeComponent();
            init(_customer);
        }
        public void init(CustomerModel customer)
        {
            if(customer != null)
            {
                _customer = customer;
                txtID.Text = customer.ID.ToString();
                txtName.Text = customer.Name;
                txtPhone.Text = customer.Phone;
                txtAddress.Text = customer.Address;

            }
        }

        private void ptbEdit_Click(object sender, EventArgs e)
        {
            UpdateCustomer updateCustomer = new UpdateCustomer();
            updateCustomer.init(_customer);
            updateCustomer._OnLoadCus += () =>
            {
                OnLoadClick?.Invoke(); // // Gọi event khi cập nhật xong
            };
            updateCustomer.ShowDialog();
        }

        private void ptbDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = ShowScreen.ShowConfirmDialog(
      "Bạn có chắc chắn muốn lưu trữ khách hàng này?",
      "Xác nhận"
  );

            if (result == DialogResult.Yes)
            {
                // Gọi thủ tục lưu trữ khách hàng (status = 0)
                _customerController.ArchiveCustomer(_customer.ID);

                ShowScreen.ShowMessage("Khách hàng đã được lưu trữ thành công!", "Thông báo");

                // Gọi event để cập nhật lại danh sách khách hàng
                OnLoadClick?.Invoke();
            }
            else
            {
                Console.WriteLine("Cancel archive customer");
            }
        }
    }
}
