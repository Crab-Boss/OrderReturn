using System;
using System.Collections.Generic;
using System.Text;

namespace OrderReturn.Dtos.GLS
{
    public class GLSReturnRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string PortalUuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LanguageCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnReason { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SenderDto Sender { get; set; }
    }

    public class SenderDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GLSAddress Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Email { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public SelectedDropOffPoint SelectedDropOffPoint { get; set; }
    }
    public class GLSAddress
    {
        /// <summary>
        /// 
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ZipCode { get; set; }

        public string GetFullAddress()
        {
            return $"{CountryCode} {City} {Street}";
        }
    }
}
