using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyShop.EntityFrameworkCore.DbMigration
{
    public class DbMigrationsContextFactory : IDesignTimeDbContextFactory<DbMigrationsContext>
    {
        public DbMigrationsContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var dbOptionsBuilder = new DbContextOptionsBuilder<DbMigrationsContext>()
                .UseMySql(config.GetConnectionString("Default"));

            return new DbMigrationsContext(dbOptionsBuilder.Options);
        }
    }
}
