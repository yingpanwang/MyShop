using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Contract.Order.Dto
{
    public class CreateOrderDto
    {

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Total { get; set; }
    }
}
