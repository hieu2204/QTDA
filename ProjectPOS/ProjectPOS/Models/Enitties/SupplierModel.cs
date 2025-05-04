using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models
{
    public class SupplierModel
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public SupplierModel() { }
        public SupplierModel(int supplierID, string supplierName, string phone, string email, string address, int status, DateTime createAt, DateTime updateAt)
        {
            SupplierID = supplierID;
            SupplierName = supplierName;
            Phone = phone;
            Email = email;
            Address = address;
            Status = status;
            CreateAt = createAt;
            UpdateAt = updateAt;
        }
    }
}
