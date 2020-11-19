using Microsoft.AspNetCore.Authorization;
using MyShop.Application.Contract.Base;
using MyShop.Application.Contract.Order;
using MyShop.Application.Contract.Order.Dto;
using MyShop.Application.Contract.User;
using MyShop.Application.Core.ResponseModel;
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
    /// 订单服务
    /// </summary>
    [Authorize]
    public class OrderApplicationService : ApplicationService, IOrderApplicationService
    {

        private readonly IRepository<Order,long> _orderRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="orderRepository"></param>
        public OrderApplicationService(IRepository<Order,long> orderRepository) 
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public async Task<BaseResult<OrderInfoDto>> GetAsync(long id)
        {
            var order = await _orderRepository.GetAsync(g => g.Id == id);
            
            return BaseResult<OrderInfoDto>.Success(ObjectMapper.Map<Order, OrderInfoDto>(order));
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <returns></returns>
        public async Task<Core.ResponseModel.PagedResult<OrderInfoDto>> GetListAsync(BasePageInput input)
        {
            var user = CurrentUser;

            var orders = _orderRepository
                .Where(p => p.UserId == user.Id);
            var result = ObjectMapper.Map<List<Order>,List<OrderInfoDto>>(orders.PageBy(input).ToList());
            
            return Core.ResponseModel.PagedResult<OrderInfoDto>.Success(orders.Count(), result);
        }
    }
}
