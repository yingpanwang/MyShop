
using Microsoft.AspNetCore.Authorization;
using MyShop.Application.Contract.Base;
using MyShop.Application.Contract.Product;
using MyShop.Application.Contract.Product.Dto;
using MyShop.Application.Core.ResponseModel;
using MyShop.Domain.Entities;
using MyShop.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyShop.Application
{
    /// <summary>
    /// 商品服务
    /// </summary>
    public class ProductApplicationService : ApplicationService, IProductApplicationService
    {
        /// <summary>
        /// 自定义商品仓储
        /// </summary>
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// 商品类别仓储
        /// </summary>
        private readonly IRepository<Category> _categoryRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="productRepository">自定义商品仓储</param>
        /// <param name="categoryRepository">商品类别仓储</param>
        public ProductApplicationService(IProductRepository productRepository,
            IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        public async Task<ProductItemDto> GetAsync(long id)
        {
            return await Task.FromResult(Query(p => p.Id == id).FirstOrDefault(p =>p.Id == id));
        }

        /// <summary>
        /// 获取分页商品列表
        /// </summary>
        /// <param name="page">分页信息</param>
        /// <returns></returns>
        public PagedResult<ProductItemDto> GetPage(BasePageInput page)
        {
            var query = Query()
                .WhereIf(!string.IsNullOrEmpty(page.Keyword), w => w.Name.StartsWith(page.Keyword));
               
            var count =  query.Count();

            var products = query.PageBy(page).ToList();

            return PagedResult<ProductItemDto>.Success(count,products);
        }

        private IQueryable<ProductItemDto> Query(Expression<Func<Product,bool>> expression = null) 
        {
            var products = _productRepository.WhereIf(expression != null, expression);

            var categories = _categoryRepository;

            var data = from product in products
                       join category in categories on product.CategoryId equals category.Id into temp
                       from result in temp.DefaultIfEmpty()
                       select new ProductItemDto
                       {
                           Id = product.Id,
                           Description = product.Description,
                           Name = product.Name,
                           Price = product.Price,
                           Stock = product.Stock,
                           CategoryName = result.CategoryName,
                           CoverImage = product.CoverImage
                       };

            return data;
        }
    }
}
