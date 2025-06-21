using BookSto.Application.Common;
using BookSto.Application.DTOs;

namespace BookSto.Application.Interfaces;

public interface IBookService
{
    Task<Result<int>> CreateAsync(CreateBookRequestDto dto, CancellationToken ct);
}
