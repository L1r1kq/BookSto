using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BookSto.Domain.Models;
using System.Threading.Tasks;
using AutoMapper;

public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("Profile")]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User); // Получаем текущего пользователя
        if (user == null)
        {
            return RedirectToAction("Login", "Account"); // Перенаправляем на страницу входа, если пользователя нет
        }
        return View(user); // Передаем пользователя в представление
    }

    // Страница редактирования профиля
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User); // Получаем текущего пользователя
        if (user == null)
        {
            return RedirectToAction("Login", "Account"); // Перенаправляем на страницу входа, если пользователя нет
        }
        return View(user); // Передаем пользователя в представление для редактирования
    }

    [HttpPost("Profile/Edit")]
    public async Task<IActionResult> Edit(ApplicationUser model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Birthday = model.Birthday;
                user.Address = model.Address;
                user.Phone = model.Phone;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction("Index"); // Перенаправляем на страницу профиля
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update profile.";
                }
            }
        }
        return View(model); // Возвращаем обратно форму редактирования
    }

    public IActionResult ChangePassword()
    {
        throw new NotImplementedException();
    }
}
