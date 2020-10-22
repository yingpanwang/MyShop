using AutoMapper;
using MyShop.Admin.Application.Dto.Product;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Admin.Application.AutoMapper.Profiles
{
    public class MyShopAdminApplicationProfile:Profile
    {
        public MyShopAdminApplicationProfile()
        {
            CreateMap<Product, ProductItemDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();

        }
    }
}
