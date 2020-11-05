using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Domain.Entities
{
    public class BasketItem :BaseEntity
    {
        public long BasketId { get; set; }

        public long ProductId { get; set; }

        public decimal Price { get; set; }

        public long Count { get; set; }
    }
}
