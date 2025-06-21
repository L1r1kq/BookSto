using BookSto.Application.DTOs;
using System.Threading.Tasks;
using BookSto.Application.Common;
using MediatR;

namespace BookSto.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<Unit>> RegisterAsync(string email, string password, string confirmPassword, string firstName, string lastName, bool acceptTerms);
        Task<Result<Unit>> LoginAsync(string email, string password);
    }
}
