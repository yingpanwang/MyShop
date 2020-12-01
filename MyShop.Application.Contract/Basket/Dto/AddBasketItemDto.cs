using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyShop.Application.Contract.Basket.Dto
{

    /// <summary>
    /// 添加购物篮项
    /// </summary>
    public class AddBasketItemDto
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public long PId { get; set; }
    }

    /// <summary>
    /// 移除购物篮项
    /// </summary>
    public class RemoveBasketItemDto
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public long PId { get; set; }

        /// <summary>
        /// 是否移除
        /// </summary>
        public bool Clear { get; set; } = false;
    }

    /// <summary>
    /// 更改购物篮项数量
    /// </summary>
    public class ChangeBasketItemDto 
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public long PId { get; set; }

        /// <summary>
        /// 变更购物篮中商品数量
        /// </summary>
        [Range(1,999)]
        public double Count { get; set; }
    }
}
