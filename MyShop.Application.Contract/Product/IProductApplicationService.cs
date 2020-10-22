using MyShop.Application.Contract.Base;
using MyShop.Application.Contract.Product.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyShop.Application.Contract.Product
{
    public interface IProductApplicationService :IApplicationService
    {
        Task<List<ProductItemDto>> GetListAsync();
        IPagedResult<ProductItemDto> GetPage(BasePageInput page);
        Task<ProductItemDto> GetAsync(long id);
    }
}
