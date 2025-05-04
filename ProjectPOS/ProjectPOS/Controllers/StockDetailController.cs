using ProjectPOS.Models.DTOs;
using ProjectPOS.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Controllers
{
    public class StockDetailController
    {
        public List<PurDetailDTO> GetListPurchase(int receipt)
        {
            return IStockDetailService.GetListPurchaseDetail(receipt);
        }
    }
}
