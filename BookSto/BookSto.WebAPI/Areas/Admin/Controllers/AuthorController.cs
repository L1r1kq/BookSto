using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSto.Domain.Models;
using BookSto.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace BookSto.WebAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(ApplicationDbContext context, ILogger<AuthorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET /Admin/Author
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors.AsNoTracking().ToListAsync();
            return View(authors);                 // ~/Areas/Admin/Views/Author/Index.cshtml
        }
        
        // GET /Admin/Author/Create
        [HttpGet("Create")]
        public IActionResult Create() => View(); // ~/Areas/Admin/Views/Author/Create.cshtml

        // POST /Admin/Author/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Author author)
        {
            if (!ModelState.IsValid) return View(author);

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Автор создан: {Name}", author.Name);

            return RedirectToAction("", "Author", new { area = "Admin" });
        }

        // GET /Admin/Author/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            return View(author);                 // ~/Areas/Admin/Views/Author/Edit.cshtml
        }

        // POST /Admin/Author/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Author author)
        {
            if (id != author.Id) return NotFound();
            if (!ModelState.IsValid) return View(author);

            try
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Автор обновлён: {Id}", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id)) return NotFound();
                throw;
            }

            return RedirectToAction("", "Author", new { area = "Admin" });
        }

        // POST /Admin/Author/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Автор удалён: {Id}", id);

            return RedirectToAction("", "Author", new { area = "Admin" });
        }

        private bool AuthorExists(int id) =>
            _context.Authors.Any(e => e.Id == id);
    }
}
