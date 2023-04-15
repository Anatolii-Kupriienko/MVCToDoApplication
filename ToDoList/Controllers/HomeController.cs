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
                    CategoryId = row.category_id,
                    IsCompleted = row.is_completed
                });
            }
            return View(events);
        }
        public IActionResult AddNewEvent()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewEvent(EventModel model)
        {
            if (ModelState.IsValid)
            {
                EventProcessor.AddNewEvent(model.Name, model.DueDate, model.CategoryId);
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult DeleteEvent(EventModel model)
        {
            var data = EventProcessor.LoadEvent(model.id);
            model.Name = data[0].name;
            model.DateCreated = data[0].date_created;
            model.DueDate = data[0].due_date;
            model.CategoryId = data[0].category_id;
            model.IsCompleted = data[0].is_completed;
            return View(model);
        }
        public IActionResult Delete(EventModel model)
        {
            EventProcessor.DeleteEvent(model.id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}