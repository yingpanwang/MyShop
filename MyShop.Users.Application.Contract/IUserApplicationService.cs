
using MyShop.Application.Core.ResponseModel;
using MyShop.Users.Application.Contract.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MyShop.Application.Contract.User
{
    public interface IUserApplicationService:IApplicationService
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerInfo">注册信息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<BaseResult<TokenInfo>> Register(UserRegisterDto registerInfo, CancellationToken cancellationToken);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns></returns>
        Task<BaseResult<TokenInfo>> Login(UserLoginDto loginInfo);

    }
}
