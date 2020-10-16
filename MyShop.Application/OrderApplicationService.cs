using MyShop.Application.Contract.Order;
using MyShop.Application.Contract.Order.Dto;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyShop.Application
{
    public class OrderApplicationService : ApplicationService, IOrderApplicationService
    {

        private readonly IRepository<Order> _orderRepository;

        public OrderApplicationService(IRepository<Order> orderRepository) 
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderInfoDto> GetAsync(long id)
        {
            var order = await _orderRepository.GetAsync(g => g.Id == id);

            return ObjectMapper.Map<Order, OrderInfoDto>(order);
        }

        public async Task<List<OrderInfoDto>> GetListAsync()
        {
            var orders = await _orderRepository.GetListAsync();

            return ObjectMapper.Map<List<Order>, List<OrderInfoDto>>(orders);
        }
    }
}
