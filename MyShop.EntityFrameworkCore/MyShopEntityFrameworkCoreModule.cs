using Microsoft.Extensions.DependencyInjection;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace MyShop.EntityFrameworkCore
{
    [DependsOn(typeof(AbpEntityFrameworkCoreMySQLModule))]
    public class MyShopEntityFrameworkCoreModule :AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyShopDbContext>(option=> 
            {

                // 如果只是用了 entity， 请将 includeAllEntities 设置为 true 否则会提示异常，导致所属仓储无法注入
                option.AddDefaultRepositories(true);
            });

            Configure<AbpDbContextOptions>(option=> 
            {
                option.UseMySQL();
            });
            
        }
    }
}
