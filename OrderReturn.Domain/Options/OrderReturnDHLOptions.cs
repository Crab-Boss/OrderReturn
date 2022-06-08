namespace OrderReturn
{
    public class OrderReturnDHLOptions
    {
        /// <summary>
        /// Sandbox
        ///     User name: DeveloperID
        ///     Password: Developer portal password
        /// Live
        ///     User name: ApplicationID
        ///     Password: Application token
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// Sandbox
        ///     User name: DeveloperID
        ///     Password: Developer portal password
        /// Live
        ///     User name: ApplicationID
        ///     Password: Application token
        /// </summary>
        public string ApplicationToken { get; set; }
        
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
    }
}
