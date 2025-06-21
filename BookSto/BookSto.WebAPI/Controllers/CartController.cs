// Controllers/CartController.cs
using System.Threading.Tasks;
using BookSto.Application.Interfaces;
using BookSto.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookSto.WebAPI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cart;
        private readonly UserManager<ApplicationUser> _users;

        public CartController(ICartService cart, UserManager<ApplicationUser> users)
        {
            _cart  = cart;
            _users = users;
        }

        private string GetUserId() => _users.GetUserId(User);

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var uid   = GetUserId();
            var count = _cart.GetCountAsync(uid).Result;
            return Json(count);
        }

        [HttpGet("/Cart")]
        public async Task<IActionResult> Index()
        {
            var uid   = GetUserId();
            var items = await _cart.GetItemsAsync(uid);
            return View(items);
        }
        
        /// <summary>
        /// Увеличить количество на qty (по умолчанию +1)
        /// </summary>
        [HttpPost("/Cart/Increment/{id:int}")]
        public async Task<IActionResult> Increment(int id, int qty = 1)
        {
            var uid = GetUserId();
            await _cart.AddAsync(uid, id, qty);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var count = await _cart.GetCountAsync(uid);
                return Json(new { count });
            }

            return RedirectBack();
        }
        
        
        /// <summary>
        /// Уменьшить количество на qty (по умолчанию –1)
        /// Если после этого в корзине не останется ни одного, позиция удалится
        /// </summary>
        [HttpPost("/Cart/Decrement/{id:int}")]
        public async Task<IActionResult> Decrement(int id, int qty = 1)
        {
            var uid = GetUserId();
            // предположим, что AddAsync понимает отрицательные qty,
            // иначе добавьте в ICartService отдельный метод DecreaseAsync
            await _cart.AddAsync(uid, id, -qty);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var count = await _cart.GetCountAsync(uid);
                return Json(new { count });
            }

            return RedirectBack();
        }
        
        
        [Authorize]
        [HttpPost("/Cart/Add/{id:int}")]
        public async Task<IActionResult> Add(int id, int qty = 1)
        {
            var uid = GetUserId();
            await _cart.AddAsync(uid, id, qty);

            // получаем обновлённое количество в корзине
            var count = await _cart.GetCountAsync(uid);

            // если это AJAX-запрос — вернём JSON, иначе — редирект
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { count });

            return RedirectBack();
        }

        [HttpPost("/Cart/Remove/{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {
            var uid   = GetUserId();
            await _cart.RemoveAsync(uid, id);
            return RedirectBack();
        }

        [HttpPost("/Cart/Clear")]
        public async Task<IActionResult> Clear()
        {
            var uid   = GetUserId();
            await _cart.ClearAsync(uid);
            return RedirectToAction(nameof(Index));
        }

        private IActionResult RedirectBack()
        {
            var referer = Request.Headers["Referer"].ToString();
            return !string.IsNullOrEmpty(referer)
                ? Redirect(referer)
                : RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateQuantity()
        {
            throw new NotImplementedException();
        }
    }
}
