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

        /// <summary>
        /// 响应成功信息
        /// </summary>
        /// <param name="total">数据总条数</param>
        /// <param name="list">分页列表信息</param>
        /// <returns></returns>
        public static PagedResult<T> Success(int total,IEnumerable<T> list,string message= "请求成功") 
            => new PagedResult<T> (ResponseResultCode.Success,message,new PageData<T> (total,list));

    }

    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PageData<T> 
    {

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="total">数据总条数</param>
        /// <param name="list">数据集合</param>
        public PageData(int total,IEnumerable<T> list) 
        {
            this.Total = total;
            this.Data = list;
        }

        /// <summary>
        /// 数据总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}
