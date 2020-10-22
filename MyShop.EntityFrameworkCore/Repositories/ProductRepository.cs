using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyShop.EntityFrameworkCore.Repositories
{
    public class ProductRepository : EfCoreRepository<MyShopDbContext, Product, long>, IProductRepository
    {
        public ProductRepository(IDbContextProvider<MyShopDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IQueryable<Product> GetProducts(long id) 
        {
            return DbContext.Products.Where(p => p.Id == id);
        }
    }
}
