using BookSto.Domain.Models;
using BookSto.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSto.WebAPI.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Защищаем эндпоинт, если требуется
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = new DashboardData
            {
                UserCount = await _userManager.Users.CountAsync(),
                BookCount = await _dbContext.Books.CountAsync(),
                SaleCount = await _dbContext.Sales.CountAsync(),
                OrderCount = await _dbContext.Orders.CountAsync()
            };
            return Ok(data);
        }
        
        [HttpGet("invoices/open")]
        public async Task<IActionResult> GetOpenInvoices()
        {
            var invoices = await _dbContext.Orders
                .Include(o => o.User)
                .Select(o => new
                {
                    ClientName = o.User != null ? o.User.FirstName + " " + o.User.LastName : "Unknown",
                    Date = o.OrderDate.ToString("dd/MM/yyyy"),
                    InvoiceNumber = o.Id.ToString(),
                    Amount = "$" + o.Total.ToString("0.00"),
                    Status = o.Status ?? "Unknown"
                })
                .ToListAsync();

            if (!invoices.Any())
            {
                Console.WriteLine("No invoices found.");
            }
            else
            {
                foreach (var invoice in invoices)
                {
                    Console.WriteLine($"Client: {invoice.ClientName}, Date: {invoice.Date}, Amount: {invoice.Amount}, Status: {invoice.Status}");
                }
            }

            return Ok(invoices);
        }
    }
}