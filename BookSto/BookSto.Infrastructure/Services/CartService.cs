using BookSto.Application.Interfaces;
using BookSto.Domain.Models;
using BookSto.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookSto.Infrastructure.Services;

// Infrastructure/Services/CartService.cs
public class CartService : ICartService
{
    private readonly ApplicationDbContext _db;

    public CartService(ApplicationDbContext db) => _db = db;

    public async Task<int> GetCountAsync(string userId) =>
        await _db.CartItems.Where(c => c.UserId == userId)
            .SumAsync(c => c.Quantity);

    public async Task<IEnumerable<CartItem>> GetItemsAsync(string userId) =>
        await _db.CartItems
            .Include(c => c.Book)
            .Where(c => c.UserId == userId)
            .ToListAsync();

    public async Task AddAsync(string userId, int bookId, int quantity)
    {
        // Find the book from the Books table
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookId); // Change 'BookId' to 'Id'
        if (book == null)
            throw new ArgumentException("Book not found.");

        // Check if the user already has the book in their cart
        var existingItem = await _db.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);
    
        if (existingItem != null)
        {
            // If the book is already in the cart, update the quantity
            existingItem.Quantity += quantity;
        }
        else
        {
            // Add a new cart item
            _db.CartItems.Add(new CartItem
            {
                UserId = userId, // Ensure you set the UserId
                BookId = bookId,
                Quantity = quantity
                
                 // Ensure you set the ImageUrl from the Book entity
            });
        }

        // Save changes to the database
        await _db.SaveChangesAsync();
    }




    public async Task RemoveAsync(string userId, int bookId)
    {
        var item = await _db.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);
        if (item != null)
        {
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
        }
    }

    public async Task ClearAsync(string userId)
    {
        var items = _db.CartItems.Where(c => c.UserId == userId);
        _db.CartItems.RemoveRange(items);
        await _db.SaveChangesAsync();
    }
}
