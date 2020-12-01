using AutoMapper;
using MyShop.Application.Contract.Order.Dto;
using MyShop.Application.Contract.Product.Dto;
using MyShop.Application.Core.Helpers;
using MyShop.Domain.Entities;
using MyShop.Users.Application.Contract.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Volo.Abp.Guids;

namespace MyShop.Application.AutoMapper.Profiles
{
    public class MyShopApplicationProfile:Profile
    {
        public MyShopApplicationProfile() 
        {
            CreateMap<Product, ProductItemDto>().ReverseMap();
            CreateMap<ProductItemDto, ProductDetailsDto>();
            CreateMap<Order, OrderInfoDto>().ReverseMap();

            
            // 用户注册信息映射
            CreateMap<UserRegisterDto, User>()
                .ForMember(src=>src.UserStatus ,opt=>opt.MapFrom(src=> UserStatusEnum.Registered))
                .ForMember(src=>src.Password , opt=>opt.MapFrom(src=> EncryptHelper.MD5Encrypt(src.Password,string.Empty)));

        }
    }
}
