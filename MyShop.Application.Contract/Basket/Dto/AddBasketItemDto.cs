using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Contract.Basket.Dto
{
    public class AddBasketItemDto
    {
        public long PId { get; set; }
    }

    public class RemoveBasketItemDto
    {
        public long PId { get; set; }
    }
}
