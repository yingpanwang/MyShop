using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Contract.Order.Dto
{
    public class OrderInfoDto
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public Guid OrderNo { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Total { get; set; }

    }
}
