using Microsoft.AspNetCore.Http;
using MyShop.Application.Core.ResponseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
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
        }

        private async Task HandleException(HttpContext context, Exception ex = null) 
        {

            BaseResult<object> result = BaseResult<object>.Error();
            context.Response.ContentType = "application/json;utf-8";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
        }
    }
}
