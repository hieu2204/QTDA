using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models
{
    public class PromotionModel
    {
        public int PromotionID { get; set; }
        public string PromotionName { get; set; }
        public string DiscountType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now;
        public int Status { get; set; }
    }
}
