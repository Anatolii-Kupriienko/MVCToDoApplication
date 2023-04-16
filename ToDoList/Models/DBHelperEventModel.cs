using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ToDoList.Models
{
    public class DBHelperEventModel
    {
        public int id { set; get; }
        public string name { set; get; }
        public DateTime date_created { set; get; }
        public DateTime due_date { set; get; }
        public int category_id { set; get; }
        public string category { set; get; }
        public bool is_completed { set; get; }
       
    }
}
