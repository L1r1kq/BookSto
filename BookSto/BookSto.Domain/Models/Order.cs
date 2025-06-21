namespace BookSto.Domain.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; }

    public ApplicationUser User { get; set; }
    public ICollection<Sale> Sales { get; set; }
}