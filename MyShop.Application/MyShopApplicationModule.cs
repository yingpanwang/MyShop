using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.AutoMapper.Profiles;
using MyShop.Application.Contract;
using MyShop.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MyShop.Application
{
    /// <summary>
    /// 项目依赖
    /// </summary>
    [DependsOn(typeof(MyShopApplicationContractModule),
        typeof(MyShopDomainModule))]

    /// 组件依赖
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class MyShopApplicationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            //添加ObjectMapper注入
            context.Services.AddAutoMapperObjectMapper<MyShopApplicationModule>();

            // Abp AutoMapper设置
            Configure<AbpAutoMapperOptions>(config => 
            {
                // 添加对应依赖关系Profile
                config.AddMaps<MyShopApplicationProfile>();
            });
        }
    }
}
