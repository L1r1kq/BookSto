using Microsoft.AspNetCore.Mvc;

namespace BookSto.Controllers;

public class Admin : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}