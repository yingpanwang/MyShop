using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Contract.Basket.Dto
{
    public class BasketDto
    {
        public Guid BasketId { get; set; }

        public long UserId { get; set; }

        public decimal Total { get; set; }
    }
}
