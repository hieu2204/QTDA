using ProjectPOS.Controllers;
using ProjectPOS.Models.DTOs;
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
    public partial class PurchaseDetail : Form
    {
        private PurInvoiceDTO purchaseInvoiceDTO = new PurInvoiceDTO();
        private StockDetailController StockDetailController = new StockDetailController();
        public PurchaseDetail()
        {
            InitializeComponent();
        }
        public void Init(PurInvoiceDTO purInvoice)
        {
            purchaseInvoiceDTO = purInvoice;
            if(purInvoice != null )
            {
                Console.WriteLine("TotalPrice:" + purInvoice.TotalAmount);

                lbID.Text = purInvoice.ReceiptID.ToString();
                lbName.Text = purInvoice.EmployeeName;
                lbDate.Text = purInvoice.ReceiptDate.ToString("dd/MM/yyyy") ?? "";
                lbTotalAmount.Text = purInvoice.TotalAmount.ToString();
                Console.WriteLine("Lb TotalAmount: " + lbTotalAmount.Text);
                //FormatNumber.FormatAsNumber(lbTotalAmount);
                LoadDataPurchaseDetail();
            }

        }

        private void ptbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void LoadDataPurchaseDetail()
        {
            // Lấy danh sách sản phẩm từ database
            List<PurDetailDTO> purDetails = StockDetailController.GetListPurchase(int.Parse(lbID.Text));

            if (purDetails != null)
            {
                // Gán dữ liệu vào DataGridView
                dgvCartDetail.DataSource = null; // Reset tránh lỗi
                dgvCartDetail.DataSource = purDetails;

                // Định dạng lại cột sau khi gán dữ liệu
                FormatDataGridView();
            }
        }
        
        private void FormatDataGridView()
        {
            // Đảm bảo có dữ liệu trước khi định dạng
            if (dgvCartDetail.Columns.Count == 0) return;
            dgvCartDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCartDetail.ColumnHeadersHeight = 50;
            dgvCartDetail.Columns["ReceiptID"].Visible = false;
            // Đặt kích thước cột
            dgvCartDetail.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dgvCartDetail.Columns["ProductName"].Width = 200;

            dgvCartDetail.Columns["CategoryName"].HeaderText = "Loại sản phẩm";
            dgvCartDetail.Columns["CategoryName"].Width = 150;
            dgvCartDetail.Columns["CategoryName"].ReadOnly = true;

            dgvCartDetail.Columns["SupplierName"].HeaderText = "Nhà cung cấp";
            dgvCartDetail.Columns["SupplierName"].Width = 150;
            dgvCartDetail.Columns["SupplierName"].ReadOnly = true;

            dgvCartDetail.Columns["Quantity"].HeaderText = "Số lượng";
            dgvCartDetail.Columns["Quantity"].Width = 100;
            dgvCartDetail.Columns["Quantity"].DefaultCellStyle.Format = "N0";

            dgvCartDetail.Columns["UnitPrice"].HeaderText = "Giá nhập";
            dgvCartDetail.Columns["UnitPrice"].Width = 120;
            dgvCartDetail.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";

            dgvCartDetail.Columns["TotalPrice"].HeaderText = "Thành tiền";
            dgvCartDetail.Columns["TotalPrice"].Width = 150;
            dgvCartDetail.Columns["TotalPrice"].DefaultCellStyle.Format = "N0";
            dgvCartDetail.Columns["TotalPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // Căn giữa tiêu đề và dữ liệu
            dgvCartDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCartDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Thiết lập chế độ hiển thị bảng
            dgvCartDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvCartDetail.ScrollBars = ScrollBars.Vertical;

            // Cho phép tự động wrap text nếu nội dung dài
            dgvCartDetail.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvCartDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
    }
}
