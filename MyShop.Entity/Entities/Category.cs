using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Domain.Entities
{
    public class Category:BaseEntity
    {
        public long ParentId { get; set; } = 0;
        public string CategoryName { get; set; }
    }
}
