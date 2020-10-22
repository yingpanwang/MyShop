using MyShop.Admin.Application.Dto.Order;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyShop.Admin.Application.Services
{
    public class OrderApplicationService:ApplicationService
    {
        private readonly IRepository<Order, long> _orderRepository;

        public OrderApplicationService(IRepository<Order,long> orderRepository) 
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderInfoDto> GetAsync(long id) 
        {
            var order = await _orderRepository.GetAsync(id);

            return ObjectMapper.Map<Order, OrderInfoDto>(order);
        }

        public async Task<List<OrderListItemDto>> GetListAsync() 
        {
            var orders = await _orderRepository.GetListAsync();
            return ObjectMapper.Map<List<Order>, List<OrderListItemDto>>(orders);
        }

    }
}
