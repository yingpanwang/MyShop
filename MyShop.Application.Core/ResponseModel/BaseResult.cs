using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace MyShop.Application.Core.ResponseModel
{
    /// <summary>
    /// 基础响应信息
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    public class BaseResult<T> where T:class
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public ResponseResultCode Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public virtual T Data { get; set; }

        /// <summary>
        /// 响应成功信息
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <returns></returns>
        public static BaseResult<T> Success(T data,string message = "请求成功") => new BaseResult<T>(ResponseResultCode.Success,message, data);

        /// <summary>
        /// 响应失败信息
        /// </summary>
        /// <param name="message">响应信息</param>
        /// <returns></returns>
        public static BaseResult<T> Failed(string message = "请求失败!") 
            => new BaseResult<T> (ResponseResultCode.Failed,message,null);

        /// <summary>
        /// 响应异常信息
        /// </summary>
        /// <param name="message">响应信息</param>
        /// <returns></returns>
        public static BaseResult<T> Error(string message = "请求失败!")
            => new BaseResult<T>(ResponseResultCode.Error, message, null);

        /// <summary>
        /// 构造响应信息
        /// </summary>
        /// <param name="code">响应码</param>
        /// <param name="message">响应消息</param>
        /// <param name="data">响应数据</param>
        public BaseResult(ResponseResultCode code,string message,T data) 
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }
    }

    public enum ResponseResultCode 
    {
        Success = 200,
        Failed = 400,
        Unauthorized = 401,
        Error = 500
    }
}
