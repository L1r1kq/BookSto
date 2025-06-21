using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookSto.Application.DTOs;

public class CreateBookRequestDto
{
    [Required, StringLength(200)] public string Title       { get; init; }
    [Required]                     public int    AuthorId    { get; init; }
    [Required]                     public int    CategoryId  { get; init; }
    [Range(0, 5)]                  public decimal Rating     { get; init; }
    [Required]                     public string Description { get; init; }
    [Required]                     public decimal Price      { get; init; }

    [Url, Required(ErrorMessage = "Укажите ссылку на изображение")]
    public string ImageUrl { get; init; }

    [Url, Required(ErrorMessage = "Укажите ссылку на PDF")]
    public string PdfUrl { get; init; }
}
