using System.Threading.Tasks;
using BookSto.Application.Interfaces;
using BookSto.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookSto.WebAPI.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cart;
        private readonly UserManager<ApplicationUser> _users;

        public CheckoutController(ICartService cart, UserManager<ApplicationUser> users)
        {
            _cart  = cart;
            _users = users;
        }

        [Route("checkout")]
        public async Task<IActionResult> Index()
        {
            var uid   = _users.GetUserId(User);
            var items = await _cart.GetItemsAsync(uid);
            // items: List<CartItem> с полем Book (ImageUrl, Title, Price) и Quantity
            return View(items);
        }
    }
}