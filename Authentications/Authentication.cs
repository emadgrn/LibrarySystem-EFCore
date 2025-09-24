using Azure;
using HW12.Entities;
using HW12.Infrastructure.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Entities.Enums;
using HW12.Extension;
using HW12.Interfaces.Authentications;
using HW12.Interfaces.Repositories;
using HW12.Interfaces.Validators;

namespace HW12.Authentications
{
    public class Authentication(IUserRepository userRepo, IValidator validate):IAuthentication
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IValidator _validate = validate;

        public void Register(string firstName, string lastName, string username, string password,int role)
        {
            if (!_validate.UsernameValidator(username) || !_validate.PasswordValidator(password))
                throw new Exception("Invalid username or password!");
            
            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Password = password,
                Role = (RoleEnum)role
            };
            _userRepo.Create(user);
        }

        public bool Login(string username, string password)
        {
            var users = _userRepo.GetAll();

            var user = users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.ConfirmPassword(password));

            if (user == null)
                return false;

            if (!user.IsActive)
                throw new Exception("This user is not Active! Call the operators.");

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
