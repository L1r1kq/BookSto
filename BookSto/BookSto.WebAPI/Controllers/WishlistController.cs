using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSto.Domain.Models;
using BookSto.Persistence;

namespace BookSto.WebAPI.Controllers;

[Authorize]
public class WishlistController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public WishlistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("/Wishlist/Add/{id:int}")]
    public async Task<IActionResult> Add(int id)
    {
        var userId = _userManager.GetUserId(User);

        var exists = await _context.WishlistItems
            .AnyAsync(w => w.BookId == id && w.UserId == userId);

        if (!exists)
        {
            var item = new WishlistItem
            {
                UserId = userId,
                BookId = id
            };
            _context.WishlistItems.Add(item);
            await _context.SaveChangesAsync();
        }

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { added = true });

        return RedirectToAction("Index");
    }


    [HttpGet("wishlist")]
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var wishlist = await _context.WishlistItems
            .Include(w => w.Book)
            .Where(w => w.UserId == userId)
            .ToListAsync();

        return View(wishlist);
    }
    
    [HttpPost("Wishlist/Remove/{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var userId = _userManager.GetUserId(User);
        var item = await _context.WishlistItems
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

        if (item != null)
        {
            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}