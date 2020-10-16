
using MyShop.Application.Contract.Product;
using MyShop.Application.Contract.Product.Dto;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyShop.Application
{
    public class ProductApplicationService :ApplicationService, IProductApplicationService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductApplicationService (IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductItemDto>> GetListAsync() 
        {
            var products = await _productRepository.GetListAsync();
            return ObjectMapper.Map<List<Product>, List<ProductItemDto>>(products);

        }
    }
}
