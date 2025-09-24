using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Interfaces.Authentications
{
    public interface IAuthentication
    {
        void Register(string firstName, string lastName, string username, string password, int role);
        bool Login(string username, string password);
        void Logout();
    }
}
