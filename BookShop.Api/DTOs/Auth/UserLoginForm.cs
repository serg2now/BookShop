using System.ComponentModel.DataAnnotations;

namespace BookShop.Api.DTOs.Auth
{
    public class UserLoginForm
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
