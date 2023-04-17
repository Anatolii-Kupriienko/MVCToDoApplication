using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class CategoryModel
    {
        public int id { get; set; }
        [Display(Name="Title")]
        [Required]
        public string name { get; set; }
    }
}
