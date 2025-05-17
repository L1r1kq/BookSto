using Microsoft.AspNetCore.Mvc;

namespace BookSto.WebAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View(); // ищет в /Areas/Admin/Views/Dashboard/Index.cshtml
        }
    }
}