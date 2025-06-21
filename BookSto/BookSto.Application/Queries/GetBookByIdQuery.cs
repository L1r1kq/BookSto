using BookSto.Application.Common;
using BookSto.Application.DTOs;
using MediatR;

namespace BookSto.Application.Queries;

public class GetBookByIdQuery : IRequest<Result<BookDto>>
{
    public int Id { get; set; }
}