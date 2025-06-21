namespace BookSto.Domain.Models;

public class Sale
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime SaleDate { get; set; }

    public ApplicationUser User { get; set; }
    public Order Order { get; set; }
}

