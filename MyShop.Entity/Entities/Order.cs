using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace MyShop.Domain.Entities
{

    /// <summary>
    /// 订单
    /// </summary>
    public class Order:BaseEntity
    {

        /// <summary>
        /// 订单号
        /// </summary>
        public Guid OrderNo { get; set; } = Guid.NewGuid();
        
        /// <summary>
        /// 用户
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

    }

    public enum OrderStatus 
    {
        Created,
        Cancel,
        Paid,
    }
}
