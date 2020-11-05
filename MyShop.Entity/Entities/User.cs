using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace MyShop.Domain.Entities
{
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
}
