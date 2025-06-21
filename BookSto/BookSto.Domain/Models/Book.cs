using System.ComponentModel.DataAnnotations;
using SocialNetwork309.Domain.Common;
using SocialNetwork309.Domain.Common.Interfaces;

namespace BookSto.Domain.Models
{
    public class Book : BaseAuditableEntity, IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        
        public string ImageUrl { get; set; }

        [Range(0.0, 5.0)]
        public decimal Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string PdfUrl { get; set; }

        public decimal Price { get; set; }
    }

    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}