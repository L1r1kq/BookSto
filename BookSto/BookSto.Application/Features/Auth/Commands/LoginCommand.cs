using BookSto.Application.Common;
using BookSto.Application.DTOs;
using MediatR;

namespace BookSto.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<Result<Unit>>
{
    public string Email { get; init; }
    public string Password { get; init; }
}