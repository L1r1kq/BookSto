using System.ComponentModel.DataAnnotations;

namespace BookSto.WebAPI.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите email")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Введите имя")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Введите фамилию")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Необходимо принять условия использования")]
    public bool AcceptTerms { get; set; }
}