using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyShop.Domain.Entities
{
    public class Basket :BaseEntity
    {
        public long UserId { get; set; }
    }
}
