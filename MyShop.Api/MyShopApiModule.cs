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

            // 移除Abp异常过滤器
            Configure<MvcOptions>(options =>
            {
                var index = options.Filters.ToList().FindIndex(filter => filter is ServiceFilterAttribute attr && attr.ServiceType.Equals(typeof(AbpExceptionFilter)));
                if (index > -1)
                    options.Filters.RemoveAt(index);
            });

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

            app.UseMyShopExceptionMiddleware();

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
                            return Task.CompletedTask;
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
