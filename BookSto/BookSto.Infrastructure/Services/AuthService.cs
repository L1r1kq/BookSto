using BookSto.Application.Common;
using BookSto.Application.Interfaces.Services;
using BookSto.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using BookSto.Application.Interfaces;
using MediatR;

namespace BookSto.Infrastructur.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<Result<Unit>> RegisterAsync(string email, string password, string confirmPassword, string firstName, string lastName, bool acceptTerms)
        {
            if (!acceptTerms)
                return Result<Unit>.Failure("Необходимо принять условия использования");

            if (password != confirmPassword)
                return Result<Unit>.Failure("Пароли не совпадают");

            var user = new ApplicationUser 
            { 
                Email = email, 
                UserName = email, 
                FirstName = firstName, 
                LastName = lastName 
            };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return Result<Unit>.Failure(result.Errors.Select(e => e.Description).First());

            await _signInManager.SignInAsync(user, isPersistent: false);

            await _emailSender.SendAsync(email, "Регистрация", 
                $"Ваш логин: {email}\nВаш пароль: {password}");

            return Result<Unit>.Success(Unit.Value);
        }

        public async Task<Result<Unit>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<Unit>.Failure("Пользователь не найден");

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            return result.Succeeded ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Неверный логин или пароль");
        }
    }
}