using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Dashboard.MVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }       
    }
}
