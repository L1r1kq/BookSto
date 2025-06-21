using System.Globalization;
using BookSto.Application.Extensions;
using BookSto.Application.Features.Auth.Commands;
using BookSto.Application.Interfaces;
using BookSto.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookSto.Application.Interfaces.Services;
using BookSto.Infrastructur.Services;
using BookSto.Domain.Models;
using BookSto.Infrastructure.Extensions;
using BookSto.Infrastructure.Services;
using Microsoft.AspNetCore.Localization;


var builder = WebApplication.CreateBuilder(args);



// ─────────────────────  DB и инфраструктура  ─────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(connectionString));

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();
builder.Services.AddTransient<IEmailSender, EmailSender>();


builder.Services.AddSignalR();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();



// ─────────────────────  Session  ─────────────────────
builder.Services.AddDistributedMemoryCache();   // ✨ кэш для Session
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout        = TimeSpan.FromMinutes(30);
    opt.Cookie.HttpOnly    = true;
    opt.Cookie.IsEssential = true;
});

// ─────────────────────  Identity  ─────────────────────
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    opt.SignIn.RequireConfirmedAccount = false;
    opt.Password.RequireDigit          = true;
    opt.Password.RequiredLength        = 6;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase      = false;
    opt.Password.RequireLowercase      = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ─────────────────────  Cookie  ─────────────────────
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath        = "/Account/Login";
    opt.LogoutPath       = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied";
});

// ─────────────────────  DI-сервисы  ─────────────────────
builder.Services.AddScoped<IAuthService,   AuthService>();
builder.Services.AddScoped<IBookService,   BookService>();
builder.Services.AddScoped<BookSto.Application.Interfaces.IEmailSender,
                            BookSto.Infrastructure.Services.EmailSender>();
builder.Services.AddScoped<ICartService, CartService>();


// Cart-фильтр
builder.Services.AddHttpContextAccessor();

// ─────────────────────  MediatR  ─────────────────────
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateBookCommand>();
    cfg.RegisterServicesFromAssembly(typeof(BookSto.Application.Common.Result<>).Assembly);
});

// ─────────────────────  MVC / Razor  ─────────────────────
builder.Services.AddControllersWithViews();    // <-- Регистрируем MVC
builder.Services.AddAuthorization(); 

// ДОБАВИТЬ СРАЗУ ПОСЛЕ builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
});


var app = builder.Build();

var supportedCultures = new[] { "en", "ru" };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
});


// ─────────────────────  Middleware  ─────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

   
app.UseStaticFiles();
app.UseRouting();


app.UseSession(); 


app.UseAuthentication();
app.UseAuthorization();

// ─────────────────────  Роутинг  ─────────────────────
app.MapControllerRoute(
    name: "admin",
    pattern: "admin",
    defaults: new { area = "Admin", controller = "Dashboard", action = "Index" });

app.MapHub<DashboardHub>("/hubs/dashboard");

app.MapControllerRoute(
    name: "bookDetails",
    pattern: "Admin/Books/Details/{id}",
    defaults: new { area = "Admin", controller = "Books", action = "Details" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "home/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "account",
    pattern: "Account/{action}",
    defaults: new { controller = "Account", action = "Login" });

app.MapControllerRoute(
    name: "profile",
    pattern: "Profile",
    defaults: new { controller = "Profile", action = "Index" });

app.MapControllerRoute(
    name: "profile",
    pattern: "Profile/{action=Index}",
    defaults: new { controller = "Profile" });

app.MapControllerRoute(
    name: "admin-users",
    pattern: "admin/users/{action=Index}/{id?}",
    defaults: new { area = "Admin", controller = "Users" });






app.Run();
