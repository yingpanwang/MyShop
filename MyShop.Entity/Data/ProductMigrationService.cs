using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MyShop.Domain.Data
{
    public class ProductMigrationService:ITransientDependency
    {
        private readonly ILogger<ProductMigrationService> _logger;

        private readonly IProductMigrator _migrator;

        public ProductMigrationService(IProductMigrator migrator) 
        {
            _migrator = migrator;
            _logger = NullLogger<ProductMigrationService>.Instance;
        }

        public async Task MigrateAsync() 
        {
            _logger.LogInformation("开始迁移。。。");

            try
            {
                _logger.LogInformation("迁移中。。。");
                await _migrator.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"迁移异常:{ex.Message}");
            }

            _logger.LogInformation("迁移完成。。。");
        }
    }
}
