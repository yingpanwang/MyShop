using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.EntityFrameworkCore.DbContextCreatingExtension;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MyShop.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class MyShopDbContext:AbpDbContext<MyShopDbContext>
    {
        public MyShopDbContext(DbContextOptions<MyShopDbContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureProductStore();
            builder.ConfigureOrderStore();
            builder.ConfigureOrderItemStore();
            builder.ConfigureCategoryStore();
            builder.ConfigureUserStore();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttachment> ProductAttachments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
