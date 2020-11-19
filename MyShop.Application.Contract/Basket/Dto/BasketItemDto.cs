using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Contract.Basket.Dto
{
    public class BasketItemDto
    {
        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public double Count { get; set; }

        public decimal Total { get => this.Price * (decimal)this.Count; }

        public int  Status { get; set; }

        public string StatusText { get; set; }

        public string CoverImage { get; set; }
    }
}
