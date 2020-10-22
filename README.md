# 尝试从零开始构建我的商城 (一) :使用Abp vNext快速一个简单的商城项目


## 前言

### 此文目的

本文将尝试使用Abp vNext 搭建一个商城系统并尽可能从业务，架构相关方向优化升级项目结构，并会将每次升级所涉及的模块及特性以blog的方式展示出来。

### Abp vNext 介绍

#### 什么是Abp vNext

## 代码实践

> Abp vNext 使用 DDD 领域驱动设计 是非常方便的，但是由于本人认为自身没有足够的功力玩转DDD，所以开发中使用的是基于贫血模型的设计的开发，而不是遵照DDD的方式

### Domain 领域层

#### 1.添加引用

 > 通过 **Nuget** 安装 **Volo.Abp.Ddd.Domain**

#### 2.定义模块

创建 **MyShopDomainModule.cs** 作为Domain的模块，Domain不依赖任何外部模块，本体也没有什么相关配置，所以只需要继承AbpModule即可

``` csharp
namespace MyShop.Domain
{
    public class MyShopDomainModule:AbpModule
    {
    }
}
```

#### 3.定义实体

###### BaseEntity 实体基类

定义BaseEntity并继承由**Volo.Abp.Ddd.Domain**提供的Entity并添加**CreationTime**属性

``` csharp
    public class BaseEntity :Entity<long>
    {
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
```

###### Product 商品

继承**BaseEntity**类 添加相关属性
``` csharp
   /// <summary>
    /// 商品信息
    /// </summary>
    public class Product :BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal? Price { get; set; }
        
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public decimal Stock { get; set; }
    }
```
###### Order 订单

继承**BaseEntity**类 添加相关属性
``` csharp
    /// <summary>
    /// 订单
    /// </summary>
    public class Order:BaseEntity
    {

        /// <summary>
        /// 订单号
        /// </summary>
        public Guid OrderNo { get; set; } = Guid.NewGuid();
        
        /// <summary>
        /// 用户
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

    }

    public enum OrderStatus 
    {
        Created,
        Cancel,
        Paid,
    }
```

### Application 应用层实现

#### 1.添加引用

 > 通过 **Nuget** 安装 **Volo.Abp.Application**
 > 通过 **Nuget** 安装 **Volo.Abp.AutoMapper**

#### 2.定义模块

创建 **MyShopApplicationModule.cs** ,继承自AbpModule，并且依赖Domain以及ApplicationContract层和后续使用的AutoMapper，所以对应DependsOn中需要添加对应依赖

``` csharp
    /// <summary>
    /// 项目模块依赖
    /// </summary>
    [DependsOn(typeof(MyShopApplicationContractModule),
        typeof(MyShopDomainModule))]

    /// 组件依赖
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class MyShopApplicationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            //添加ObjectMapper注入
            context.Services.AddAutoMapperObjectMapper<MyShopApplicationModule>();

            // Abp AutoMapper设置
            Configure<AbpAutoMapperOptions>(config => 
            {
                // 添加对应依赖关系Profile
                config.AddMaps<MyShopApplicationProfile>();
            });
        }
    }
```

#### 3.实现相关服务

这里由于我们使用AutoMapper所以需要创建Profile并添加相关映射关系

##### Profile定义

``` csharp

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

```

##### 服务实现

Abp 可以很方便的将我们的服务向外暴露，通过简单配置可以自动生成对应接口,只需要需要暴露的服务
继承ApplicationService。

>   **Abp**在确定服务方法的**HTTP Method**时使用命名约定:
    **Get**: 如果方法名称以**GetList**,**GetAll**或**Get**开头.
    **Put**: 如果方法名称以**Put**或**Update**开头.
    **Delete**: 如果方法名称以**Delete**或**Remove**开头.
    **Post**: 如果方法名称以**Create**,**Add**,**Insert**或**Post**开头.
    **Patch**: 如果方法名称以**Patch**开头.
    **其他情况**, **Post** 为 默认方式.

###### OrderApplicationService

``` csharp
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
```
###### ProductApplicationService

``` csharp
 /// <summary>
    /// 商品服务
    /// </summary>
    public class ProductApplicationService : ApplicationService, IProductApplicationService
    {
        /// <summary>
        /// 自定义商品仓储
        /// </summary>
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// 商品类别仓储
        /// </summary>
        private readonly IRepository<Category> _categoryRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="productRepository">自定义商品仓储</param>
        /// <param name="categoryRepository">商品类别仓储</param>
        public ProductApplicationService(IProductRepository productRepository,
            IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        public async Task<ProductItemDto> GetAsync(long id)
        {
            return await Task.FromResult(Query(p => p.Id == id).FirstOrDefault(p =>p.Id == id));
        }

        /// <summary>
        /// 获取分页商品列表
        /// </summary>
        /// <param name="page">分页信息</param>
        /// <returns></returns>
        public IPagedResult<ProductItemDto> GetPage(BasePageInput page)
        {
            var query = Query()
                .WhereIf(!string.IsNullOrEmpty(page.Keyword), w => w.Name.StartsWith(page.Keyword));
               
            var count =  query.Count();

            var products = query.PageBy(page).ToList();

            return new PagedResultDto<ProductItemDto>(count,products);
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductItemDto>> GetListAsync()
        {
            var products = await _productRepository.GetListAsync();
            return ObjectMapper.Map<List<Product>, List<ProductItemDto>>(products);
        }


        private IQueryable<ProductItemDto> Query(Expression<Func<Product,bool>> expression = null) 
        {
            var products = _productRepository.WhereIf(expression != null, expression);

            var categories = _categoryRepository;

            var data = from product in products
                       join category in categories on product.CategoryId equals category.Id into temp
                       from result in temp.DefaultIfEmpty()
                       select new ProductItemDto
                       {
                           Id = product.Id,
                           Description = product.Description,
                           Name = product.Name,
                           Price = product.Price,
                           Stock = product.Stock,
                           CategoryName = result.CategoryName,
                       };

            return data;
        }
    }
```

### Application.Contract 应用层抽象

#### 1.添加引用

 > 通过 **Nuget** 安装 **Volo.Abp.Application.Contract**

#### 2.定义模块

创建 **MyShopApplicationContractModule.cs** ,继承自AbpModule，并且没有对外依赖

``` csharp
   public class MyShopApplicationContractModule:AbpModule
    {

    }
```
#### 3.定义Contract

###### IOrderApplicationService

``` csharp
    public interface IOrderApplicationService:IApplicationService
    {
        Task<OrderInfoDto> GetAsync(long id);
        Task<List<OrderInfoDto>> GetListAsync();
    }
```

###### IProductApplicationService

``` csharp
    public interface IProductApplicationService :IApplicationService
    {
        Task<List<ProductItemDto>> GetListAsync();
        IPagedResult<ProductItemDto> GetPage(BasePageInput page);
        Task<ProductItemDto> GetAsync(long id);
    }
```

### Admin.Application 管理后台应用层

这里和Application层差不多，但是为了方便编写代码所以Admin.Contract和Admin.Application 放在了一个项目中，这里我定义了**IBaseAdminCRUDApplicationService**接口并且实现了一个**BaseAdminCRUDApplicationService**实现了一些简单的CRUD的方法暂时作为后台数据管理的方法,并且为了区分后台管理接口和平台接口在自动注册为api时加上**admin**前缀
``` csharp

            service.Configure((AbpAspNetCoreMvcOptions options) =>
            {
                // 平台接口和后台管理接口暂时放在一个站点下
                options.ConventionalControllers.Create(typeof(Application.ProductApplicationService).Assembly);
                options.ConventionalControllers.Create(typeof(Application.OrderApplicationService).Assembly);

                // 区分接口 请求路径加上admin
                options.ConventionalControllers.Create(typeof(Admin.Application.Services.ProductApplicationService).Assembly, options =>
                 {
                     options.RootPath = "admin";
                 });
            });

```
#### 1.添加引用

 > 通过 **Nuget** 安装 **Volo.Abp.Application**
 > 通过 **Nuget** 安装 **Volo.Abp.AutoMapper**

#### 2.定义模块

创建 **MyShopAdminApplicationModule.cs** ,继承自AbpModule，并且依赖Domain以及ApplicationContract层和后续使用的AutoMapper，所以对应DependsOn中需要添加对应依赖

``` csharp
    [DependsOn(typeof(MyShopDomainModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class MyShopAdminApplicationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<MyShopAdminApplicationModule>();

            Configure<AbpAutoMapperOptions>(config =>
            {
                config.AddMaps<MyShopAdminApplicationProfile>();
            });
        }
    }
```

### EntityFreaworkCore数据访问层中实现Repository
> 通过Nuget添加 **Volo.Abp.EntityFrameworkCore.MySQL**
> 
###### 定义DbContext

``` csharp

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
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }
    }

```
这里通过扩展方法分开了实体的一些定义


###### ConfigureProductStore

``` csharp
    public static class ProductCreatingExtension
    {
        public static void ConfigureProductStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<Product>(option =>
            {
                option.ToTable("Product");
                option.ConfigureByConvention();
            });
        }
    }
```

###### ConfigureOrderStore

``` csharp
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
```

###### ConfigureOrderItemStore

``` csharp
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
```

###### ConfigureCategoryStore

``` csharp
    public static class CategoryCreatingExtension
    {

        public static void ConfigureCategoryStore(this ModelBuilder builder) 
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<Category>(option =>
            {
                option.ToTable("Category");
                option.ConfigureByConvention();
            });
        }
    }
```


由于目前Abp vNext提供的 IRepository<TEntity,TKey> 接口已经满足我们当前的使用需要，也提供了IQuaryable 接口，所以灵活组装的能力也有，所以不另外实现自定义仓储，不过这里还是预先准备一个自定义的ProductRepsitory

###### 在Domain层中定义IProductRepository

``` csharp

    public interface IProductRepository : IRepository<Product, long>
    {
        IQueryable<Product> GetProducts(long id);
    }

```

###### EntityFrameworkCore数据访问层中实现IProductRepository
 
 继承EfCoreRepository以方便实现 IRepository接口中提供的基本方法和 DbContext的访问，并实现IProductRepository
 ``` csharp

    public class ProductRepository : EfCoreRepository<MyShopDbContext, Product, long>, IProductRepository
    {
        public ProductRepository(IDbContextProvider<MyShopDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IQueryable<Product> GetProducts(long id) 
        {
            return DbContext.Products.Where(p => p.Id == id);
        }
    }

 ```

#### 定义模块，注入默认仓储
``` csharp

    [DependsOn(typeof(AbpEntityFrameworkCoreMySQLModule))]
    public class MyShopEntityFrameworkCoreModule :AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyShopDbContext>(option=> 
            {

                // 如果只是用了 entity， 请将 includeAllEntities 设置为 true 否则会提示异常，导致所属仓储无法注入
                option.AddDefaultRepositories(true);
            });

            Configure<AbpDbContextOptions>(option=> 
            {
                option.UseMySQL();
            });
            
        }
    }

```


### Migration 迁移

#### 1.添加引用
######

> 通过Nuget安装 **Microsoft.EntityFrameworkCore.Tools**
###### 添加Domain，EntiyFrameworkCore数据访问层的引用

``` csharp
 <ItemGroup>
    <ProjectReference Include="..\MyShop.EntityFrameworkCore\MyShop.EntityFrameworkCore.csproj" />
    <ProjectReference Include="..\MyShop.Entity\MyShop.Domain.csproj" />
  </ItemGroup>
```

#### 2.定义DbContext

这里我们使用了之前在EntifyFrameworkCore中定义的一些实体配置


``` csharp

    [ConnectionStringName("Default")]
    public class DbMigrationsContext : AbpDbContext<DbMigrationsContext>
    {
        public DbMigrationsContext(DbContextOptions<DbMigrationsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureProductStore();
            builder.ConfigureOrderStore();
            builder.ConfigureOrderItemStore();
            builder.ConfigureCategoryStore();
        }

    }

```

#### 3.定义模块

``` csharp 

    [DependsOn(typeof(MyShopEntityFrameworkCoreModule))]
    public class MyShopEntityFrameworkCoreMigrationModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DbMigrationsContext>();
        }
    }

```

#### 4.执行迁移

打开程序包管理器控制台，由于我们的连接字符串定义在Api层，我已我们需要将Api设置为启动项目，并将程序包管理器控制台默认程序切换为我们的迁移层Migration并执行一下指令

1.添加迁移文件

> Add-Migration "Init"

执行完后我们会在Migrations文件夹目录下得到相关的迁移文件

2.执行迁移

>Update-Database

执行完成后就可以打开数据库查看我们生成的表了
 
### Api 接口层

#### 1.建立项目

接下来建立AspNetCore WebAPI 项目 并命名为MyShop.Api作为我们对外暴露的API，并添加Abp Vnext AspNetCore.MVC包

> 通过Nuget 安装 Volo.Abp.AspNetCore.Mvc

###### StartUp

``` csharp

namespace MyShop.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 注册模块
            services.AddApplication<MyShopApiModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 初始化
            app.InitializeApplication();
        }
    }
}

```

###### Program


由于我们使用了 Abp vNext 中的Autofac 所以需要添加 Abp vNext Autofac 模块 并注册Autofac

> 通过Nuget 安装 Volo.Abp.Autofac

``` csharp 

namespace MyShop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac();
    }
}

```

###### MyShopApiModule 

这里我们使用了Swagger 作为Api文档自动生成，所以需要添加相关Nuget包，并且依赖了MyShopEntityFrameworkCoreModule和 MyShopApplicationModule,MyShopAdminApplicationModule 所以需要添加相关依赖及项目引用,
并且使用了Api作为迁移连接字符串所在项目，所以还需要添加EF的Design包

###### Nuget
> 通过Nuget 安装 Swashbuckle.AspNetCore
> 通过Nuget 安装 Microsoft.EntityFrameworkCore.Design

###### 项目引用

> MyShop.AdminApplication
> MyShop.Application
> MyShop.EntitFrameworkCore
> MyShop.EntitFrameworkCore.DbMigration


``` csharp

namespace MyShop.Api
{

    // 注意是依赖于AspNetCoreMvc 而不是 AspNetCore
    [DependsOn(typeof(AbpAspNetCoreMvcModule),typeof(AbpAutofacModule))]
    [DependsOn(typeof(MyShopApplicationModule),typeof(MyShopEntityFrameworkCoreModule),typeof(MyShopAdminApplicationModule))]
    public class MyShopApiModule :AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var service = context.Services;

            // 跨域
            context.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // 自动生成控制器
            service.Configure((AbpAspNetCoreMvcOptions options) =>
            {
                options.ConventionalControllers.Create(typeof(Application.ProductApplicationService).Assembly);
                options.ConventionalControllers.Create(typeof(Application.OrderApplicationService).Assembly);
                options.ConventionalControllers.Create(typeof(Admin.Application.Services.ProductApplicationService).Assembly, options =>
                 {
                     options.RootPath = "admin";
                 });
            });

            // swagger
            service.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MyShopApi",
                    Version = "v0.1"
                });
                options.DocInclusionPredicate((docName, predicate) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var env = context.GetEnvironment();
            var app = context.GetApplicationBuilder();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyShopApi");
            });
            app.UseRouting();
            app.UseConfiguredEndpoints();
        }
    }
}

```

至此我们的简单的项目就搭建好了





