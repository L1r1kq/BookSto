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
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Admin/Category
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            return View(categories);                      // ~/Areas/Admin/Views/Category/Index.cshtml
        }

        // GET: /Admin/Category/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);                       // ~/Areas/Admin/Views/Category/Details.cshtml
        }

        // GET: /Admin/Category/Create
        [HttpGet("Create")]
        public IActionResult Create() => View();         // ~/Areas/Admin/Views/Category/Create.cshtml

        // POST: /Admin/Category/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (!ModelState.IsValid) return View(category);

            _context.Add(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Категория создана: {Name}", category.Name);

            return RedirectToAction("", "Category", new { area = "Admin" });
        }

        // GET: /Admin/Category/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);                       // ~/Areas/Admin/Views/Category/Edit.cshtml
        }

        // POST: /Admin/Category/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id) return NotFound();
            if (!ModelState.IsValid) return View(category);

            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Категория обновлена: {Id}", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id)) return NotFound();
                throw;
            }

            return RedirectToAction("", "Category", new { area = "Admin" });
        }
        
        // POST: /Admin/Category/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Категория удалена: {Id}", id);

            return RedirectToAction("", "Category", new { area = "Admin" });
        }


        private bool CategoryExists(int id) =>
            _context.Categories.Any(e => e.Id == id);
    }
}
