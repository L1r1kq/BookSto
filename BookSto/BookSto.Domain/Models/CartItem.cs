namespace BookSto.Domain.Models;

// Domain/Models/CartItem.cs
public class CartItem
{
    public int    Id       { get; set; }
    public string UserId   { get; set; }            // связка с ASP-NET Identity
    public int    BookId   { get; set; }
    public int    Quantity { get; set; }
    //public string ImageUrl { get; set; } // Add this property


    public virtual ApplicationUser User { get; set; }
    public virtual Book            Book { get; set; }
}
