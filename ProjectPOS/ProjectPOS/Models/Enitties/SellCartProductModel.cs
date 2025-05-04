using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models.Enitties
{
    public class SellCartProductModel
    {
       
            public int ProductID { get; set; }
            public int Quantity { get; set; }         // Số lượng mua của khách
            public int GiftQuantity { get; set; }       // Số lượng quà tặng
            public int TotalQuantity { get; set; }      // Tổng số lượng (mua + tặng)
            public double UnitPrice { get; set; }       // Đơn giá áp dụng cho số lượng mua
            public double TotalPrice { get; set; }      // Thành tiền = Quantity * UnitPrice
    }
}
