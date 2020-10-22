using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyShop.Domain.IRepositories
{
    public interface IProductRepository : IRepository<Product, long>
    {
        IQueryable<Product> GetProducts(long id);
    }
}
