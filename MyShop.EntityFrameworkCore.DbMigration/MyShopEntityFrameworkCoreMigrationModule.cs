using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace MyShop.EntityFrameworkCore.DbMigration
{
    
    [DependsOn(typeof(MyShopEntityFrameworkCoreModule))]
    public class MyShopEntityFrameworkCoreMigrationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DbMigrationsContext>();
        }
    }
}
