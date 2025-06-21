using Microsoft.AspNetCore.Mvc;

namespace BookSto.WebAPI.Controllers;

[Route("bookPDF")]
public class BookPDF : Controller
{
    
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}