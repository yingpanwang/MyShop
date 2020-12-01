using MyShop.Application.Contract.Base;
using MyShop.Application.Contract.Product.Dto;
using MyShop.Application.Core.ResponseModel;
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
        PagedResult<ProductItemDto> GetPage(BasePageInput page);
        Task<BaseResult<ProductDetailsDto>> GetAsync(long id);
    }
}
