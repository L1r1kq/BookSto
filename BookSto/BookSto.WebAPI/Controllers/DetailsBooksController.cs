using BookSto.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSto.WebAPI.Controllers;

[Route("[controller]")]
public class DetailsBooksController : Controller
{
    private readonly ApplicationDbContext _context;

    public DetailsBooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var book = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book); // или путь к вью, если нужен
    }
}