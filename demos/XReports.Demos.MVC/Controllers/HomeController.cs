using Microsoft.AspNetCore.Mvc;

namespace XReports.Demos.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
