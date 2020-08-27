using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookShop.Api.DAL.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Api.DAL.Models.Auth
{
    public class User : IdentityUser<Guid>, IModel
    {
        [Required]
        [Column(TypeName = "NVarchar(30)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(30)")]
        public string Surname { get; set; }

        [Column(TypeName = "NVarchar(30)")]
        public string MiddleName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(50)")]
        public string City { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(150)")]
        public string DeliveryAdress { get; set; }

        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
