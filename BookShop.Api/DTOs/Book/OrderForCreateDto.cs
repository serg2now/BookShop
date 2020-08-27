using System.ComponentModel.DataAnnotations;

namespace BookShop.Api.DTOs.Book
{
    public class OrderForCreateDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string BookId { get; set; }
    }
}
