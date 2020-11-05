
# 尝试从零开始构建我的商城 (二) :使用JWT保护我们的信息安全，完善Swagger配置
[toc]
## 前言

### GitHub地址

> https://github.com/yingpanwang/MyShop/tree/dev_jwt
> 此文对应分支 dev_jwt
### 此文目的

&nbsp;上一篇文章中，我们使用Abp vNext构建了一个可以运行的简单的API,但是整个站点没有一个途径去对我们的API访问有限制，导致API完全是裸露在外的，如果要运行正常的商业API是完全不可行的，所以接下来我们会通过使用JWT(Json Web Toekn)的方式实现对API的访问数据限制。

#### JWT简介

###### 什么是JWT

现在API一般是分布式且要求是无状态的，所以传统的Session无法使用，JWT其实类似于早期API基于Cookie自定义用户认证的形式，只是JWT的设计更为紧凑和易于扩展开放，使用方式更加便利基于JWT的鉴权机制类似于http协议也是无状态的，它不需要在服务端去保留用户的认证信息或者会话信息。

###### JWT的组成

JWT （以下简称Token）的组成分为三部分 header，playload，signature 完整的Token长这样

    eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoid2FuZ3lpbmdwYW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9tb2JpbGVwaG9uZSI6IjEyMiIsImV4cCI6MTYwNDI4MzczMSwiaXNzIjoiTXlTaG9wSXNzdWVyIiwiYXVkIjoiTXlTaG9wQXVkaWVuY2UifQ.U-2bEniEz82ECibBzk6C5tuj2JAdqISpbs5VrpA8W9s

**header**

包含类型及算法信息

``` JSON
{
  "alg": "HS256",
  "typ": "JWT"
}
```
通过base64加密后得到了 

    eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9

**playload**
包含标准的公开的生命和私有的声明,不建议定义敏感信息，因为该信息是可以被解密的

部分公开的声明

* iss: jwt签发者
* aud: 接收jwt的一方
* exp: jwt的过期时间，这个过期时间必须要大于签发时间

私有的声明

*  http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name : 名称
*  http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier: 标识
*  http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone: 电话
``` JSON
{
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "wangyingpan",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": "2",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone": "122",
  "exp": 1604283731,
  "iss": "MyShopIssuer",
  "aud": "MyShopAudience"
}
```
通过base64加密后得到了 

    eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoid2FuZ3lpbmdwYW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9tb2JpbGVwaG9uZSI6IjEyMiIsImV4cCI6MTYwNDI4MzczMSwiaXNzIjoiTXlTaG9wSXNzdWVyIiwiYXVkIjoiTXlTaG9wQXVkaWVuY2UifQ

**signature**

signature 由 三部分信息组成，分别为base64加密后的**header**，**playload**用"."连接起来，通过声明的算法加密使用 服务端**secret** 作为盐(**salt**)

三部分通过“.”连接起三部分组成了最终的Token

## 代码实践

### 添加用户服务

##### 1.Domain中定义用户实体User

``` csharp

 public class User:BaseGuidEntity
    {
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatusEnum UserStatus { get; set; }
    }
    public enum UserStatusEnum 
    {
        Registered,//已注册
        Incompleted, // 未完善信息
        Completed,//完善信息
        Locked, // 锁定
        Deleted // 删除
    }

```

##### 2.EntityFrameworkCore 中添加Table

**UserCreatingExtension.cs**
``` csharp

public static class UserCreatingExtension
    {
        public static void ConfigureUserStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            builder.Entity<User>(option =>
            {
                option.ToTable("User");
                option.ConfigureByConvention();
            });
        }
    }

```

**MyShopDbContext.cs**

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
            builder.ConfigureBasketAndItemsStore();

            // 配置用户表
            builder.ConfigureUserStore();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Basket> Basket { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        //添加用户表
        public DbSet<User> Users { get; set; }
    }

```

##### 3.迁移生成User表

首先添加用户表定义

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
            builder.ConfigureBasketAndItemsStore();

            // 用户配置
            builder.ConfigureUserStore();
        }

    }

```

然后打开程序包管理控制台切换为迁移项目**MyShop.EntityFrameworkCore.DbMigration**并输入

* >Add-Migration "AddUser"

* >Update-Database

此时User表就已经生成并对应Migration文件


##### 4.Application中构建UserApplication

###### Api中添加配置AppSetting.json

``` csharp

"Jwt": {
    "SecurityKey": "1111111111111111111111111111111",
    "Issuer": "MyShopIssuer", 
    "Audience": "MyShopAudience",
    "Expires": 30 // 过期分钟
  }

```

###### IUserApplicationService

``` csharp

namespace MyShop.Application.Contract.User
{
    public interface IUserApplicationService
    {
        Task<BaseResult<TokenInfo>> Register(UserRegisterDto registerInfo, CancellationToken cancellationToken);
        Task<BaseResult<TokenInfo>> Login(UserLoginDto loginInfo);
    }
}

```

###### AutoMapper Profiles中添加映射

``` csharp

namespace MyShop.Application.AutoMapper.Profiles
{
    public class MyShopApplicationProfile:Profile
    {
        public MyShopApplicationProfile() 
        {
            CreateMap<Product, ProductItemDto>().ReverseMap();
               
            CreateMap<Order, OrderInfoDto>().ReverseMap();

            CreateMap<Basket, BasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

            CreateMap<InsertBasketItemDto, BasketItem>().ReverseMap();

            // 用户注册信息映射
            CreateMap<UserRegisterDto, User>()
                .ForMember(src=>src.UserStatus ,opt=>opt.MapFrom(src=> UserStatusEnum.Registered))
                .ForMember(src=>src.Password , opt=>opt.MapFrom(src=> EncryptHelper.MD5Encrypt(src.Password,string.Empty)));

        }
    }
}

```

###### UserApplicationService

``` csharp

namespace MyShop.Application
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserApplicationService : ApplicationService, IUserApplicationService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<User,Guid> _userRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userRepository">用户仓储</param>
        /// <param name="configuration">配置信息</param>
        public UserApplicationService(IRepository<User, Guid> userRepository, IConfiguration configuration) 
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns></returns>
        public async Task<BaseResult<TokenInfo>> Login(UserLoginDto loginInfo)
        {
            if (string.IsNullOrEmpty(loginInfo.Account) || string.IsNullOrEmpty(loginInfo.Password))
                return BaseResult<TokenInfo>.Failed("用户名密码不能为空!");

            var user = await Task.FromResult(_userRepository.FirstOrDefault(p => p.Account == loginInfo.Account));
            if (user == null)
            {
                return BaseResult<TokenInfo>.Failed("用户名密码错误!");
            }
            string md5Pwd = EncryptHelper.MD5Encrypt(loginInfo.Password);
            if (user.Password != md5Pwd)
            {
                return BaseResult<TokenInfo>.Failed("用户名密码错误!");
            }

            var claims = GetClaims(user);

            var token = GenerateToken(claims);

            return BaseResult<TokenInfo>.Success(token);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerInfo">注册信息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<BaseResult<TokenInfo>> Register(UserRegisterDto registerInfo,CancellationToken cancellationToken)
        {
            var user = ObjectMapper.Map<UserRegisterDto, User>(registerInfo);

            var registeredUser = await _userRepository.InsertAsync(user, true, cancellationToken);

            var claims = GetClaims(user);

            var token = GenerateToken(claims);

            return BaseResult<TokenInfo>.Success(token);
        }

        #region Token生成

        private IEnumerable<Claim> GetClaims(User user) 
        {
            var claims = new[]
            {
                new Claim(AbpClaimTypes.UserName,user.NickName),
                new Claim(AbpClaimTypes.UserId,user.Id.ToString()),
                new Claim(AbpClaimTypes.PhoneNumber,user.Tel),
                new Claim(AbpClaimTypes.SurName, user.UserStatus == UserStatusEnum.Completed ?user.RealName:string.Empty)
            };
            return claims;
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="claims">声明</param>
        /// <returns></returns>
        private TokenInfo GenerateToken(IEnumerable<Claim> claims) 
        {
            // 密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 过期时间
            int expires = string.IsNullOrEmpty(_configuration["Expires"]) ? 30 : Convert.ToInt32(_configuration["Expires"]);
            
            //生成token
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(expires),
                    signingCredentials: creds);

            return new TokenInfo()
            {
                Expire = expires,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        #endregion
    }
}

```

### 启用认证并添加相关Swagger配置

在需要启动认证的站点模块中添加以下代码（MyShopApiModule）

###### MyShopApiModule.cs

``` csharp

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyShop.Admin.Application;
using MyShop.Admin.Application.Services;
using MyShop.Api.Middleware;
using MyShop.Application;
using MyShop.Application.Contract.Order;
using MyShop.Application.Contract.Product;
using MyShop.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

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

            // 配置jwt
            ConfigureJwt(service);

            // 配置跨域
            ConfigureCors(service);

            // 配置swagger
            ConfigureSwagger(service);

            service.Configure((AbpAspNetCoreMvcOptions options) =>
            {
                options.ConventionalControllers.Create(typeof(Application.ProductApplicationService).Assembly);
                options.ConventionalControllers.Create(typeof(Application.OrderApplicationService).Assembly);
                options.ConventionalControllers.Create(typeof(Application.UserApplicationService).Assembly);
                options.ConventionalControllers.Create(typeof(Application.BasketApplicationService).Assembly);
                options.ConventionalControllers.Create(typeof(Admin.Application.Services.ProductApplicationService).Assembly, options =>
                {
                    options.RootPath = "admin";
                });
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

            // 跨域
            app.UseCors("AllowAll");

            // swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyShopApi");
            });

            app.UseRouting();

            //添加jwt验证 注意：必须先添加认证再添加授权中间件，且必须添加在UseRouting 和UseEndpoints之间
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseConfiguredEndpoints();
        }

        #region ServicesConfigure

        private void ConfigureJwt(IServiceCollection services) 
        {
            var configuration = services.GetConfiguration();
            services
                .AddAuthentication(options=> 
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options=> 
                {
                    options.RequireHttpsMetadata = false;// 开发环境为false

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ClockSkew = TimeSpan.FromSeconds(30), // 偏移时间,所以实际过期时间 = 给定过期时间+偏移时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = configuration["Jwt:Audience"],//Audience
                        ValidIssuer = configuration["Jwt:Issuer"],//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]))//拿到SecurityKey
                    };

                    // 事件
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context => 
                        {
                            return Task.CompletedTask;
                        },
                        OnChallenge = context => 
                        {
                            // 验证失败
                            BaseResult<object> result = new BaseResult<object>(ResponseResultCode.Unauthorized,"未授权",null);
                            context.HandleResponse();
                            context.Response.ContentType = "application/json;utf-8";

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                            await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
                        },
                        OnForbidden = context => 
                        {
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private void ConfigureCors(IServiceCollection services) 
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MyShopApi",
                    Version = "v0.1"
                });
                options.DocInclusionPredicate((docName, predicate) => true);
                options.CustomSchemaIds(type => type.FullName);

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                options.IncludeXmlComments(Path.Combine(basePath, "MyShop.Application.xml"));
                options.IncludeXmlComments(Path.Combine(basePath, "MyShop.Application.Contract.xml"));

                #region 添加请求认证

                //Bearer 的scheme定义
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //参数添加在头部
                    In = ParameterLocation.Header,
                    //使用Authorize头部
                    Type = SecuritySchemeType.Http,
                    //内容为以 bearer开头
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                };

                //把所有方法配置为增加bearer头部信息
                var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "MyShopApi"
                                    }
                                },
                                new string[] {}
                        }
                    };

                options.AddSecurityDefinition("MyShopApi", securityScheme);
                options.AddSecurityRequirement(securityRequirement);

                #endregion

            });
        }

        #endregion
    }
}


```

### 统一响应格式

###### 基础 BaseResult
定义全体响应类型父类，并提供基础响应成功及响应失败结果创建静态函数
``` csharp
    /// <summary>
    /// 基础响应信息
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    public class BaseResult<T> where T:class
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public ResponseResultCode Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public virtual T Data { get; set; }

        /// <summary>
        /// 响应成功信息
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <returns></returns>
        public static BaseResult<T> Success(T data,string message = "请求成功") => new BaseResult<T>(ResponseResultCode.Success,message, data);

        /// <summary>
        /// 响应失败信息
        /// </summary>
        /// <param name="message">响应信息</param>
        /// <returns></returns>
        public static BaseResult<T> Failed(string message = "请求失败!") 
            => new BaseResult<T> (ResponseResultCode.Failed,message,null);

        /// <summary>
        /// 响应异常信息
        /// </summary>
        /// <param name="message">响应信息</param>
        /// <returns></returns>
        public static BaseResult<T> Error(string message = "请求失败!")
            => new BaseResult<T>(ResponseResultCode.Error, message, null);

        /// <summary>
        /// 构造响应信息
        /// </summary>
        /// <param name="code">响应码</param>
        /// <param name="message">响应消息</param>
        /// <param name="data">响应数据</param>
        public BaseResult(ResponseResultCode code,string message,T data) 
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }
    }

    public enum ResponseResultCode 
    {
        Success = 200,
        Failed = 400,
        Unauthorized = 401,
        Error = 500
    }
```

###### 列表 ListResult
派生自BaseResult并添加泛型为IEnumerable
``` csharp
 /// <summary>
    /// 列表响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListResult<T> : BaseResult<IEnumerable<T>> where T : class
    {
        public ListResult(ResponseResultCode code, string message, IEnumerable<T> data) 
        : base(code, message, data)
        {
        }
    }
```

###### 分页列表 PagedResult
派生自BaseResult并添加PageData分页数据类型泛型
``` csharp
public class PagedResult<T> : BaseResult<PageData<T>>
    {
        public PagedResult(ResponseResultCode code, string message, PageData<T> data) : base(code, message, data)
        {
            
        }

        /// <summary>
        /// 响应成功信息
        /// </summary>
        /// <param name="total">数据总条数</param>
        /// <param name="list">分页列表信息</param>
        /// <returns></returns>
        public static PagedResult<T> Success(int total,IEnumerable<T> list,string message= "请求成功") 
            => new PagedResult<T> (ResponseResultCode.Success,message,new PageData<T> (total,list));

    }

    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PageData<T> 
    {

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="total">数据总条数</param>
        /// <param name="list">数据集合</param>
        public PageData(int total,IEnumerable<T> list) 
        {
            this.Total = total;
            this.Data = list;
        }

        /// <summary>
        /// 数据总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
```

### 自定义异常

在我们添加自定义异常时需要先将abp vNext 默认提供的全局异常过滤器移除。
在Module的ConfigureServices中添加移除代码

``` csharp

    // 移除Abp异常过滤器
    Configure<MvcOptions>(options =>
    {
        var index = options.Filters.ToList().FindIndex(filter => filter is ServiceFilterAttribute attr && attr.ServiceType.Equals(typeof(AbpExceptionFilter)));

        if (index > -1)
            options.Filters.RemoveAt(index);
    });

```

定义MyShop自定义异常中间件

``` csharp

/// <summary>
    /// MyShop异常中间件
    /// </summary>
    public class MyShopExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        public MyShopExceptionMiddleware(RequestDelegate next) 
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
            finally 
            {
                await HandleException(context);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex = null) 
        {

            BaseResult<object> result = null; ;

            bool handle = true;
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                result = new BaseResult<object>(ResponseResultCode.Unauthorized, "未授权!", null);
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                result = new BaseResult<object>(ResponseResultCode.Error, "服务繁忙!", null);
            }
            else 
            {
                handle = false;
            }

            if(handle) await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
        }
    }

```

为了方便通过**IApplicationBuilder**调用这里我们添加个扩展方法用于方便添加我们的自定义异常中间件

``` csharp

    public static class MiddlewareExtensions
    {

        /// <summary>
        /// MyShop异常中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMyShopExceptionMiddleware(this IApplicationBuilder app) 
        {
            app.UseMiddleware<MyShopExceptionMiddleware>();
            return app;
        }
    }

```

最后在 Module类中 添加对应的中间件

``` csharp

app.UseMyShopExceptionMiddleware();

```

## 程序运行

###### 访问Order列表
【显示401未授权】
###### 访问登录接口获取token

###### 使用token访问Order列表

###### 手动抛出异常显示自定义异常响应
