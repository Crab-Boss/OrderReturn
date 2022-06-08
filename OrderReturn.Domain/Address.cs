using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace OrderReturn
{
    /// <summary>
    /// 地址
    /// </summary>
    public class Address : ValueObject
    {
        /// <summary>
        /// 国家编码
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 房屋号
        /// </summary>
        public string HouseNumber { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostCode { get; set; }

        public Address(string countryCode, string country, string city, string street, string postCode)
        {
            CountryCode = countryCode;
            Country = country;
            City = city;
            Street = street;
            PostCode = postCode;
        }
        public void SetHouseNumber(string houseNumber)
        {
            HouseNumber = houseNumber;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CountryCode;
            yield return Country;
        }
    }
}
