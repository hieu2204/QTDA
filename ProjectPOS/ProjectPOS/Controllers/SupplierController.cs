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
    public class SupplierController
    {
        public List<SupplierModel> GetAllSupplier(int pagenumber, int pagesize)
        {
            return ISupplierService.GetListSupplier(pagenumber, pagesize);
        }
        public int GetTotalPageSupp(int pagesize)
        {
            return ISupplierService.GetTotalPageSupplier(pagesize);
        }
        public void UpdateSupplier(int ID, string Name, string Phone, string Email, string Address, int Status)
        {
            ISupplierService.UpdateSupplier(ID, Name, Phone, Email, Address, Status);
        }
        public void InsertSup(string name, string phone, string email, string address)
        {
            ISupplierService.InsertSup(name, phone, email, address);
        }
        public List<SupplierDTO> GetSuppliers()
        {
            return ISupplierService.GetSuppliers();
        }
        public void ArchiveSupplier(int supplierID)
        {
            ISupplierService.ArchiveSupplier(supplierID);
        }
        public List<SupplierModel> SearchSupplier(string keyword)
        {
            return ISupplierService.SearchSupplier(keyword);
        }

    }
}
