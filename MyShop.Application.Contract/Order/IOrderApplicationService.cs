using MyShop.Application.Contract.Base;
using MyShop.Application.Contract.Order.Dto;
using MyShop.Application.Core.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MyShop.Application.Contract.Order
{
    public interface IOrderApplicationService:IApplicationService
    {
        Task<BaseResult<OrderInfoDto>> GetAsync(long id);
        Task<PagedResult<OrderInfoDto>> GetListAsync(BasePageInput input);
    }
}
