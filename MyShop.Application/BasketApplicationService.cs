using Microsoft.AspNetCore.Authorization;
using MyShop.Application.Contract.Basket;
using MyShop.Application.Contract.Basket.Dto;
using MyShop.Application.Contract.User;
using MyShop.Application.Core.Configs;
using MyShop.Application.Core.ResponseModel;
using MyShop.Domain.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace MyShop.Application
{

    /// <summary>
    /// 购物车服务
    /// </summary>
    public class BasketApplicationService : ApplicationService, IBasketApplicationService
    {
        private readonly ConnectionMultiplexer _redis;

        private readonly IRepository<Product, long> _products;
        /// <summary>
        /// 构造
        /// </summary>
        public BasketApplicationService(ConnectionMultiplexer redis,
            IRepository<Product,long> products) 
        {
            _redis = redis;
            _products = products;
        }

        /// <summary>
        /// 获取购物篮
        /// </summary>
        /// <returns></returns>
        public async Task<ListResult<BasketItemDto>> GetAsync() 
        {
            var store = _redis.GetDatabase(MyShopRedisConfig.BASKETDBNUMBER);
            var userId = CurrentUser.Id;
            var key = $"{MyShopRedisConfig.BASKETKEY_PRE}:{userId}";

            var basket = await store.HashGetAllAsync(key);
            List<long> pids = basket.Select(x=>(long)x.Name).ToList();
            var products = _products.Where(x=>pids.Contains(x.Id));

            var list = products.ToList().Select(item => new BasketItemDto()
            {
                Count = (double)basket.FirstOrDefault(x => x.Name == item.Id).Value,
                CoverImage = item.CoverImage,
                Price = item.Price.Value,
                ProductId = item.Id,
                ProductName = item.Name,
                Status = (int)item.Status,
                StatusText = Enum.GetName(typeof(ProductStatus),item.Status)
            });

            return ListResult<BasketItemDto>.Success(list);
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="input">请求信息</param>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public async Task<BaseResult<object>> AddAsync(AddBasketItemDto input)
        {
            long pid = input.PId;
            var store = _redis.GetDatabase(MyShopRedisConfig.BASKETDBNUMBER);
            var userId = CurrentUser.Id;
            var key = $"{MyShopRedisConfig.BASKETKEY_PRE}:{userId}";

            var basket = store.HashGetAll(key);
            if (basket.Any(x => x.Name == pid))
            {
                await store.HashIncrementAsync(key, pid);
            }
            else 
            {
                var product = await _products.FindAsync(p=>p.Id == pid);
                if (product != null)
                {
                    if (product.Status != ProductStatus.Normal) 
                    {
                        return BaseResult<object>.Failed("商品已下架!");
                    }
                    await store.HashSetAsync(key, new HashEntry[] { new HashEntry(pid, 1) });
                }
                else 
                {
                    return BaseResult<object>.Failed("加入购物车失败：商品不见了!");
                }
            }

            return BaseResult<object>.Success(new { Pid = pid });
        }

        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="input">请求信息</param>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public async Task<BaseResult<object>> RemoveAsync(RemoveBasketItemDto input)
        {
            long pid = input.PId;
            var store = _redis.GetDatabase(MyShopRedisConfig.BASKETDBNUMBER);
            var userId = CurrentUser.Id;
            var key = $"{MyShopRedisConfig.BASKETKEY_PRE}:{userId}";

            var basket = store.HashGetAll(key);
            if (basket.Any(x => x.Name == pid))
            {
                await store.HashDecrementAsync(key, pid);
                double count = (double)await store.HashGetAsync(key, pid);
                if(count <= 0) 
                {
                    await store.HashDeleteAsync(key, pid);
                }
            }

            return BaseResult<object>.Success(null);
        }


        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResult<object>> ClearAsync()
        {

            var store = _redis.GetDatabase(MyShopRedisConfig.BASKETDBNUMBER);
            var userId = CurrentUser.Id;
            var key = $"{MyShopRedisConfig.BASKETKEY_PRE}:{userId}";

            await store.KeyDeleteAsync(key);

            return BaseResult<object>.Success(null);
        }
    }
}
