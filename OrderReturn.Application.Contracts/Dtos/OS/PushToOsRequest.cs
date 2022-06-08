namespace OrderReturn.Dtos.OS
{
    /// <summary>
    /// 推送到OS请求入参
    /// </summary>
    public class PushToOsRequest
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string? OrderId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string? CustomerName { get; set; }

        /// <summary>
        /// 客户email
        /// </summary>
        public string? CustomerEmail { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        public string? CustomerAddress { get; set; }

        /// <summary>
        /// 退货跟踪号
        /// </summary>
        public string? TrackingNumber { get; set; }

        /// <summary>
        /// 退货原因
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// 原记录Id
        /// </summary>
        public string? KeyId { get; set; }
    }
}
