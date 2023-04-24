using System.ComponentModel.DataAnnotations;
using ToDoList.Controllers;

namespace ToDoList.Models
{
    public class EventModel
    {
        public int id { set; get; }

        [Required]
        [MinLength(2, ErrorMessage ="Event name must be longer")]
        [Display(Name = "Title")]
        public string Name { set; get; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { set; get; }
        [Display(Name="Due Date")]
        public DateTime DueDate { set; get; }
        public CategoryModel Category { set; get; }
        [Display(Name ="Completion Status")]
        public bool IsCompleted { set; get; }
        [Display(Name="Category")]
        public List<CategoryModel> categories { set; get; }
        [Display(Name="Category")]
        public CategoryModel SelectedCategory { set; get; }
        public static bool isXML { set; get; }
        static EventModel()
        {
            isXML = false;
        }
    }
}
