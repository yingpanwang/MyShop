using MyShop.Application.Contract.User.Dto;
using MyShop.Application.Core.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop.Application.Contract.User
{
    public interface IUserApplicationService
    {
        Task<BaseResult<TokenInfo>> Register(UserRegisterDto registerInfo, CancellationToken cancellationToken);
        Task<BaseResult<TokenInfo>> Login(UserLoginDto loginInfo);
    }
}
