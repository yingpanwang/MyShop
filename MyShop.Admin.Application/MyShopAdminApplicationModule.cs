using Microsoft.Extensions.DependencyInjection;
using MyShop.Admin.Application.AutoMapper.Profiles;
using MyShop.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MyShop.Admin.Application
{
    [DependsOn(typeof(MyShopDomainModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class MyShopAdminApplicationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<MyShopAdminApplicationModule>();

            Configure<AbpAutoMapperOptions>(config =>
            {
                config.AddMaps<MyShopAdminApplicationProfile>();
            });
        }
    }
}
