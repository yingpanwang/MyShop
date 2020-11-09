using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Users.Application.Contract.Dto
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public int Expire { get; set; }
    }
}
