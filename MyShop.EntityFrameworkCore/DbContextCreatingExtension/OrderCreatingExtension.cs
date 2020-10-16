using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyShop.EntityFrameworkCore.DbContextCreatingExtension
{
    public static class OrderCreatingExtension
    {
        public static void ConfigureOrderStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<Order>(option =>
            {
                option.ToTable("Order");
                option.ConfigureByConvention();
            });
        }
    }
}
