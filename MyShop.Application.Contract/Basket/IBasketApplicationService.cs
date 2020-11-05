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
        Task<bool> CreateAsync(InsertBasketItemDto input);
        Task<bool> AddItemAsync(long id,InsertBasketItemDto input);
        Task<bool> DeleteItemAsync(long id);
    }
}
