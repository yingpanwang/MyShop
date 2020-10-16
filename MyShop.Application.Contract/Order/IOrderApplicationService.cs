using MyShop.Application.Contract.Order.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MyShop.Application.Contract.Order
{
    public interface IOrderApplicationService:IApplicationService
    {
        Task<OrderInfoDto> GetAsync(long id);
        Task<List<OrderInfoDto>> GetListAsync();
    }
}
