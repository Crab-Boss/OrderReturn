using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderReturn.Dtos
{
    /// <summary>
    /// 对dhl接口
    /// </summary>
    public class DHLReturnRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string ReceiverId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipmentReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SenderAddress SenderAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TelephoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int WeightInGrams { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnDocumentType { get; set; }
    }
    public class CountryDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string CountryISOCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }
    }
    public class SenderAddress
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StreetName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HouseNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public CountryDto Country { get; set; }

        public string GetFullAddress()
        {
            return $"{Country?.Country} {City} {StreetName} {HouseNumber}";
        }
    }   

}
