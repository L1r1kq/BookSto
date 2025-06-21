using BookSto.WebAPI.ViewModels;
using BookSto.Application.Interfaces;
using BookSto.Domain.Models;
using BookSto.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookSto.WebAPI.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly IEmailSender _emailSender;

        private readonly ICartService _cart;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHubContext<DashboardHub> _hubContext;

        public InvoiceController(
            ICartService cart,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            IHubContext<DashboardHub> hubContext,
            IEmailSender emailSender) // добавлено
        {
            _cart = cart;
            _userManager = userManager;
            _dbContext = dbContext;
            _hubContext = hubContext;
            _emailSender = emailSender; // присваивание
        }


        [Route("invoice")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Challenge();

            var items = (await _cart.GetItemsAsync(userId)).ToList();

            var subtotal = items.Sum(ci => ci.Book.Price * ci.Quantity);
            var discount = 20m;
            var tax = 2m; // Фиксированный налог $2.00
            var deliveryCharge = 0m;
            var total = subtotal - discount + tax + deliveryCharge;

            var vm = new InvoiceViewModel
            {
                CustomerName = $"{user.FirstName} {user.LastName}",
                BillingAddress = user.Address ?? "-",
                ShippingAddress = user.Address ?? "-",
                OrderDate = DateTime.Now,
                OrderId = DateTime.Now.Ticks.ToString(),
                Status = "Unpaid",
                Items = items,
                Subtotal = subtotal,
                Discount = discount,
                Tax = tax,
                DeliveryCharge = deliveryCharge,
                Total = total
            };

            return View("Invoice", vm);
        }

        [HttpPost]
        [Route("invoice/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(decimal Subtotal, decimal Discount, decimal Tax, decimal DeliveryCharge, decimal Total)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Challenge();

            var items = (await _cart.GetItemsAsync(userId)).ToList();
            if (!items.Any()) return RedirectToAction("Index", "Cart");

            

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Total = Total,
                Status = "Unpaid"
            };
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            var sale = new Sale
            {
                UserId = userId,
                OrderId = order.Id,
                Amount = Total,
                SaleDate = DateTime.UtcNow
            };
            _dbContext.Sales.Add(sale);

            await _cart.ClearAsync(userId);
            await _dbContext.SaveChangesAsync();
            string messageBody = $@"
    <h2>Здравствуйте, {user.FirstName}!</h2>
    <p>Благодарим за ваш заказ в магазине <strong>BookSto</strong>.</p>
    <h3>Сведения о заказе:</h3>
    <ul>
        <li><strong>Номер заказа:</strong> {order.Id}</li>
        <li><strong>Дата заказа:</strong> {order.OrderDate:dd.MM.yyyy HH:mm}</li>
        <li><strong>Сумма:</strong> ${order.Total:F2}</li>
        <li><strong>Статус:</strong> {order.Status}</li>
    </ul>
    <p>Скоро мы приступим к обработке вашего заказа. Следите за статусом в личном кабинете.</p>
    <br/>
    <p>С уважением,<br/>Команда BookSto 📚</p>
";

            await _emailSender.SendAsync(
                user.Email,
                "Подтверждение заказа в BookSto",
                messageBody
            );


            await UpdateDashboard();

            TempData["Success"] = "Заказ успешно создан";
            return RedirectToAction("Index");
        }

        
        

        private async Task UpdateDashboard()
        {
            var userCount = await _userManager.Users.CountAsync();
            var bookCount = await _dbContext.Books.CountAsync();
            var saleCount = await _dbContext.Sales.CountAsync();
            var orderCount = await _dbContext.Orders.CountAsync();

            var data = new DashboardData
            {
                UserCount = userCount,
                BookCount = bookCount,
                SaleCount = saleCount,
                OrderCount = orderCount
            };

            await _hubContext.Clients.All.SendAsync("ReceiveDashboardUpdate", data);
        }
    }
}