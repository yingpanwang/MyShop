using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.AutoMapper.Profiles;
using MyShop.Application.Contract;
using MyShop.Application.Contract.User;
using MyShop.Domain;
using MyShop.Users.Application.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace MyShop.Application
{
    /// <summary>
    /// 项目依赖
    /// </summary>
    [DependsOn(typeof(MyShopApplicationContractModule),
        typeof(MyShopDomainModule))]

    /// 组件依赖
    [DependsOn(typeof(AbpAutoMapperModule),typeof(AbpHttpClientModule))]
    public class MyShopApplicationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            //添加ObjectMapper注入
            context.Services.AddAutoMapperObjectMapper<MyShopApplicationModule>();

            //创建动态客户端代理
            context.Services.AddHttpClientProxy<IUserApplicationService>();

            // Abp AutoMapper设置
            Configure<AbpAutoMapperOptions>(config => 
            {
                // 添加对应依赖关系Profile
                config.AddMaps<MyShopApplicationProfile>();
            });
        }
    }
}
