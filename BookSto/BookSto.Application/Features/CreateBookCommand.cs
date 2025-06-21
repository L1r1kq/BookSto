using BookSto.Application.DTOs;
using MediatR;
using BookSto.Application.Common;


namespace BookSto.Application.Features.Auth.Commands;

// Например, возвращаем Id:
public record CreateBookCommand(CreateBookRequestDto Dto)
    : IRequest<Result<int>>;
