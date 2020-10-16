using MyShop.Domain;
using MyShop.EntityFrameworkCore.DbMigration;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyShop.Console.DbMigrator
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(MyShopDomainModule),typeof(MyShopEntityFrameworkCoreMigrationModule))]
    public class MyShopDbMigratorModule:AbpModule
    {

    }
}
