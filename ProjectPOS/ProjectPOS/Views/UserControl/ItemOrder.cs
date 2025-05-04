using ProjectPOS.Models.DTOs;
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
    public partial class ItemOrder : UserControl
    {
        private SellDTO _sellDTO;   
        public ItemOrder()
        {
            InitializeComponent();
        }
        public void Init(SellDTO sellDTO)
        {
            _sellDTO = sellDTO;
            if (sellDTO != null)
            {
                txtID.Text = sellDTO.OrderID.ToString();
                txtUserName.Text = sellDTO.EmployeeName;
                txtCustomerName.Text = sellDTO.CustomerName;

                txtDate.Text = sellDTO.OrderDate.ToString("dd/MM/yyyy");
                txtTotalAmount.Text = sellDTO.TotalAmount.ToString("N0");
                txtFinalTotalAmount.Text = sellDTO.FinalTotalAmount.ToString("N0");
            }
        }

        private void ptbViewDetail_Click(object sender, EventArgs e)
        {
            SellDetail detail = new SellDetail();

            detail.Init(_sellDTO);
            detail.ShowDialog();
        }
    }
}
