using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyShop.EntityFrameworkCore.DbContextCreatingExtension
{
    public static class BasketAndItemsCreatingExtension
    {

        public static void ConfigureBasketAndItemsStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<Basket>(option =>
            {
                option.ToTable("Basket");
                option.ConfigureByConvention();
            });
            builder.Entity<BasketItem>(option =>
            {
                option.ToTable("BasketItem");
                option.ConfigureByConvention();
            });
        }
    }
}
