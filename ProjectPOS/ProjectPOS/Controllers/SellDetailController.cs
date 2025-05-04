using ProjectPOS.Models.DTOs;
using ProjectPOS.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Controllers
{
    public class SellDetailController
    {
        public List<SellDetailDTO> GetListSellDetail(int orderID)
        {
            return ISellDetailService.GetListSellDetail(orderID);
        }
    }
}
