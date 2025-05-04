using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Servies
{
    public interface IUserServiceWrapper
    {
        bool IsUsernameExit(string username);
        bool IsPhoneExists(string phone);
        bool IsEmailExists(string email);
    }
}
