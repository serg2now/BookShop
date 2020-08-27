using BookShop.Api.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Api.DAL.Models
{
    [Table("Book")]
    public class Book : BaseModel
    {
        [Required]
        [Column(TypeName = "NVarchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(70)")]
        public string Author { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(30)")]
        public string Genre { get; set; }

        [Range(1, 1000)]
        [Column(TypeName = "Money")]
        public decimal Cost { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
