using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyShop.Domain.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;

namespace MyShop.Console.DbMigrator
{
    public class MyShopDbMigratorHostedService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var application = AbpApplicationFactory.Create<MyShopDbMigratorModule>(option=> 
            {
                option.UseAutofac();
                option.Services.AddLogging(c => c.AddSerilog());
            }))
            {
                application.Initialize();

                await application
                    .ServiceProvider
                    .GetRequiredService<ProductMigrationService>()
                    .MigrateAsync();

                application.Shutdown();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
