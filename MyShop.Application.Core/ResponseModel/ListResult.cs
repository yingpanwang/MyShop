using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Core.ResponseModel
{
    /// <summary>
    /// 列表响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListResult<T> : BaseResult<IEnumerable<T>> where T : class
    {
        public ListResult(ResponseResultCode code, string message, IEnumerable<T> data) : base(code, message, data)
        {
        }

        /// <summary>
        /// 响应成功信息
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <returns></returns>
        public static ListResult<T> Success(IEnumerable<T> data, string message = "请求成功") => new ListResult<T>(ResponseResultCode.Success, message, data);

        /// <summary>
        /// 响应失败信息
        /// </summary>
        /// <param name="message">响应信息</param>
        /// <returns></returns>
        public static ListResult<T> Failed(string message = "请求失败!")
            => new ListResult<T>(ResponseResultCode.Failed, message, null);

        /// <summary>
        /// 响应异常信息
        /// </summary>
        /// <param name="message">响应信息</param>
        /// <returns></returns>
        public static ListResult<T> Error(string message = "请求失败!")
            => new ListResult<T>(ResponseResultCode.Error, message, null);

    }
}
