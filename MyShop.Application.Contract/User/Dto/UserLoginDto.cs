using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyShop.Application.Contract.User.Dto
{
    public class UserLoginDto
    {

        [Required]
        public string Account { get; set; }

        [Required]
        public string Password { get; set; }


        public string VerificationCode { get; set; }
    }
}
