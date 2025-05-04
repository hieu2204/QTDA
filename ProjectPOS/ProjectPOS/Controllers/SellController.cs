using ProjectPOS.Models.DTOs;
using ProjectPOS.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS.Controllers
{
    public class SellController
    {
        public int GetNextSellID()
        {
            return ISellService.GetNextID();
        }
        public void SaveSellOrder(int userId, decimal totalAmount, decimal finalTotalAmount, DataGridView dgvCart, int? customerId, int paymentStatus)
        {
            // Gọi OrderService để lưu đơn hàng; nếu có lỗi, exception sẽ được ném ra
            ISellService.SubmitSellOrder(userId, totalAmount, finalTotalAmount, dgvCart, customerId,paymentStatus);
        }
        public List<SellDTO> GetAllOrder(int pageNumber, int pageSize)
        {
            return ISellService.GetListOrder(pageNumber, pageSize);
        }
        public int GetTotalPageOrder(int pageSize) 
        {
            return ISellService.GetTotalPageOrder(pageSize);
        }
        public List<SellDTO> SearchOrder(string keyword)
        {
            return ISellService.SearchOrder(keyword);
        }
    }
}
