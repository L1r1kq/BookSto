using BookSto.Application.Common;
using BookSto.Application.DTOs;
using BookSto.Application.Features.Auth.Commands;
using BookSto.Application.Interfaces.Services;
using MediatR;

namespace BookSto.Application.Features.Auth.Handlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Unit>>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<Unit>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _authService.RegisterAsync(
            request.Email, 
            request.Password, 
            request.ConfirmPassword, 
            request.FirstName, 
            request.LastName, 
            request.AcceptTerms);
    }
}