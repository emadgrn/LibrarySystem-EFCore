using Azure;
using HW12.Entities;
using HW12.Infrastructure.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Extension;
using HW12.Interfaces.Repositories;

namespace HW12.Authentications
{
    public class Authentication(IUserRepository userRepo)
    {
        private readonly IUserRepository _userRepo = userRepo;

        public bool Login(string username, string password)
        {
            var users = _userRepo.GetAll();



            var user = users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.ConfirmPassword(password));

            if (user == null)
                throw new Exception("Username or password is incorrect.");

            if (!user.IsActive)
                throw new Exception("This User is not Active! Call the main operator.");

            LocalStorage.CurrentUser = user;
            return true;
        }

        public void Logout()
        {
            LocalStorage.CurrentUser = null;
            ConsoleHelper.PrintResult(true, "Logout was successful!");
        }
    }
}
