using System.ComponentModel.DataAnnotations;

namespace BookShop.Api.DTOs.Book
{
    public class BookDto
    {
        public string BookId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(70)]
        public string Author { get; set; }

        [Required]
        [MaxLength(30)]
        public string Genre { get; set; }

        [Required]
        public string Cost { get; set; }
    }
}
