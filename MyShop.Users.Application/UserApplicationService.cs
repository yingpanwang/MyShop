using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyShop.Application.Contract.User;
using MyShop.Application.Core.Helpers;
using MyShop.Application.Core.ResponseModel;
using MyShop.Domain.Entities;
using MyShop.Users.Application.Contract.Dto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Security.Claims;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace MyShop.Users.Application
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
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = claims.FirstOrDefault(x=>x.Type == AbpClaimTypes.UserName)?.Value
            };
        }

        #endregion
    }
}
