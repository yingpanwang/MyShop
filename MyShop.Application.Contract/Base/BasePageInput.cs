using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MyShop.Application.Contract.Base
{
    public class BasePageInput:IPagedResultRequest
    {

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        public int SkipCount { get; set; }

        public int MaxResultCount { get; set; }
    }
}
