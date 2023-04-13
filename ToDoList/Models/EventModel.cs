using System.ComponentModel.DataAnnotations;

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
        [Display(Name="Due Date(Optional)")]
        public DateTime DueDate { set; get; }
        [Display(Name = "Category")]
        public int CategoryId { set; get; }
        [Display(Name ="Completion Status")]
        public bool IsCompleted { set; get; }
    }
}
