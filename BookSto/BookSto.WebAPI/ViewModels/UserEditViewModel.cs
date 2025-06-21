using System.ComponentModel.DataAnnotations;

namespace BookSto.WebAPI.ViewModels;

public class UserEditViewModel
{
    public string Id        { get; set; } = null!;
    public string UserName  { get; set; } = null!;
    public string Email     { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName  { get; set; }

    /* роли */
    public IList<string>  Roles         { get; set; } = new List<string>();   // текущие роли (для отображения)
    public IList<string>? SelectedRoles { get; set; }                         // роли, пришедшие из формы
}
