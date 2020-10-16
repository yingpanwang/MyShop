using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Domain.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MyShop.EntityFrameworkCore.DbMigration
{


    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IProductMigrator))]
    public class MyShopDbMigrator : IProductMigrator
    {

        private readonly DbMigrationsContext _dbContext;
        public MyShopDbMigrator(DbMigrationsContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}
