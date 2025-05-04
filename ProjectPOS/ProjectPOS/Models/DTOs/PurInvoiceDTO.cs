using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models.DTOs
{
    public class PurInvoiceDTO
    {
        public int ReceiptID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal TotalAmount { get; set; }
        public PurInvoiceDTO() { }
        public PurInvoiceDTO(int receiptID, string employeeName, DateTime receiptDate, decimal totalAmount)
        {
            ReceiptID = receiptID;
            EmployeeName = employeeName;
            ReceiptDate = receiptDate;
            TotalAmount = totalAmount;
        }
    }
}
