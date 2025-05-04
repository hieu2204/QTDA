using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ProjectPOS.Controllers;
using ProjectPOS.Models.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjectPOS.Views.Forms
{
    public partial class SellDetail : Form
    {
        private SellDetailController _sellDetailController = new SellDetailController();
        private SellDTO sellDTOs;

        public SellDetail()
        {
            InitializeComponent();
        }

        public void Init(SellDTO sellDTO)
        {
            sellDTOs = sellDTO;
            if (sellDTO != null)
            {
                lbID.Text = sellDTO.OrderID.ToString();
                lbName.Text = sellDTO.EmployeeName;
                lbDate.Text = sellDTO.OrderDate.ToString("dd/MM/yyyy");
                lbTotalAmount.Text = sellDTO.FinalTotalAmount.ToString("N0");
                LoadDataPurchaseDetail();
            }
        }

        public void LoadDataPurchaseDetail()
        {
            List<SellDetailDTO> purDetails = _sellDetailController.GetListSellDetail(sellDTOs.OrderID);

            if (purDetails != null)
            {
                dgvCartDetail.DataSource = null;
                dgvCartDetail.DataSource = purDetails;
                FormatDataGridView();
            }
        }

        private void FormatDataGridView()
        {
            if (dgvCartDetail.Columns.Count == 0) return;
            dgvCartDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCartDetail.ColumnHeadersHeight = 50;
            dgvCartDetail.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dgvCartDetail.Columns["ProductName"].Width = 300;
            dgvCartDetail.Columns["Quantity"].HeaderText = "Số lượng";
            dgvCartDetail.Columns["Quantity"].Width = 230;
            dgvCartDetail.Columns["Quantity"].DefaultCellStyle.Format = "N0";
            dgvCartDetail.Columns["UnitPrice"].HeaderText = "Giá nhập";
            dgvCartDetail.Columns["UnitPrice"].Width = 250;
            dgvCartDetail.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
            dgvCartDetail.Columns["Price"].HeaderText = "Thành tiền";
            dgvCartDetail.Columns["Price"].Width = 120;
            dgvCartDetail.Columns["Price"].DefaultCellStyle.Format = "N0";
            dgvCartDetail.Columns["Price"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCartDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCartDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCartDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvCartDetail.ScrollBars = ScrollBars.Vertical;
            dgvCartDetail.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void ptbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Kiểm tra file có bị khóa hay không
        private bool IsFileInUse(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return false; // File không bị khóa
                }
            }
            catch (IOException)
            {
                return true; // File bị khóa
            }
        }

        // Hàm xử lý xuất PDF
        private void btnExport_Click(object sender, EventArgs e)
        {
            string tempFilePath = null;
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (sellDTOs == null)
                {
                    MessageBox.Show("Không có dữ liệu hóa đơn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (sellDTOs.OrderID == 0)
                {
                    MessageBox.Show("Mã đơn hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"HoaDon_{sellDTOs.OrderID}.pdf"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                string filePath = saveFileDialog.FileName;
                tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

                // Sử dụng font Unicode hỗ trợ tiếng Việt
                //string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                //PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                // Font Arial Unicode MS - hỗ trợ đầy đủ tiếng Việt
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Arial.ttf");
                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

                //PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, true);


                // Tạo PDF
                using (var writer = new PdfWriter(tempFilePath))
                using (var pdf = new PdfDocument(writer))
                using (var document = new Document(pdf))
                {
                    // Header
                    document.Add(new Paragraph("HÓA ĐƠN BÁN HÀNG")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16)
                        .SetFont(font)
                        );

                    // Thông tin đơn hàng
                    document.Add(new Paragraph($"Mã đơn hàng: {sellDTOs.OrderID}").SetFont(font));
                    document.Add(new Paragraph($"Nhân viên: {sellDTOs.EmployeeName}").SetFont(font));
                    document.Add(new Paragraph($"Ngày bán: {sellDTOs.OrderDate:dd/MM/yyyy}").SetFont(font));
                    document.Add(new Paragraph($"Tổng tiền: {sellDTOs.TotalAmount:N0} VND").SetFont(font));
                    document.Add(new Paragraph($"Tổng tiền sau giảm: {sellDTOs.FinalTotalAmount:N0} VND").SetFont(font));
                    document.Add(new Paragraph("\n"));

                    // Tạo bảng sản phẩm
                    Table table = new Table(4).UseAllAvailableWidth();

                    // Header table
                    table.AddHeaderCell(CreateCell("Tên sản phẩm", font, true));
                    table.AddHeaderCell(CreateCell("Số lượng", font, true));
                    table.AddHeaderCell(CreateCell("Đơn giá", font, true));
                    table.AddHeaderCell(CreateCell("Thành tiền", font, true));

                    // Thêm dữ liệu
                    var purDetails = _sellDetailController.GetListSellDetail(sellDTOs.OrderID);
                    if (purDetails?.Any() != true)
                    {
                        MessageBox.Show("Không có chi tiết đơn hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    foreach (var detail in purDetails)
                    {
                        table.AddCell(CreateCell(detail.ProductName, font));
                        table.AddCell(CreateCell(detail.Quantity.ToString(), font));
                        table.AddCell(CreateCell(detail.UnitPrice.ToString("N0"), font));
                        table.AddCell(CreateCell(detail.Price.ToString("N0"), font));
                    }

                    document.Add(table);
                }

                // Xử lý file
                if (File.Exists(filePath)) File.Delete(filePath);
                File.Move(tempFilePath, filePath);

                MessageBox.Show("Xuất PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (tempFilePath != null && File.Exists(tempFilePath))
                {
                    try { File.Delete(tempFilePath); } catch { }
                }
            }
        }

        // Hàm hỗ trợ tạo ô table
        // Trong hàm CreateCell
        private Cell CreateCell(string content, PdfFont font, bool isHeader = false)
        {
            Paragraph paragraph = new Paragraph(content)
                .SetFont(font)
                .SetFontSize(isHeader ? 10 : 9);

            return new Cell()
                .Add(paragraph)
                .SetPadding(5)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetHorizontalAlignment(
                    isHeader
                    ? iText.Layout.Properties.HorizontalAlignment.CENTER
                    : iText.Layout.Properties.HorizontalAlignment.LEFT
                );
        }
    }
}
