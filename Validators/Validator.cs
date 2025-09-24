using Azure;
using HW12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Interfaces.Repositories;
using HW12.Interfaces.Validators;

namespace HW12.Validators
{
    public class Validator(IUserRepository userRepo) : IValidator
    {
        private readonly IUserRepository _userRepo = userRepo;

        public bool UsernameValidator(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            if (username.Length < 4) return false;
            if (UsernameCount(username) > 0)
                throw new Exception($"{username} already exists!");

            foreach (char c in username)
            {
                if (!(char.IsLetterOrDigit(c) || c == '_' || c == '.'))
                    return false;
            }
            return true;
        }

        public bool PasswordValidator(string password)
        {
            char[] specials = new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '.', ',', '?' };

            if (!string.IsNullOrEmpty(password) && password.Length >= 8
                                                && password.IndexOfAny(specials) >= 0
                                                && password.Any(char.IsUpper) && password.Any(char.IsLower)) return true;

            return false;
        }

        private int UsernameCount(string username)
        {
            var users = _userRepo.GetAll();
            return users.Count(u => u.Username == username);

        }
    }
}
