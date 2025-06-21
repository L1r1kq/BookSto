using BookSto.Application.Common;
using BookSto.Application.Features;
using BookSto.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using BookSto.Persistence;
using Microsoft.Extensions.Logging;

namespace BookSto.Application.Features
{
    // public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Result<Book>>
    // {
    //     private readonly ApplicationDbContext _context;
    //     private readonly ILogger<AddBookCommandHandler> _logger;
    //
    //     public AddBookCommandHandler(ApplicationDbContext context, ILogger<AddBookCommandHandler> logger)
    //     {
    //         _context = context;
    //         _logger = logger;
    //     }
    //
    //     public async Task<Result<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
    //     {
    //         _logger.LogInformation("Попытка добавить книгу: {Title}", request.Title);
    //         try
    //         {
    //             var book = new Book
    //             {
    //                 Title = request.Title,
    //                 Description = request.Description,
    //                 Rating = request.Rating,
    //                 AuthorId = request.AuthorId,
    //                 CategoryId = request.CategoryId,
    //                 ImageUrl = request.ImagePath,
    //                 PdfUrl = request.PdfPath,
    //                 Price = request.Price
    //             };
    //
    //             _logger.LogInformation("Добавление книги в контекст: {Title}", book.Title);
    //             _context.Books.Add(book);
    //             await _context.SaveChangesAsync(cancellationToken);
    //             _logger.LogInformation("Книга успешно добавлена: {BookId}", book.Id);
    //
    //             return Result<Book>.Success(book);
    //         }
    //         catch (Exception ex)
    //         {
    //             _logger.LogError(ex, "Ошибка при добавлении книги: {Message}", ex.Message);
    //             return Result<Book>.Failure("Произошла ошибка при добавлении книги.");
    //         }
    //     }
    // }
}