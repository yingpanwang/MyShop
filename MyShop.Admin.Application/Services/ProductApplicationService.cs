using MyShop.Admin.Application.Dto.Product;
using MyShop.Admin.Application.Services.Base;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Volo.Abp.Domain.Repositories;

namespace MyShop.Admin.Application.Services
{

    public class ProductApplicationService : BaseAdminCRUDApplicationService<Product, CreateProductDto, ProductItemDto>
    {
        public ProductApplicationService(IRepository<Product,long> entityRepository) : base(entityRepository)
        {
        }
    }
}
