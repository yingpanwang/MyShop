using Microsoft.AspNetCore.Authorization;
using MyShop.Application.Contract.Basket;
using MyShop.Application.Contract.Basket.Dto;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyShop.Application
{

    /// <summary>
    /// 购物车服务
    /// </summary>
    public class BasketApplicationService : ApplicationService, IBasketApplicationService
    {
        private readonly IRepository<Basket,long> _basketRepository;
        private readonly IRepository<BasketItem,long> _itemsRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="basketRepository"></param>
        /// <param name="itemsRepository"></param>
        public BasketApplicationService(IRepository<Basket,long> basketRepository,IRepository<BasketItem,long> itemsRepository) 
        {
            _basketRepository = basketRepository;
            _itemsRepository = itemsRepository;
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="id">购物车id</param>
        /// <param name="input">物品信息</param>
        /// <returns></returns>
        public async Task<bool> AddItemAsync(long id,InsertBasketItemDto input)
        {

            if (_basketRepository.Any(x => x.Id == id)) 
            {
                var entity = ObjectMapper.Map<InsertBasketItemDto, BasketItem>(input);

                await _itemsRepository.InsertAsync(entity);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建购物车
        /// </summary>
        /// <param name="input">物品信息</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(InsertBasketItemDto input)
        {
            if (_basketRepository.Any(x => x.Id == input.BasketId))
            {
                return false;
            }
            else 
            {
                var basket = new Basket()
                {
                    CreationTime = DateTime.Now
                };

                var item = ObjectMapper.Map<InsertBasketItemDto, BasketItem>(input);
                
                var insertedBasket = await _basketRepository.InsertAsync(basket,true);
                item.BasketId = insertedBasket.Id;

                await _itemsRepository.InsertAsync(item);
                return true;
            }
            
        }

        /// <summary>
        /// 删除购物车信息
        /// </summary>
        /// <param name="id">物品信息id</param>
        /// <returns></returns>
        public Task<bool> DeleteItemAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
