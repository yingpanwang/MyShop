using Microsoft.AspNetCore.Http;
using MyShop.Application.Core.ResponseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Api.Middleware
{
    /// <summary>
    /// MyShop异常中间件
    /// </summary>
    public class MyShopExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        public MyShopExceptionMiddleware(RequestDelegate next) 
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
            finally 
            {
                await HandleException(context);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex = null) 
        {

            BaseResult<object> result = null; ;

            bool handle = true;
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                result = new BaseResult<object>(ResponseResultCode.Unauthorized, "未授权!", null);
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                result = new BaseResult<object>(ResponseResultCode.Error, "服务繁忙!", null);
            }
            else 
            {
                handle = false;
            }

            if(handle) await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
        }
    }
}
