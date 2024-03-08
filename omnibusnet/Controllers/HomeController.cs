using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using omnibusnet.Models;

namespace omnibusnet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public class Book
    {
        public string BookName { get; set; }
        public string BookAuthor { get; set; }

        public string ISBN { get; set; }
    }

    public IActionResult Index()
    {
        List<Book> books = new List<Book>();

        try
        {
            using (SqlConnection sqlCon = new SqlConnection())
            {
                sqlCon.ConnectionString = "Server=tcp:omnibus.database.windows.net,1433;Initial Catalog=Inventory;Persist Security Info=False;User ID=CloudSA7a07e689;Password=1qaz!QAZ;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                sqlCon.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Books", sqlCon);
                SqlDataReader sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    Book b = new Book();
                    b.BookName = sqlReader["BookName"].ToString();
                    b.BookAuthor = sqlReader["BookAuthor"].ToString();
                    b.ISBN = sqlReader.GetSqlString(3).Value;

                    books.Add(b);
                }

                sqlReader.Close();
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message != null ? ex.Message.ToString() : "no exception message";
            ViewBag.StackTrace = ex.StackTrace != null ? ex.StackTrace.ToString() : "no stack trace";
        }

        return View(books);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
