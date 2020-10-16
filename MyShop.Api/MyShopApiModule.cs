using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyShop.Application;
using MyShop.Application.Contract.Order;
using MyShop.Application.Contract.Product;
using MyShop.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyShop.Api
{

    // 注意是依赖于AspNetCoreMvc 而不是 AspNetCore
    [DependsOn(typeof(AbpAspNetCoreMvcModule),typeof(AbpAutofacModule))]
    [DependsOn(typeof(MyShopApplicationModule),typeof(MyShopEntityFrameworkCoreModule))]
    public class MyShopApiModule :AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var service = context.Services;

            service.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(ProductApplicationService).Assembly);
                options.ConventionalControllers.Create(typeof(OrderApplicationService).Assembly);
            });

            service.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MyShopApi",
                    Version = "v0.1"
                });
                options.DocInclusionPredicate((docName, predicate) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var env = context.GetEnvironment();
            var app = context.GetApplicationBuilder();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyShopApi");
            });
            app.UseRouting();
            app.UseConfiguredEndpoints();

        }
    }
}
