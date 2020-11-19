using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace MyShop.Domain.Entities
{

    /// <summary>
    /// 商品信息
    /// </summary>
    public class Product :BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverImage { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public long CategoryId { get; set; }

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

        public ProductStatus Status { get; set; }
    }

    public enum ProductStatus 
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,

        /// <summary>
        /// 下线
        /// </summary>
        Offline
    }

}
