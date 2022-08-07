using Microsoft.AspNetCore.Mvc;
using System.Data;
using EvaluacionDeveloper.Models;

namespace EvaluacionDeveloper.Controllers
{
    public class AuthorsController : Controller
    {
        public IActionResult Index()
        {
            List<AuthorModel> Authors = (from DataRow Item in Database.SelectQuery("select * from Authors").Rows

                                         select new AuthorModel
                                         {
                                             ID = (int)Item["ID"],
                                             FirstName = (string)Item["FirstName"],
                                             LastName = (string)Item["LastName"],
                                             DateOfBirth = ((DateTime)Item["DateOfBirth"]).Date
                                         }).ToList();
            return View(Authors);
        }
        [HttpPost]
        public JsonResult InsertNewAuthor(string FirstName, string LastName, DateTime DateOfBirth)
        {
            int i = Database.ExecuteQuery(@$"insert into Authors(FirstName, LastName, DateOfBirth) values
                                            ('{FirstName}', '{LastName}', '{DateOfBirth.ToString("yyyy-MM-dd")}')");

            return Json(i);
        }
        [HttpPost]
        public JsonResult SubmitEditedUser(int id, string FirstName, string LastName, DateTime DateOfBirth)
        {
            int i = Database.ExecuteQuery(@$"update Authors
                                            set FirstName = '{FirstName}', LastName = '{LastName}', DateOfBirth = '{DateOfBirth.ToString("yyyy-MM-dd")}'
                                            where ID = {id}");

            return Json(i);
        }
        [HttpPost]
        public JsonResult GetAuthorInformation(int id)
        {
            DataRow Author = Database.SelectQuery($"select * from Authors where ID = {id}").Rows[0];

            return Json(new { FirstName = Author["FirstName"], LastName = Author["LastName"], DateOfBirth = Convert.ToDateTime(Author["DateOfBirth"]).ToString("yyyy-MM-dd") });
        }

    }
}
