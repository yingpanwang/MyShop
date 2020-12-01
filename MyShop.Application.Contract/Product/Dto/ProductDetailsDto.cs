using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Contract.Product.Dto
{
    public class ProductDetailsDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverImage { get; set; }

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

        /// <summary>
        /// 图片
        /// </summary>
        public List<string> Images { get; set; }
        
        /// <summary>
        /// 说明
        /// </summary>
        public string Summary { get; set; }
    }
}
