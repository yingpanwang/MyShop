using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Domain.Entities
{
    public class ProductAttachment:BaseEntity
    {
        public long ProductId { get; set; }
        public ProductAttachmentType Type { get; set; }
        public string Content { get; set; }

        public ProductAttachment(long productId, ProductAttachmentType type,string content)
        {
            this.ProductId = productId;
            this.Type = type;
            this.Content = content;
        }
    }

    public enum ProductAttachmentType 
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image,

        /// <summary>
        /// 视频
        /// </summary>
        Video,

        /// <summary>
        /// 概要
        /// </summary>
        Summary
    }
}
