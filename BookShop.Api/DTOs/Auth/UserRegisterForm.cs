using System;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Api.DTOs.Auth
{
    public class UserRegisterForm
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Surname { get; set; }

        [MaxLength(30)]
        public string MiddleName { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(150)]
        public string DeliveryAdress { get; set; }
    }
}
