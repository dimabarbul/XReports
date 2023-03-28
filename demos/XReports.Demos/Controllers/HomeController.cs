using Microsoft.AspNetCore.Mvc;

namespace XReports.Demos.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.View();
    }
}
