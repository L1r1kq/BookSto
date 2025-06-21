namespace BookSto.WebAPI.ViewModels;

public class UserViewModel
{
    public string           Id            { get; set; } = null!;
    public string?          Email         { get; set; }
    public string?          FirstName     { get; set; }
    public string?          LastName      { get; set; }
    public string?          Phone         { get; set; }
    public string?          Address       { get; set; }
    public List<string>     Roles         { get; set; } = new();
    public bool             LockoutEnabled{ get; set; }
    public DateTimeOffset?  LockoutEnd    { get; set; }
}