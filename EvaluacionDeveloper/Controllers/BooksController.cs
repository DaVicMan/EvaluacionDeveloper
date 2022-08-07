using Microsoft.AspNetCore.Mvc;
using EvaluacionDeveloper.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EvaluacionDeveloper.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            List<BookModel> Books = (from DataRow Item in Database.SelectQuery(@"select b.ISBN, b.BookName, b.PublishDate, b.Pages, Authors.FirstName, Authors.LastName from Books as b
                                                                                inner join Authors  on Authors.ID = b.AuthorID ").Rows

                                     select new BookModel 
                                     {
                                      ISBN = (string)Item["ISBN"],
                                      BookName = (string)Item["BookName"],
                                      PublishDate = ((DateTime)Item["PublishDate"]).Date,
                                      PageCount = (int)Item["Pages"],
                                      Author = $"{(string)Item["FirstName"]} {(string)Item["LastName"]}"
                                     }).ToList();

            return View(Books);
        }

        public IActionResult BooksByAuthor(int Author)
        {
            List<BookModel> Books = (from DataRow Item in Database.SelectQuery(@$"select b.ISBN, b.BookName, b.PublishDate, b.Pages, Authors.FirstName, Authors.LastName from Books as b
                                                                                inner join Authors  on Authors.ID = b.AuthorID where AuthorID = {Author}").Rows

                                     select new BookModel
                                     {
                                         ISBN = (string)Item["ISBN"],
                                         BookName = (string)Item["BookName"],
                                         PublishDate = ((DateTime)Item["PublishDate"]).Date,
                                         PageCount = (int)Item["Pages"],
                                         Author = $"{(string)Item["FirstName"]} {(string)Item["LastName"]}"
                                     }).ToList();

            return View("Index",Books);
        }
        public IActionResult DeleteBook(string ISBN)
        {
            int i = Database.ExecuteQuery($"delete from Books where ISBN = '{ISBN}'");

            return RedirectToAction("Index", "Books");

        }
        [HttpPost]
        public JsonResult SubmitCreatedBook(string ISBN, string BookName, DateTime PublishDate, int Pages, int AuthorID)
        {
            int i = Database.ExecuteQuery($@"insert into Books(ISBN, BookName, PublishDate, Pages, AuthorID) values
                                            ('{ISBN}','{BookName}', '{PublishDate.ToString("yyyy-MM-dd")}', {Pages}, {AuthorID})");

            return Json(i);
        }
        public JsonResult SubmitEditedBook(string ISBN, string BookName, DateTime PublishDate, int Pages, int AuthorID)
        {
            int i = Database.ExecuteQuery($@"update Books
                                            set BookName = '{BookName}', PublishDate = '{PublishDate.ToString("yyyy-MM-dd")}', Pages = {Pages}, AuthorID = {AuthorID}
                                            where ISBN = '{ISBN}'");

            return Json(i);
        }

        public JsonResult GetAuthors()
        {
            return Json( Database.GetAuthors());
        }
        public JsonResult GetBookInformation(string ISBN)
        {
           DataRow Book = Database.SelectQuery(@$"select b.ISBN, b.BookName, b.PublishDate, b.Pages, b.AuthorID from Books as b
                                                where ISBN = '{ISBN}'").Rows[0];

            return Json(new { BookName = (string)Book["BookName"], Pages = (int)Book["Pages"], 
                PublishDate = Convert.ToDateTime(Book["PublishDate"]).ToString("yyyy-MM-dd"), AuthorID = (int)Book["AuthorID"]});
        }
    }
}
