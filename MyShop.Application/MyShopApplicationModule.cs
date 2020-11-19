using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.AutoMapper.Profiles;
using MyShop.Application.Contract;
using MyShop.Application.Contract.User;
using MyShop.Domain;
using MyShop.Users.Application.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace MyShop.Application
{
    /// <summary>
    /// 项目依赖
    /// </summary>
    [DependsOn(typeof(MyShopApplicationContractModule),
        typeof(MyShopDomainModule))]

    /// 组件依赖
    [DependsOn(typeof(AbpAutoMapperModule),typeof(AbpCachingModule))]
    public class MyShopApplicationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            //添加ObjectMapper注入
            context.Services.AddAutoMapperObjectMapper<MyShopApplicationModule>();

            context.Services.AddSingleton(ConnectionMultiplexer.Connect("127.0.0.1:6379"));;


            // Abp AutoMapper设置
            Configure<AbpAutoMapperOptions>(config => 
            {
                // 添加对应依赖关系Profile
                config.AddMaps<MyShopApplicationProfile>();
            });
        }
    }
}
