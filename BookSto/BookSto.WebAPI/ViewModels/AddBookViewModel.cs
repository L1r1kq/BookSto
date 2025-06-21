using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookSto.WebAPI.ViewModels
{
    public class AddBookViewModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFile ImageFile { get; set; }  // Для изображения
        public IFormFile PdfFile { get; set; }    // Для PDF

        public decimal Price { get; set; }

        // Добавлены новые свойства для категорий и авторов
        [BindNever] public List<SelectListItem> Categories { get; set; }
        [BindNever] public List<SelectListItem> Authors    { get; set; }
    }
}