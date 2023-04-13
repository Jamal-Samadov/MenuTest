using Microsoft.AspNetCore.Mvc;

namespace BookmallMenu.Areas.Panelbookmall2017.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
