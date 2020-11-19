
using MyShop.Application.Contract.Basket.Dto;
using MyShop.Application.Core.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace MyShop.Application.Contract.Basket
{
    /// <summary>
    /// 购物篮服务
    /// </summary>
    public interface IBasketApplicationService:IApplicationService
    {
        /// <summary>
        /// 获取购物篮
        /// </summary>
        /// <returns></returns>
        Task<ListResult<BasketItemDto>> GetAsync();

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        Task<BaseResult<object>> AddAsync(AddBasketItemDto input);

        /// <summary>
        /// 移除商品
        /// </summary>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        Task<BaseResult<object>> RemoveAsync(RemoveBasketItemDto input);

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <returns></returns>
        Task<BaseResult<object>> ClearAsync();

    }
}
