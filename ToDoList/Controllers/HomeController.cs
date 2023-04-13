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
            EventProcessor.DeleteEvent(model.id);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}