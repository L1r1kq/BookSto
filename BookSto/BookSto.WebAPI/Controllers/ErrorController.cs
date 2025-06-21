using Microsoft.AspNetCore.Mvc;

namespace BookSto.WebAPI.Controllers;

public class ErrorController : Controller
{
    [Route("Error/404")]
    public IActionResult Error404() => View("Error404");

    [Route("Error/403")]
    public IActionResult Error403() => View("Error403");

    [Route("Error/500")]
    public IActionResult Error500() => View("Error500");

    [Route("Error/{code}")]
    public IActionResult ErrorByCode(int code)
    {
        return View($"Error{code}");
    }
}
