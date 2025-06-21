using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using BookSto.Domain.Models;
using BookSto.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class DashboardHub : Hub
{
    
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _users;

    public DashboardHub(ApplicationDbContext db, UserManager<ApplicationUser> users)
    {
        _db    = db;
        _users = users;
    }
    
    public async Task SendDashboardUpdate(DashboardData data)
    {
        await Clients.All.SendAsync("ReceiveDashboardUpdate", data);
    }
    
    public override async Task OnConnectedAsync()
    {
        var data = new DashboardData
        {
            UserCount  = await _users.Users.CountAsync(),
            BookCount  = await _db.Books.CountAsync(),
            SaleCount  = await _db.Sales.CountAsync(),
            OrderCount = await _db.Orders.CountAsync()
        };
        // Отправляем только тому клиенту, который подключился
        await Clients.Caller.SendAsync("ReceiveDashboardUpdate", data);

        await base.OnConnectedAsync();
    }
}



public class DashboardData
{
    public int UserCount { get; set; }
    public int BookCount { get; set; }
    public int SaleCount { get; set; }
    public int OrderCount { get; set; }
}