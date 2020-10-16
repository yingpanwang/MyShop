using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Domain.Data
{
    public interface IProductMigrator
    {
        Task MigrateAsync();
    }
}
