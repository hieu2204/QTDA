using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Servies
{
    public class UserServiceWrapper : IUserServiceWrapper
    {
        public bool IsUsernameExit(string username) => IUserService.IsUsernameExit(username);
        public bool IsPhoneExists(string phone) => IUserService.IsPhoneExists(phone);
        public bool IsEmailExists(string email) => IUserService.IsEmailExists(email);
    }
}
