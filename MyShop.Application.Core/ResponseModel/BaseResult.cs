using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace MyShop.Application.Core.ResponseModel
{
    public class BaseResult<T> where T:class
    {

        public ResponseResultCode Code { get; set; }
        public string Message { get; set; }
        public virtual T Data { get; set; }

        public static BaseResult<T> Success(T data) => new BaseResult<T>(ResponseResultCode.Success, "请求成功", data);

        public static BaseResult<T> Failed(string message = "请求失败!") 
            => new BaseResult<T> (ResponseResultCode.Failed,message,null);

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
