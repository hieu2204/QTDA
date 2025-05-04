using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models.DTOs
{
    public class PromotionDTO
    {
        public int PromotionID { get; set; }  // ID của khuyến mãi
        public string Name { get; set; }  // Tên khuyến mãi
        public string Description { get; set; } // Mô tả khuyến mãi
        public DateTime StartDate { get; set; }  // Ngày bắt đầu
        public DateTime EndDate { get; set; }  // Ngày kết thúc
        public string DiscountType { get; set; }  // 1: Giảm %, 2: Giảm tiền, 3: Tặng sản phẩm
        public decimal? MinTotalAmount { get; set; }
        public decimal? DiscountValue { get; set; }  // Giá trị giảm giá (nếu có)
        public int? ProductID { get; set; }  // Sản phẩm cần mua (nếu áp dụng cho sản phẩm cụ thể)
        public int? MinQuantity { get; set; }  // Số lượng tối thiểu để áp dụng
        public int AppliesToOrder { get; set; } // Áp dụng cho đơn hàng (true/false)
        public int? GiftProductID { get; set; }  // Sản phẩm quà tặng (nếu có)
        public int? GiftQuantity { get; set; }  // Số lượng quà tặng (nếu có)
    }
}
