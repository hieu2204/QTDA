using ProjectPOS.Models;
using ProjectPOS.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Controllers
{
    public class UserController
    {
        public void InsertUser(string user, string pass, string name, string email, string phone, string gender, DateTime birth, string address, string image, double salary, string role)
        {
            IUserService.InsertUser(user, pass, name, email, phone, gender, birth, address, image, salary, role);
        }
        public List<UserModel> GetAllUser(int pagenumber,int pagesize)
        {
            return IUserService.GetListUser(pagenumber,pagesize);
        }
        public List<UserModel> GetAllUser()
        {
            return IUserService.GetListUser();
        }
        public int GetTotalPageUser(int pagesize)
        {
            return IUserService.GetTotalPage(pagesize);
        }
        public void UpdateUser(int id,string pass, string name, string email, string phone, string gender,
            DateTime birth, string address, string imageURL, double salary, int status, string role)
        {
            IUserService.UpdateUser(id,pass, name, email, phone, gender, birth,address, imageURL, salary, status, role);
        }
        public List<UserModel> SearchUser(string keyword)
        {
            return IUserService.SearchUsers(keyword);
        }

    }
}
