using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Api.Middleware
{
    public static class MiddlewareExtensions
    {

        /// <summary>
        /// MyShop异常中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMyShopExceptionMiddleware(this IApplicationBuilder app) 
        {
            app.UseMiddleware<MyShopExceptionMiddleware>();
            return app;
        }
    }
}
