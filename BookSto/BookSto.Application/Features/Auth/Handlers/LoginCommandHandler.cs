using BookSto.Application.Common;
using BookSto.Application.DTOs;
using BookSto.Application.Features.Auth.Commands;
using BookSto.Application.Interfaces.Services;
using MediatR;

namespace BookSto.Application.Features.Auth.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<Unit>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<Unit>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request.Email, request.Password);
    }
}
