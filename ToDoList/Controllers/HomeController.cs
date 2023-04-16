using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.Models;
using ToDoList.DataAccess.EventProcessor;
using ToDoList.DataAccess.SQLDataAccess;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var data = EventProcessor.LoadEvents();
            List<EventModel> events = new List<EventModel>();
            foreach (var row in data)
            {

                events.Add(new EventModel
                {
                    id = row.id,
                    Name = row.name,
                    DateCreated = row.date_created,
                    DueDate = row.due_date,
                    IsCompleted = row.is_completed,
                    SelectedCategory = new CategoryModel { name=row.category}
                });
            }
            return View(events);
        }
        public IActionResult CategoryList()
        {
            var categories = EventProcessor.LoadCategories();
            return View(categories);
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                EventProcessor.AddNewCategory(model.name);
            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteCategory(CategoryModel model)
        {
            var data = EventProcessor.LoadCategory(model.id);
            model.name = data[0].name;
            return View(model);
        }
        public IActionResult DeleteCategoryAction(CategoryModel model)
        {
            EventProcessor.DeleteCategory(model.id);
            return RedirectToAction("Index");
        }
        public IActionResult AddNewEvent()
        {
            var model = new EventModel();
            model.categories = EventProcessor.LoadCategories();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewEvent(EventModel model)
        {
                EventProcessor.AddNewEvent(model.Name, model.DueDate, model.SelectedCategory);
                return RedirectToAction("Index");
        }
        public IActionResult DeleteEvent(EventModel model)
        {
            var data = EventProcessor.LoadEvent(model.id);
            model.Name = data[0].name;
            model.DateCreated = data[0].date_created;
            model.DueDate = data[0].due_date;
            model.SelectedCategory = new CategoryModel { name = data[0].category };
            model.IsCompleted = data[0].is_completed;
            return View(model);
        }
        public IActionResult Delete(EventModel model)
        {
            EventProcessor.DeleteEvent(model.id);
            return RedirectToAction("Index");
        }
        public IActionResult ChangeCompleteStatus(int id)
        {
            EventProcessor.ChangeCompletenes(id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}