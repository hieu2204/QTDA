using ProjectPOS.Models.DTOs;
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
    public partial class ItemStock : UserControl
    {
        private PurInvoiceDTO _purchase = new PurInvoiceDTO();
        public ItemStock()
        {
            InitializeComponent();
        }
        public void Init(PurInvoiceDTO purInvoiceDTO)
        {
            _purchase = purInvoiceDTO;
            if(purInvoiceDTO != null)
            {
                txtID.Text = purInvoiceDTO.ReceiptID.ToString();
                txtName.Text = purInvoiceDTO.EmployeeName;
                Console.WriteLine("Tổng giá đơn nhập: " + purInvoiceDTO.TotalAmount);
                txtTotalAmount.Text = purInvoiceDTO.TotalAmount.ToString("N0");
                txtDate.Text = purInvoiceDTO.ReceiptDate.ToString("dd/MM/yyyy") ?? "";
            }
        }

        private void ptbViewDetail_Click(object sender, EventArgs e)
        {
            PurchaseDetail detail = new PurchaseDetail();
            detail.Init(_purchase);
            detail.ShowDialog();
        }
    }
}
