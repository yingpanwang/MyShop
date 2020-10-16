using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Contract.Product.Dto
{
    public class ProductItemDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public decimal Stock { get; set; }
    }
}
