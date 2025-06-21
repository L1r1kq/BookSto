using BookSto.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookSto.WebAPI.Controllers;

public class CategoryPageController : Controller

{
    private readonly ApplicationDbContext _context;
    
    public CategoryPageController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("categoryPage")]
    [HttpGet]
    public async Task<IActionResult> Index(string search, int? categoryId, int? authorId)
    {
        // Базовый запрос с подгрузкой навигационных свойств
        var q = _context.Books
            .Include(b => b.Category)
            .Include(b => b.Author)
            .AsQueryable();

        // Фильтр по названию
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(b => EF.Functions.ILike(b.Title, $"%{search}%"));

        // Фильтр по категории
        if (categoryId.HasValue)
            q = q.Where(b => b.CategoryId == categoryId.Value);

        // Фильтр по автору
        if (authorId.HasValue)
            q = q.Where(b => b.AuthorId == authorId.Value);

        var books = await q.ToListAsync();

        // Для выпадающих списков на view
        ViewBag.Categories = new SelectList(
            await _context.Categories.OrderBy(c => c.Name).ToListAsync(),
            "Id", "Name", categoryId
        );
        ViewBag.Authors = new SelectList(
            await _context.Authors.OrderBy(a => a.Name).ToListAsync(),
            "Id", "Name", authorId
        );

        ViewBag.Search = search;

        return View(books);
    }
}