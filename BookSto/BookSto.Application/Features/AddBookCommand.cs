using BookSto.Application.Common;
using BookSto.Domain.Models;
using MediatR;

namespace BookSto.Application.Features
{
    public class AddBookCommand : IRequest<Result<Book>>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public decimal Rating { get; init; }
        public int AuthorId { get; init; }
        public int CategoryId { get; init; }
        public string ImagePath { get; init; }
        public string PdfPath { get; init; }
        public decimal Price { get; init; }
    }
}