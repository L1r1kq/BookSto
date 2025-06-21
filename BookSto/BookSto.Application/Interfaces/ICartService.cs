using BookSto.Domain.Models;

namespace BookSto.Application.Interfaces;

// Application/Interfaces/ICartService.cs
public interface ICartService
{
    Task<int>                   GetCountAsync(string userId);
    Task<IEnumerable<CartItem>> GetItemsAsync(string userId);
    Task AddAsync(string userId, int bookId, int quantity);
    Task RemoveAsync(string userId, int bookId);
    Task ClearAsync(string userId);
    
    
}

