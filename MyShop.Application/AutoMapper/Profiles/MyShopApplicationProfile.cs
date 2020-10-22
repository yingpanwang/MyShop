using AutoMapper;
using MyShop.Application.Contract.Order.Dto;
using MyShop.Application.Contract.Product.Dto;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyShop.Application.AutoMapper.Profiles
{
    public class MyShopApplicationProfile:Profile
    {
        public MyShopApplicationProfile() 
        {
            CreateMap<Product, ProductItemDto>().ReverseMap();
               
            CreateMap<Order, OrderInfoDto>().ReverseMap();
        }
    }
}
