using Microsoft.AspNetCore.Mvc;

namespace BusinessExam.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
