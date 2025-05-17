using Microsoft.AspNetCore.Mvc;

namespace BookSto.Areas.Admin.Controllers;

[Area("Admin")] // Указываем, что этот контроллер относится к области "Admin"
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
