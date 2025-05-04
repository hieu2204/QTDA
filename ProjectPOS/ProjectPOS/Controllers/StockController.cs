using ProjectPOS.Models;
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
    public class StockController
    {
        public int GetNextInvoiceID()
        {
            return IStockService.GetNextInvoiceID();
        }
        public void SubmitStockReceipt(int userid, decimal totalamount, DataGridView dgv)
        {
            IStockService.SubmitStockReceipt(userid, totalamount, dgv);
        }
        public List<PurInvoiceDTO> GetAllPurchaseInvoice(int pagenumber, int pagesize)
        {
            return IStockService.GetListPurchaseInvoice(pagenumber, pagesize);
        }
        public int GetTotalPagePurchaseInvoice(int pagesize)
        {
            return IStockService.GetTotalPagePurchaseInvoice(pagesize);
        }
    }
}
