using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookSto.WebAPI.Controllers;

[Route("terms-of-service")]
public class TermOfServiceController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}