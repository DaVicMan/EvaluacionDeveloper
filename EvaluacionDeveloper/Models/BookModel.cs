using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EvaluacionDeveloper.Models
{
    public class BookModel
    {
        public string ISBN { get; set; }
        [Display(Name ="Name")]
        public string BookName { get; set; }
        [Display(Name = "Publication Date")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        [Display(Name = "Page Count")]
        public int PageCount { get; set; }
        public string Author { get; set; }
    }
}
