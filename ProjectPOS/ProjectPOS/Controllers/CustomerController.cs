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
    public class CustomerController
    {
        public List<CustomerModel> GetAllCustomer(int pagenumber,int pagesize)
        {
            return ICustomerService.GetListCus(pagenumber, pagesize);
        }
        public int GetTotalPageCus(int pagesize)
        {
            return ICustomerService.GetTotalPage(pagesize);
        }
        public void UpdateCus (string ID, string Name, string Phone, string Email, string Address, int Status, int LoyaltyPoint)
        {
            ICustomerService.UpdateCus(ID, Name, Phone, Email, Address, Status, LoyaltyPoint);
        }
        public void InsertCus (string name, string phone, string email, string address)
        {
            ICustomerService.InsertCus(name, phone, email, address);
        }
        public List<CustomerDTO> GetCusList()
        {
            return ICustomerService.GetListCustomer();
        }
        public void ArchiveCustomer(int customerID)
        {
            ICustomerService.ArchiveCustomer(customerID);
        }
    }
}
