using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookShop.Api.DAL.Models.Base;

namespace BookShop.Api.DAL.Models.Auth
{
    //Refresh token model, base IdentityToken class contains unnecessary properties, so I've decided to write my own 
    [Table("RefreshToken")]
    public class RefreshToken : BaseModel
    {
        [Required]
        public string TokenValue { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(50)")]
        public string DeviceName { get; set; }
    }
}
