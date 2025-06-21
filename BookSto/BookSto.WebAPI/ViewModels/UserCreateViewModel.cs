using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookSto.WebAPI.ViewModels;

public class UserCreateViewModel
{
    [Required] public string UserName  { get; set; }
    [Required, EmailAddress] public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    // необязательные
    public string FirstName { get; set; }
    public string LastName  { get; set; }
    // Новое свойство для выбранной роли
    [Required(ErrorMessage = "Выберите роль")]
    public string SelectedRole { get; set; }
    
    public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
}