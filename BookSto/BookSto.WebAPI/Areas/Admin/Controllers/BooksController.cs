using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSto.Persistence;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookSto.Domain.Models;
using Microsoft.AspNetCore.Authorization;


namespace BookSto.WebAPI.Areas.Admin.Controllers   
{
    /// <summary>Список книг в админ-панели.</summary>
    [Area("Admin")]                               
    [Route("Admin/[controller]")]    
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }
        // GET /Admin/Books   
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();

            return View(books);                   
        }
        
        [Authorize]
        [HttpGet("OtherPage")]
        public async Task<IActionResult> OtherPage()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();

            return View("~/Views/Home/Index.cshtml", books);
        }
        
        // GET: Books/Details/5
        // GET: Admin/Books/Details/6
        [Authorize]
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

            return View("~/Areas/Admin/Views/Details/Details.cshtml", book);
        }
        
        
        // GET: Admin/Books/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Запрос на редактирование книги с Id = {BookId}", id);

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                _logger.LogWarning("Книга с Id = {BookId} не найдена в базе данных.", id);
                return NotFound();
            }

            _logger.LogInformation("Книга найдена: Title = {Title}, Author = {Author}, Category = {Category}",
                book.Title, book.Author?.Name, book.Category?.Name);

            var authors = await _context.Authors.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            _logger.LogDebug("Загружено {AuthorCount} авторов и {CategoryCount} категорий для выпадающих списков.",
                authors.Count, categories.Count);

            ViewData["AuthorId"] = new SelectList(authors, "Id", "Name", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", book.CategoryId);

            return View(book);
        }


        
        // POST: Admin/Books/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AuthorId,CategoryId,Description,Price,Rating,ImageUrl")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Book updated successfully. Redirecting to Books Index.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                _logger.LogInformation("Redirecting to Books Index...");
                return RedirectToAction("Index", "Books", new { area = "Admin" });

            }


            // Если модель не прошла валидацию, вернуть пользователю форму редактирования с ошибками
            ViewData["AuthorId"] = new SelectList(await _context.Authors.ToListAsync(), "Id", "Name", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", book.CategoryId);
            return View("~/Areas/Admin/Views/Books/Edit.cshtml", book);
        }


        
        
        // Удалить книгу
        [Authorize(Roles = "Admin")]
        [HttpPost("Delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            // Явный редирект на базовый маршрут
            return RedirectToAction("", "Books", new { area = "Admin" }); // Это редирект на /Admin/Books
        }
        
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

    }
}