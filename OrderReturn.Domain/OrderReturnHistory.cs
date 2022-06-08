using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderReturn
{
    /// <summary>
    /// 订单退货历史记录
    /// </summary>
    public class OrderReturnHistory : CreationAuditedEntity<Guid>
    {
        /// <summary>
        /// 物流公司类型 DHL、GLS
        /// </summary>
        public LogisticsType LogisticsType { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNumber { get; private set; }

        /// <summary>
        /// 提交到物流方json字符串
        /// </summary>
        public string ParamJson { get; private set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 追踪号
        /// </summary>
        public string? TrackingId { get; private set; }

        public bool IsDeleted { get; private set; }

        /// <summary>
        /// 推送到OS后回来的主键
        /// </summary>
        public string? OSId { get; private set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime? PushTime { get; private set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string? ErrorMessage { get; private set; }
        /// <summary>
        /// 错误次数
        /// </summary>
        public int? ErrorCount { get; private set; }
        public OrderReturnHistory()
        {

        }
        public OrderReturnHistory(Guid id, LogisticsType logisticsType, string orderNumber, string paramJson, string trackingId)
        {
            Id = id;
            OrderNumber = orderNumber;
            LogisticsType = logisticsType;
            ParamJson = paramJson;
            TrackingId = trackingId;
        }
        //public OrderReturnHistory(Guid id,LogisticsType logisticsType, string orderNumber,string senderName, Address address)
        //{
        //    LogisticsType = logisticsType;
        //    OrderNumber = orderNumber;
        //    SenderName = senderName;
        //    Address = address;
        //}
        //public void SetEmail(string email)
        //{
        //    Email = email;
        //}
        //public void SetPhone(string phone)
        //{
        //    Phone = phone;
        //}
        //public void SetReturnReason(string returnReason)
        //{
        //    ReturnReason = returnReason;
        //}
        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }
        public void SetIsDeleted(bool isDeleted = true)
        {
            IsDeleted = isDeleted;
        }
        /// <summary>
        /// 设置推送到Os的信息
        /// </summary>
        /// <param name="osId"></param>
        /// <param name="pushTime"></param>
        public void SetOSInfo(string osId, DateTime pushTime)
        {
            OSId = osId;
            PushTime = pushTime;
        }
        public void SetErrorMessage(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ErrorCount = ErrorCount == null ? 1 : ErrorCount.Value + 1;
        }
    }
}
