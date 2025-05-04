using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models.DTOs
{
    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public int LoyaltyPoints {  get; set; }
        public override string ToString()
        {
            return $"{CustomerID} - {CustomerName} - {Phone}";
        }
    }
}
