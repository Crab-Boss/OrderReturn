using System.ComponentModel.DataAnnotations;

namespace OrderReturn.Dtos.DHL
{
    /// <summary>
    /// 对本地接口
    /// </summary>
    public class GetDHLReturnLabelInput
    {
        public string? OrderNumber { get; set; }
        /// <summary>
        /// 接受人Id
        /// </summary>
        public string ReceiverId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? TelephoneNumber { get; set; }
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
        [Required]
        public SenderAddressDto SenderAddress { get; set; }
    }

    public class SenderAddressDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
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
    }
}
