using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Domain.Entities
{
    public class OrderItem:BaseEntity
    {
        public Guid OrderNo { get; set; }

        public long ProductId { get; set; }

        public decimal Count { get; set; }

        public decimal Total { get; set; }
    }
}
