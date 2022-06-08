using System;

namespace OrderReturn.Dtos.OS
{
    /// <summary>
    /// 推送到OS的响应
    /// </summary>
    public class PushToOsResponse
    {
        public string Data { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
