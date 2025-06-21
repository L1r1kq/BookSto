using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookSto.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Новые необязательные поля
        public string? Birthday { get; set; }
        public string? Address  { get; set; }
        public string? Phone    { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        [NotMapped] public IList<string>? AppRoles { get; set; }

    }

}