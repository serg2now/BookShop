using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.DTOs.Book
{
    public class OrderDto
    {
        public string OrderDate { get; set; }

        public string OrderCost { get; set; }

        public string DeliveryAdress { get; set; }

        public string BookFullName { get; set; }

        public string UserFullName { get; set; }
    }
}
