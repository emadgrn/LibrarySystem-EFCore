using Azure;
using HW12.Authentications;
using HW12.DTO;
using HW12.Entities;
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
using System.Net;
using HW12.Infrastructure.DataAccess;
using HW12.Infrastructure;

namespace HW12
{
    internal class Program
    {
        private static readonly AppDbContext _appDbContext = new AppDbContext();
        private static readonly IUserRepository _userRepo = new UserRepository(_appDbContext);
        private static readonly UnitOfWork _unitOfWork = new UnitOfWork(_appDbContext);
        private static readonly IBorrowedBookRepository _borrowedBookRepo = new BorrowedBookRepository(_appDbContext);
        private static readonly IBookRepository _bookRepo = new BookRepository(_appDbContext);
        private static readonly ICategoryRepository _categoryRepo = new CategoryRepository(_appDbContext);
        private static readonly IReviewRepository _reviewRepo = new ReviewRepository(_appDbContext);
        private static readonly IValidator _validate = new Validator(_userRepo);
        private static readonly IAuthentication _auth = new Authentication(_userRepo, _validate);
        private static readonly IAdminService _adminService = new AdminService(_userRepo, _borrowedBookRepo, _bookRepo, _categoryRepo, _reviewRepo, _unitOfWork);
        private static readonly IRegularUserService _regularUserService = new RegularUserService(_userRepo, _borrowedBookRepo, _bookRepo, _categoryRepo, _reviewRepo, _unitOfWork);

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
                    string username = Console.ReadLine()!;
                    Console.Write("Password: ");
                    string password = Console.ReadLine()!;

                    if (!_auth.Login(username, password))
                        throw new Exception("Invalid username or password.");

                    ConsoleHelper.PrintResult(true, "Login was successful!");

                    switch (LocalStorage.CurrentUser.Role)
                    {
                        case RoleEnum.Admin:
                            AdminPage();
                            break;
                        case RoleEnum.RegularUser:
                            RegularUserPage();
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
                    string username = Console.ReadLine()!;
                    Console.Write("Password: ");
                    string password = Console.ReadLine()!;
                    Console.Write("First name: ");
                    string firstname = Console.ReadLine()!;
                    Console.Write("Last name: ");
                    string lastname = Console.ReadLine()!;

                    int role = choice - '0';
                    if (role < 1 || role > 2)
                    {
                        ConsoleHelper.PrintResult(false, "Invalid option! Try again...");
                        break;
                    }

                    _auth.Register(firstname, lastname, username, password, role);
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

        public static void AdminPage()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("ADMIN PAGE");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine(" 1.Show your profile");
                    Console.WriteLine(" 2.Show all of the users info");
                    Console.WriteLine(" 3.Show all of the categories");
                    Console.WriteLine(" 4.Show all of the books");
                    Console.WriteLine(" 5.Show all of the reviews");
                    Console.WriteLine(" 6.Show complete info of a book");
                    Console.WriteLine(" 7.Activate a user");
                    Console.WriteLine(" 8.Deactivate a user");
                    Console.WriteLine(" 9.Approve a review");
                    Console.WriteLine(" 10.Reject a review");
                    Console.WriteLine(" 11.Create new category");
                    Console.WriteLine(" 12.Create new book");
                    Console.WriteLine(" 13.Log out");

                    var choice = int.Parse(Console.ReadLine()!);
                    Console.WriteLine();

                    switch (choice)
                    {
                        case 1:
                            {
                                Console.Clear();
                                var result = _adminService.ShowProfile(LocalStorage.CurrentUser!.Id);
                                ShowProfileTable(result);
                                Console.ReadKey();
                                break;
                            }
                        case 2:
                            {
                                Console.Clear();
                                var result = _adminService.ListOfUsers();
                                foreach (var item in result)
                                {
                                    ShowProfileTable(item);
                                    Console.WriteLine();
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 3:
                            {
                                Console.Clear();
                                var result = _adminService.ListOfCategories();
                                ShowCategoriesTree(result);
                                Console.ReadKey();
                                break;
                            }
                        case 4:
                            {
                                Console.Clear();
                                var result = _adminService.ListOfBooks();
                                ShowBooksTable(result);
                                Console.ReadKey();
                                break;
                            }
                        case 5:
                            {
                                Console.Clear();
                                var result = _adminService.ListOfReviews();
                                ShowReviewsTable(result);
                                Console.ReadKey();
                                break;
                            }
                        case 6:
                            {
                                Console.Clear();
                                Console.Write("Enter book Id: ");
                                int bookId = int.Parse(Console.ReadLine()!);
                                var result = _adminService.ShowCompleteInfoOfBook(bookId);
                                ShowCompleteBookInfoTree(result);
                                Console.ReadKey();
                                break;
                            }
                        case 7:
                            {
                                Console.Clear();
                                Console.Write("Enter user Id: ");
                                int userId = int.Parse(Console.ReadLine()!);
                                if (_adminService.ActivateUser(userId))
                                {
                                    ConsoleHelper.PrintResult(true, $"User with ID {userId} got activated!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 8:
                            {
                                Console.Clear();
                                Console.Write("Enter user Id: ");
                                int userId = int.Parse(Console.ReadLine()!);
                                if (_adminService.DeactivateUser(userId))
                                {
                                    ConsoleHelper.PrintResult(true, $"User with ID {userId} got deactivated!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 9:
                            {
                                Console.Clear();
                                Console.Write("Enter review Id: ");
                                int reviewId = int.Parse(Console.ReadLine()!);
                                if (_adminService.ApproveReview(reviewId))
                                {
                                    ConsoleHelper.PrintResult(true, $"Review with ID {reviewId} has been approved!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 10:
                            {
                                Console.Clear();
                                Console.Write("Enter review Id: ");
                                int reviewId = int.Parse(Console.ReadLine()!);
                                if (_adminService.RejectReview(reviewId))
                                {
                                    ConsoleHelper.PrintResult(true, $"Review with ID {reviewId} has been rejected!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 11:
                            {
                                Console.Clear();
                                Console.Write("Enter category name: ");
                                string categoryName = Console.ReadLine()!;
                                int categoryId = _adminService.CreateCategory(categoryName);
                                if (categoryId != 0)
                                {
                                    ConsoleHelper.PrintResult(true, $"Category with ID {categoryId} has been created!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 12:
                            {
                                Console.Clear();
                                Console.Write("Enter title: ");
                                string title = Console.ReadLine()!;
                                Console.Write("Enter author: ");
                                string author = Console.ReadLine()!;
                                Console.Write("Enter category Id: ");
                                int categoryId = int.Parse(Console.ReadLine()!);
                                int bookId = _adminService.CreateBook(title, author, categoryId);
                                if (bookId != 0)
                                {
                                    ConsoleHelper.PrintResult(true, $"Book with ID {bookId} has been created!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 13:
                            {
                                Console.Clear();
                                _auth.Logout();
                                return;
                            }
                        default:
                            {
                                ConsoleHelper.PrintResult(false, "Invalid option! Try again...");
                                break;
                            }
                    }
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

        public static void RegularUserPage()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("REGULAR USER PAGE");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine(" 1.Show your profile");
                    Console.WriteLine(" 2.Show all of the books");
                    Console.WriteLine(" 3.Show complete info of a book");
                    Console.WriteLine(" 4.Show all of the categories");
                    Console.WriteLine(" 5.Show all of your borrowed books");
                    Console.WriteLine(" 6.Show all of your reviews");
                    Console.WriteLine(" 7.Borrow book");
                    Console.WriteLine(" 8.Create review");
                    Console.WriteLine(" 9.Edit review's comment");
                    Console.WriteLine(" 10.Delete review");
                    Console.WriteLine(" 11.Log out");

                    var choice = int.Parse(Console.ReadLine()!);
                    Console.WriteLine();

                    switch (choice)
                    {
                        case 1:
                            {
                                Console.Clear();
                                var result = _regularUserService.ShowProfile(LocalStorage.CurrentUser!.Id);
                                ShowProfileTable(result);
                                Console.ReadKey();
                                break;
                            }
                        case 2:
                            {
                                Console.Clear();
                                var result = _regularUserService.ListOfBooks();
                                ShowBooksTable(result);
                                Console.ReadKey();
                                break;
                            }
                        case 3:
                            {
                                Console.Clear();
                                Console.Write("Enter book Id: ");
                                int bookId = int.Parse(Console.ReadLine()!);
                                var result = _regularUserService.ShowCompleteInfoOfBook(bookId);
                                ShowCompleteBookInfoTree(result);
                                Console.ReadKey();
                                break;
                            }
                        case 4:
                            {
                                Console.Clear();
                                var result = _regularUserService.ListOfCategories();
                                ShowCategoriesTree(result);
                                Console.ReadKey();
                                break;
                            }
                        case 5:
                            {
                                Console.Clear();
                                var result = _regularUserService.ListOfBorrowedBooksByUser(LocalStorage.CurrentUser!.Id);
                                ShowBorrowedBooksTable(result);
                                Console.ReadKey();
                                break;
                            }
                        case 6:
                            {
                                Console.Clear();
                                var result = _regularUserService.ListOfReviewsByUser(LocalStorage.CurrentUser!.Id);
                                ShowReviewsTable(result);
                                Console.ReadKey();
                                break;
                            }
                        case 7:
                            {
                                Console.Clear();
                                Console.Write("Enter book Id: ");
                                int bookId = int.Parse(Console.ReadLine()!);
                                int borrowedBookId = _regularUserService.BorrowBook(LocalStorage.CurrentUser!.Id, bookId);
                                if (borrowedBookId != 0)
                                {
                                    ConsoleHelper.PrintResult(true, $"Book with ID {bookId} has been borrowed by you!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 8:
                            {
                                Console.Clear();
                                Console.Write("Enter book Id: ");
                                int bookId = int.Parse(Console.ReadLine()!);
                                Console.Write("Enter rating (1 to 5): ");
                                int rating = int.Parse(Console.ReadLine()!);
                                Console.Write("Enter comment (optional): ");
                                string? input = Console.ReadLine();
                                string? comment = string.IsNullOrWhiteSpace(input) ? null : input;
                                int reviewId = _regularUserService.CreateReview(LocalStorage.CurrentUser!.Id, bookId, rating, comment!);
                                if (reviewId != 0)
                                {
                                    ConsoleHelper.PrintResult(true, $"Review with ID {reviewId} has been created!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 9:
                            {
                                Console.Clear();
                                Console.Write("Enter review Id: ");
                                int reviewId = int.Parse(Console.ReadLine()!);
                                Console.Write("Enter comment: ");
                                string? input = Console.ReadLine();
                                string? comment = string.IsNullOrWhiteSpace(input) ? null : input;
                                if (_regularUserService.EditReviewComment(LocalStorage.CurrentUser!.Id, reviewId, comment!))
                                {
                                    ConsoleHelper.PrintResult(true, $"Comment of the review with ID {reviewId} has been modified!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 10:
                            {
                                Console.Clear();
                                Console.Write("Enter review Id: ");
                                int reviewId = int.Parse(Console.ReadLine()!);
                                if (_regularUserService.DeleteReview(LocalStorage.CurrentUser!.Id, reviewId))
                                {
                                    ConsoleHelper.PrintResult(true, $"Review with ID {reviewId} has been deleted!");
                                }
                                Console.ReadKey();
                                break;
                            }
                        case 11:
                            {
                                Console.Clear();
                                _auth.Logout();
                                return;
                            }
                        default:
                            {
                                ConsoleHelper.PrintResult(false, "Invalid option! Try again...");
                                break;
                            }
                    }
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

        public static void ShowBorrowedBooksTable(List<ShowBorrowedBookDto> books)
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold yellow]Borrowed Books[/]");


            table.AddColumn("[cyan]Id[/]");
            table.AddColumn("[green]Borrowed On[/]");
            table.AddColumn("[blue]User[/]");
            table.AddColumn("[magenta]Book[/]");
            table.AddColumn("[grey]Category[/]");

            if (books != null && books.Any())
            {
                foreach (var book in books)
                {
                    table.AddRow(
                        book.BorrowedBookId.ToString(),
                        book.BorrowingDateTime.ToString("yyyy-MM-dd HH:mm"),
                        $"[bold]{book.UserFullName}[/] (Id: {book.UserId})",
                        $"[bold]{book.BookTitle}[/] (Id: {book.BookId})",
                        book.CategoryName
                    );
                }
            }
            else
            {
                table.AddRow("[grey]No borrowed books[/]", "", "", "", "");
            }

            AnsiConsole.Write(table);
        }
        public static void ShowCompleteBookInfoTree(ShowBookDto book)
        {
            var root = new Tree($"[bold yellow]{book.Title}[/] by [blue]{book.Author}[/] " +
                                $"(Category: [green]{book.CategoryName}[/], Avg Rating: [magenta]{book.AverageRating:F1}[/])");

            if (book.ApprovedReviews != null && book.ApprovedReviews.Any())
            {
                foreach (var review in book.ApprovedReviews)
                {
                    var commentText = string.IsNullOrWhiteSpace(review.Comment) ? "[grey]No comment[/]" : review.Comment;
                    root.AddNode($"[cyan]{review.UserFullName}[/] (Id: {review.UserId}) - Rating: [yellow]{review.Rating}[/] - {commentText}");
                }
            }
            else
            {
                root.AddNode("[grey]No approved reviews[/]");
            }

            AnsiConsole.Write(root);
        }

        public static void ShowProfileTable(ShowUserDto user)
        {
            var table = new Table();

            table.Border = TableBorder.Rounded;
            table.Title = new TableTitle("[bold yellow]User Profile[/]");

            table.AddColumn("[bold cyan]Field[/]");
            table.AddColumn("[bold green]Value[/]");

            table.AddRow("Id", user.Id.ToString());
            table.AddRow("Full Name", user.FullName);
            table.AddRow("Username", user.Username);
            table.AddRow("Is Active", user.IsActive ? "[green]Yes[/]" : "[red]No[/]");
            table.AddRow("Role", user.Role.ToString());

            AnsiConsole.Write(table);
        }

        public static void ShowCategoriesTree(List<ShowCategoryDto> categories)
        {
            var tree = new Tree("[bold yellow]Library Categories[/]");

            foreach (var category in categories)
            {

                var categoryNode = tree.AddNode($"[cyan]{category.Name}[/] (Id: {category.Id})");

                if (category.Books != null && category.Books.Any())
                {
                    foreach (var book in category.Books)
                    {
                        categoryNode.AddNode($"[green]{book.Title}[/] by [blue]{book.Author}[/]");
                    }
                }
                else
                {
                    categoryNode.AddNode("[grey]No books in this category[/]");
                }
            }

            AnsiConsole.Write(tree);
        }

        public static void ShowBooksTable(List<ShowBookDto> books)
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold yellow]Books[/]");


            table.AddColumn("[cyan]Id[/]");
            table.AddColumn("[green]Title[/]");
            table.AddColumn("[blue]Author[/]");
            table.AddColumn("[magenta]Category[/]");

            if (books != null && books.Any())
            {
                foreach (var book in books)
                {
                    table.AddRow(
                        book.Id.ToString(),
                        $"[bold]{book.Title}[/]",
                        book.Author,
                        book.CategoryName
                    );
                }
            }
            else
            {
                table.AddRow("[grey]No books available[/]", "", "", "");
            }

            AnsiConsole.Write(table);
        }

        public static void ShowReviewsTable(List<ShowReviewDto> reviews)
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("[bold yellow]Reviews[/]");

            table.AddColumn("[darkorange3]Review Id[/]");
            table.AddColumn("[cyan]User Id[/]");
            table.AddColumn("[green]User Name[/]");
            table.AddColumn("[blue]Book Title[/]");
            table.AddColumn("[white]Comment[/]");
            table.AddColumn("[magenta]Rating[/]");
            table.AddColumn("[yellow]Approved[/]");

            if (reviews != null && reviews.Any())
            {
                foreach (var review in reviews)
                {
                    table.AddRow(
                        review.ReviewId.ToString(),
                        review.UserId.ToString(),
                        review.UserFullName,
                        review.BookTitle,
                        string.IsNullOrWhiteSpace(review.Comment) ? "[grey]No comment[/]" : review.Comment,
                        $"[bold]{review.Rating}[/] / 5",
                        review.IsApproved ? "[green]Yes[/]" : "[red]No[/]"
                    );
                }
            }
            else
            {
                table.AddRow("[grey]No reviews found[/]", "", "", "", "", "");
            }

            AnsiConsole.Write(table);
        }
    }
}
