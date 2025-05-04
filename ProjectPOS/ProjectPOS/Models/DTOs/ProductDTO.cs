using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models.DTOs
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        //Ghi đè phương thức ToString để hiển thị ID - Name - CategoryName
        public override string ToString()
        {
            return $"{ProductID} - {ProductName} - {CategoryName}";
        }
    }
}
