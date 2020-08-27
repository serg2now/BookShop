using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookShop.Api.DAL.Models.Auth;
using BookShop.Api.DAL.Models.Base;

namespace BookShop.Api.DAL.Models
{
    //Order model, for simplifying purpose I assume that order can contains only 1 book
    [Table("Order")]
    public class Order : BaseModel
    {
        public DateTime OrderDate { get; set; }

        [Range(0, 1000)]
        [Column(TypeName = "Money")]
        public decimal OrderCost { get; set; }

        [Required]
        [Column(TypeName = "NVarchar(200)")]
        public string DeliveryAdress { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
