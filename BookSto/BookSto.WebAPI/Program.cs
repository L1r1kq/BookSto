using Microsoft.EntityFrameworkCore;
using SocialNetwork.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Подключение строки подключения из appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Регистрируем ApplicationDbContext с PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Добавляем контроллеры + Razor Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Конфигурация middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection(); // можешь включить позже
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Маршруты MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();