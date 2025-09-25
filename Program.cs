using Azure;
using HW12.Authentications;
using HW12.Entities.Enums;
using HW12.Extension;
using HW12.Infrastructure.Local;
using HW12.Infrastructure.Repositories;
using HW12.Interfaces.Authentications;
using HW12.Interfaces.Repositories;
using HW12.Interfaces.Services;
using HW12.Interfaces.Validators;
using HW12.Services;
using HW12.Validators;
using Spectre.Console;

namespace HW12
{
    internal class Program
    {
        private static readonly IUserRepository _userRepo = new UserRepository();
        private static readonly IBorrowedBookRepository _borrowedBookRepo = new BorrowedBookRepository();
        private static readonly IBookRepository _bookRepo = new BookRepository();
        private static readonly ICategoryRepository _categoryRepo = new CategoryRepository();
        private static readonly IReviewRepository _reviewRepo = new ReviewRepository();
        private static readonly IValidator _validate = new Validator(_userRepo);
        private static readonly IAuthentication _auth= new Authentication(_userRepo,_validate);
        private static readonly IAdminService _adminService= new AdminService(_userRepo,_borrowedBookRepo,_bookRepo,_categoryRepo,_reviewRepo);
        private static readonly IRegularUserService _regularUserService= new RegularUserService(_userRepo,_borrowedBookRepo,_bookRepo, _categoryRepo,_reviewRepo);

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                ConsoleHelper.PrintColorful("MAIN PAGE", null, ConsoleColor.DarkCyan);
                Console.WriteLine("\n");
                Console.WriteLine(" 1. Login");
                Console.WriteLine(" 2. Register");

                var choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (choice)
                {
                    case '1':
                        LoginPage();
                        break;
                    case '2':
                        RegisterPage();
                        break;
                    default:
                        ConsoleHelper.PrintResult(false, "Invalid option! Try again...");
                        break;
                }
            }
        }

        public static void LoginPage()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    ConsoleHelper.PrintColorful("LOGIN PAGE", null, ConsoleColor.DarkCyan);
                    Console.WriteLine("\n");
                    Console.Write("Username: ");
                    string username = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = Console.ReadLine();

                    if (!_auth.Login(username, password))
                        throw new Exception("Invalid username or password.");

                    ConsoleHelper.PrintResult(true, "Login was successful!");

                    switch (LocalStorage.CurrentUser.Role)
                    {
                        case RoleEnum.Admin:
                           // AdminPage();
                            break;
                        case RoleEnum.RegularUser:
                            //RegularUserPage();
                            break; 
                    }

                    return;
                }
                catch (Exception e)
                {
                    ConsoleHelper.PrintResult(false, e.Message);
                    Console.WriteLine("Press 0 to go back to Main Menu, or any other key to try again...");
                    var key = Console.ReadKey().KeyChar;

                    if (key == '0')
                        return;
                }
            }
        }

        public static void RegisterPage()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    ConsoleHelper.PrintColorful("REGISTER PAGE", null, ConsoleColor.DarkCyan);
                    Console.WriteLine("\n");
                    Console.WriteLine("Choose a role to register as:");
                    Console.WriteLine(" 1. Admin");
                    Console.WriteLine(" 2. Regular User");
                    var choice = Console.ReadKey().KeyChar;
                    Console.WriteLine();

                    Console.WriteLine("Let's create a new account:");
                    Console.Write("Username: ");
                    string username = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = Console.ReadLine();
                    Console.Write("First name: ");
                    string firstname = Console.ReadLine();
                    Console.Write("Last name: ");
                    string lastname = Console.ReadLine();

                    int role = choice - '0';
                    if (role < 1 || role > 2)
                    {
                        ConsoleHelper.PrintResult(false, "Invalid option! Try again...");
                        break;
                    }

                    _auth.Register(firstname, lastname, username, password,role);
                    ConsoleHelper.PrintResult(true, "Register was successful!");
                }

                catch (Exception e)
                {
                    ConsoleHelper.PrintResult(false, e.Message);
                    Console.WriteLine("Press 0 to go back to Main Menu, or press any other key to try again...");
                    var key = Console.ReadKey().KeyChar;

                    if (key == '0')
                        return;
                }
            }
        }

        // public static void AdminPage()
        // {
        //     while (true)
        //     {
        //         try
        //         {
        //             Console.Clear();
        //             Console.BackgroundColor = ConsoleColor.DarkCyan;
        //             Console.WriteLine("OPERATOR PAGE");
        //             Console.ResetColor();
        //             Console.WriteLine();
        //             Console.WriteLine(" 1. Show your profile");
        //             Console.WriteLine(" 2. Show all users info");
        //             Console.WriteLine(" 3. Show all classes info");
        //             Console.WriteLine(" 4. Activate user");
        //             Console.WriteLine(" 5. Deactivate user");
        //             Console.WriteLine(" 6. Activate class");
        //             Console.WriteLine(" 7. Deactivate class");
        //             Console.WriteLine(" 8. Log out");
        //
        //             var choice = Console.ReadKey().KeyChar;
        //             Console.WriteLine();
        //
        //             switch (choice)
        //             {
        //                 case '1':
        //                     {
        //                         Console.Clear();
        //                         _operatorService.ShowOperatorInfo((Operator)LocalStorage.CurrentUser);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '2':
        //                     {
        //                         Console.Clear();
        //                         _operatorService.ShowAllUsers((Operator)LocalStorage.CurrentUser);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '3':
        //                     {
        //                         Console.Clear();
        //                         _operatorService.ShowAllClasses();
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '4':
        //                     {
        //                         Console.Clear();
        //                         Console.Write("Enter user Id: ");
        //                         int id = int.Parse(Console.ReadLine());
        //                         _operatorService.ActivateUser(id);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '5':
        //                     {
        //                         Console.Clear();
        //                         Console.Write("Enter user Id: ");
        //                         int id = int.Parse(Console.ReadLine());
        //                         _operatorService.DeactivateUser(id);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '6':
        //                     {
        //                         Console.Clear();
        //                         Console.Write("Enter class Id: ");
        //                         int id = int.Parse(Console.ReadLine());
        //                         _operatorService.ActivateClass(id);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '7':
        //                     {
        //                         Console.Clear();
        //                         Console.Write("Enter class Id: ");
        //                         int id = int.Parse(Console.ReadLine());
        //                         _operatorService.DeactivateClass(id);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '8':
        //                     {
        //                         Console.Clear();
        //                         _auth.Logout();
        //                         return;
        //                     }
        //                 default:
        //                     {
        //                         ConsoleHelper.PrintResult(false, "Invalid option! Try again...");
        //                         break;
        //                     }
        //             }
        //         }
        //         catch (Exception e)
        //         {
        //             ConsoleHelper.PrintResult(false, e.Message);
        //             Console.WriteLine("Press 0 to go back to Main Menu, or press any other key to try again...");
        //             var key = Console.ReadKey().KeyChar;
        //
        //             if (key == '0')
        //                 return;
        //         }
        //     }
        // }
        //
        // public static void RegularUserPage()
        // {
        //     while (true)
        //     {
        //         try
        //         {
        //             Console.Clear();
        //             Console.BackgroundColor = ConsoleColor.DarkCyan;
        //             Console.WriteLine("OPERATOR PAGE");
        //             Console.ResetColor();
        //             Console.WriteLine();
        //             Console.WriteLine(" 1. Show your profile");
        //             Console.WriteLine(" 2. Show all users info");
        //             Console.WriteLine(" 3. Show all classes info");
        //             Console.WriteLine(" 4. Activate user");
        //             Console.WriteLine(" 5. Deactivate user");
        //             Console.WriteLine(" 6. Activate class");
        //             Console.WriteLine(" 7. Deactivate class");
        //             Console.WriteLine(" 8. Log out");
        //
        //             var choice = Console.ReadKey().KeyChar;
        //             Console.WriteLine();
        //
        //             switch (choice)
        //             {
        //                 case '1':
        //                     {
        //                         Console.Clear();
        //                         _operatorService.ShowOperatorInfo((Operator)LocalStorage.CurrentUser);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '2':
        //                     {
        //                         Console.Clear();
        //                         _operatorService.ShowAllUsers((Operator)LocalStorage.CurrentUser);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '3':
        //                     {
        //                         Console.Clear();
        //                         _operatorService.ShowAllClasses();
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '4':
        //                     {
        //                         Console.Clear();
        //                         Console.Write("Enter user Id: ");
        //                         int id = int.Parse(Console.ReadLine());
        //                         _operatorService.ActivateUser(id);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '5':
        //                     {
        //                         Console.Clear();
        //                         Console.Write("Enter user Id: ");
        //                         int id = int.Parse(Console.ReadLine());
        //                         _operatorService.DeactivateUser(id);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '6':
        //                     {
        //                         Console.Clear();
        //                         Console.Write("Enter class Id: ");
        //                         int id = int.Parse(Console.ReadLine());
        //                         _operatorService.ActivateClass(id);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '7':
        //                     {
        //                         Console.Clear();
        //                         Console.Write("Enter class Id: ");
        //                         int id = int.Parse(Console.ReadLine());
        //                         _operatorService.DeactivateClass(id);
        //                         Console.ReadKey();
        //                         break;
        //                     }
        //                 case '8':
        //                     {
        //                         Console.Clear();
        //                         _auth.Logout();
        //                         return;
        //                     }
        //                 default:
        //                     {
        //                         ConsoleHelper.PrintResult(false, "Invalid option! Try again...");
        //                         break;
        //                     }
        //             }
        //         }
        //         catch (Exception e)
        //         {
        //             ConsoleHelper.PrintResult(false, e.Message);
        //             Console.WriteLine("Press 0 to go back to Main Menu, or press any other key to try again...");
        //             var key = Console.ReadKey().KeyChar;
        //
        //             if (key == '0')
        //                 return;
        //         }
        //     }
        // }
    }
}
