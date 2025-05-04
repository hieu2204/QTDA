using ProjectPOS.Models;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Controllers
{
    public class PromotionController
    {
        public void AddPromotion(string Name, string Description, string DiscountType, DateTime StartDate, DateTime EndDate,
            int? ProductID, int? MinQuantity, int? AppliesToOrder, int? GiftProductID, int? GiftQuantity, decimal? MinBillAmount, decimal? DiscountValue)
        {
            IPromotionService.AddPromotion(Name, Description, DiscountType, StartDate, EndDate, ProductID, MinQuantity, AppliesToOrder, GiftProductID, GiftQuantity, MinBillAmount, DiscountValue);
        }
        public List<PromotionModel> GetListPromotion(int pageNumber, int pageSize)
        {
            return IPromotionService.GetPromotionList(pageNumber, pageSize);
        }
        public int GetTotalPage(int pageSize)
        {
            return IPromotionService.GetTotalPage(pageSize);
        }
        public List<PromotionDTO> GetAllPromotion()
        {
            return IPromotionService.GetActivePromotions();
        }
    }
}
