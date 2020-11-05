using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MyShop.Application.Core.ResponseModel
{
    public class PagedResult<T> : BaseResult<PageData<T>>
    {
        public PagedResult(ResponseResultCode code, string message, PageData<T> data) : base(code, message, data)
        {
            
        }

        public static PagedResult<T> Success(int total,IEnumerable<T> list) 
            => new PagedResult<T> (ResponseResultCode.Success,"请求成功",new PageData<T> (total,list));

    }

    public class PageData<T> 
    {

        public PageData(int total,IEnumerable<T> list) 
        {
            this.Total = total;
            this.Data = list;
        }

        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
