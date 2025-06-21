using BookSto.Domain.Models;

namespace BookSto.WebAPI.ViewModels;

public class InvoiceViewModel
{
    public string            CustomerName     { get; set; }
    public string            BillingAddress   { get; set; }
    public string            ShippingAddress  { get; set; }
    public string            OrderId          { get; set; }
    public DateTime          OrderDate        { get; set; }
    public string            Status           { get; set; }

    public List<CartItem>    Items            { get; set; }
    public decimal           Subtotal         { get; set; }
    public decimal           Discount         { get; set; }
    public decimal           Tax              { get; set; }
    public decimal           DeliveryCharge   { get; set; }
    public decimal           Total            { get; set; }
}
