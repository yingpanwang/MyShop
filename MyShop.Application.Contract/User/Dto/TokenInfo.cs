using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Application.Contract.User.Dto
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public int Expire { get; set; }
    }
}
