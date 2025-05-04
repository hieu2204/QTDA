using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string ImageURL { get; set; }
        public double Salary { get; set; }
        public int Status { get; set; }
        public string Role { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public UserModel() { }
        public UserModel(int id, string userName, string passwordHash, string name, string email, string phone, string gender, DateTime birthDate, string address, string imageURL, double salary, int status, string role, DateTime createAt, DateTime updateAt)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Name = name;
            Email = email;
            Phone = phone;
            Gender = gender;
            BirthDate = birthDate;
            Address = address;
            ImageURL = imageURL;
            Salary = salary;
            Status = status;
            Role = role;
            CreateAt = createAt;
            UpdateAt = updateAt;
        }
        public enum UserRole
        {
            Admin,
            Manager,
            Employee
        }
    }
}
