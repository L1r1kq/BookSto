using BookSto.Application.Common;
using BookSto.Application.DTOs;
using MediatR;

namespace BookSto.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<Result<Unit>>
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public bool AcceptTerms { get; init; }
}