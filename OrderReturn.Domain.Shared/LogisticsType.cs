using System.ComponentModel.DataAnnotations;

namespace OrderReturn
{
    public enum LogisticsType
    {
        /// <summary>
        /// DHL
        /// </summary>
        [Display(Name = "DHL", Description = "DHL")]
        DHL = 1,
        /// <summary>
        /// GLS
        /// </summary>
        [Display(Name = "GLS", Description = "GLS")]
        GLS = 2,
    }
}
