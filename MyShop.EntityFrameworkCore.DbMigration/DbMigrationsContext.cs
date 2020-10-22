using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.EntityFrameworkCore.DbContextCreatingExtension;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MyShop.EntityFrameworkCore.DbMigration
{
    [ConnectionStringName("Default")]
    public class DbMigrationsContext : AbpDbContext<DbMigrationsContext>
    {
        public DbMigrationsContext(DbContextOptions<DbMigrationsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureProductStore();
            builder.ConfigureOrderStore();
            builder.ConfigureOrderItemStore();
            builder.ConfigureCategoryStore();
        }

    }
}
