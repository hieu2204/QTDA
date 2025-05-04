using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models.DTOs
{
    public class SellDTO
    {
        public int OrderID { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerName { get; set; } 
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalTotalAmount { get; set; }
    }
}
