using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models
{
    public class CustomerModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int LoyaltyPoint { get; set; }
        public int Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public CustomerModel() { }
        public CustomerModel(int iD, string name, string phone, string email, string address, int loyaltyPoint, int status, DateTime createAt, DateTime updateAt)
        {
            ID = iD;
            Name = name;
            Phone = phone;
            Email = email;
            Address = address;
            LoyaltyPoint = loyaltyPoint;
            Status = status;
            CreateAt = createAt;
            UpdateAt = updateAt;
        }
    }
}
