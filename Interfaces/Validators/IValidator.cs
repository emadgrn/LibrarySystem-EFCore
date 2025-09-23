using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Interfaces.Validators
{
    interface IValidator
    {
        bool UsernameValidator(string username);
        bool PasswordValidator(string password);
    }
}
