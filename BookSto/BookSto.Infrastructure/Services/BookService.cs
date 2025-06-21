using BookSto.Application.Common;
using BookSto.Application.DTOs;
using BookSto.Application.Interfaces;
using BookSto.Domain.Models;
using BookSto.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookSto.Infrastructure.Services;

public class BookService : IBookService
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<BookService> _log;

    public BookService(ApplicationDbContext db, 
        ILogger<BookService> log)
    {
        _db = db;  _log = log;
    }

    public async Task<Result<int>> CreateAsync(CreateBookRequestDto dto, CancellationToken ct)
    {
        // 1. Проверить, нет ли дубликата по Title
        if (await _db.Books.AnyAsync(b => b.Title == dto.Title, ct))
            return Result<int>.Failure("Книга с таким названием уже есть");

        

        // 3. Создать сущность
        var book = new Book
        {
            Title       = dto.Title,
            ImageUrl    = dto.ImageUrl,   // уже полная ссылка
            PdfUrl      = dto.PdfUrl,
            AuthorId    = dto.AuthorId,
            CategoryId  = dto.CategoryId,
            Rating      = dto.Rating,
            Price       = dto.Price,
            Description = dto.Description
        };

        _db.Books.Add(book);
        await _db.SaveChangesAsync(ct);

        _log.LogInformation("Создана книга {Id}", book.Id);
        return Result<int>.Success(book.Id);
    }
}
