using BookSto.Domain.Models;
using BookSto.Persistence;
using BookSto.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookSto.Web.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHubContext<DashboardHub> _hubContext;
        private readonly ApplicationDbContext _dbContext;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHubContext<DashboardHub> hubContext,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var model = new List<UserViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                model.Add(new UserViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Phone = u.PhoneNumber,
                    Address = u.Address,
                    Roles = roles.ToList(),
                    LockoutEnabled = u.LockoutEnabled,
                    LockoutEnd = u.LockoutEnd
                });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new UserCreateViewModel
            {
                Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList()
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();
                return View(vm);
            }

            var user = new ApplicationUser
            {
                UserName = vm.UserName,
                Email = vm.Email,
                FirstName = vm.FirstName,
                LastName = vm.LastName
            };

            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                vm.Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();
                return View(vm);
            }

            if (!await _roleManager.RoleExistsAsync(vm.SelectedRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(vm.SelectedRole));
            }

            await _userManager.AddToRoleAsync(user, vm.SelectedRole);

            await UpdateDashboard();

            TempData["Success"] = $"Пользователь «{user.UserName}» создан с ролью «{vm.SelectedRole}»";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var vm = new UserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = userRoles,
                SelectedRoles = userRoles
            };

            ViewBag.AllRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
                return View(vm);
            }

            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user is null) return NotFound();

            user.Email = vm.Email;
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);

                ViewBag.AllRoles = _roleManager.Roles.Select(r => r.Name!).ToList();
                return View(vm);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var selected = vm.SelectedRoles ?? Array.Empty<string>();

            await _userManager.AddToRolesAsync(user, selected.Except(currentRoles));
            await _userManager.RemoveFromRolesAsync(user, currentRoles.Except(selected));

            TempData["Success"] = $"Пользователь «{user.UserName}» обновлён";
            return RedirectToAction(
                actionName: "Index",
                controllerName: "Users",
                new { area = "Admin" });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return RedirectToAction(nameof(Index));
            }

            await UpdateDashboard();

            TempData["Success"] = $"Пользователь «{user.UserName}» удалён";
            return RedirectToAction(nameof(Index));
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