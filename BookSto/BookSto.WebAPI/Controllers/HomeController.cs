using System.Diagnostics;
using BookSto.Persistence;
using Microsoft.AspNetCore.Mvc;
using BookSto.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSto.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;


    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    
    [Route("home")]
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books
            .Include(b => b.Author)  // Включаем автора
            .Include(b => b.Category) // Включаем категорию
            .ToListAsync();

        return View(books); // Передаем книги в представление
    }
    
    
}