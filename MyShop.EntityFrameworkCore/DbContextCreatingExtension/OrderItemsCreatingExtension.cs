using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyShop.EntityFrameworkCore.DbContextCreatingExtension
{
    public static class OrderItemsCreatingExtension
    {
        public static void ConfigureOrderItemStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<OrderItem>(option =>
            {
                option.ToTable("OrderItem");
                option.ConfigureByConvention();
            });
        }
    }
}
