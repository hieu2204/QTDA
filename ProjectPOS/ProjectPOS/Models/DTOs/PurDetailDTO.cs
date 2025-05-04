using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models.DTOs
{
    public class PurDetailDTO
    {
        public int ReceiptID { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Convert.ToDecimal(Quantity * UnitPrice);
        public PurDetailDTO() {
        }
        public PurDetailDTO(int receiptID, string productName, string categoryName, string supplierName, int quantity, decimal unitPrice)
        {
            ReceiptID = receiptID;
            ProductName = productName;
            CategoryName = categoryName;
            SupplierName = supplierName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
