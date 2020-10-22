using MyShop.Application.Contract.Basket.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace MyShop.Application.Contract.Basket
{
    public interface IBasketApplicationService:IApplicationService
    {
        Task<bool> InsertAsync(InsertBasketItemDto input);
        Task<bool> DeleteAsync(long id);
        Task<bool> RemoveAsync(long id);
    }
}
