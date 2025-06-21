using BookSto.Domain.Models;
using BookSto.Persistence;
using BookSto.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookSto.Application.DTOs;
using BookSto.Application.Features;
using BookSto.Application.Features.Auth.Commands;
using Microsoft.AspNetCore.Authorization;

namespace BookSto.WebAPI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AddBookController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddBookController> _logger;

        public AddBookController(IMediator mediator, ApplicationDbContext context, ILogger<AddBookController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        // GET: Admin/AddBook/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            ViewBag.Authors = await _context.Authors
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                .ToListAsync();

            return View(new CreateBookRequestDto());
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookRequestDto dto)
        {
            if (!ModelState.IsValid) return View(dto);          // MVC вариант

            var result = await _mediator.Send(new CreateBookCommand(dto));
            if (result.IsSuccess) return RedirectToAction("",           // действие
                "Books",            // контроллер
                new { area = "Admin" }); // область;

            ModelState.AddModelError(string.Empty, result.Error);
            return View(dto);
        }


        private async Task<string> HandleFileUpload(IFormFile file, string folder)
        {
            if (file == null)
            {
                _logger.LogDebug("HandleFileUpload called with null file for folder {Folder}", folder);
                return null;
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine("wwwroot", folder, fileName);

            _logger.LogInformation("Saving file {OriginalFileName} to {FilePath}", file.FileName, filePath);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folder}/{fileName}";
        }
    }
}
