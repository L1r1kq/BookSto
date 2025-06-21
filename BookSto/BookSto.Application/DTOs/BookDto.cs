namespace BookSto.Application.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public decimal Rating { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string PdfUrl { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateBookDto
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public decimal Rating { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string PdfUrl { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public decimal Rating { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string PdfUrl { get; set; }
        public decimal Price { get; set; }
    }
}