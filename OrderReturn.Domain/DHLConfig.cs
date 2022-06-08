using System;
using Volo.Abp.Domain.Entities;

namespace OrderReturn
{
    public class DHLConfig : Entity<int>
    {
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; private set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }

        public DHLConfig() { }
        public DHLConfig(string alias, string userName, string password)
        {
            Alias = alias;
            UserName = userName;
            Password = password;
        }
    }
}
