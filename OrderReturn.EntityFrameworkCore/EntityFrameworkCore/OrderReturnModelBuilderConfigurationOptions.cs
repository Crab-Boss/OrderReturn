using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrderReturn.EntityFrameworkCore
{
    public class OrderReturnModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public OrderReturnModelBuilderConfigurationOptions(
        [NotNull] string tablePrefix,
        [CanBeNull] string schema)
        : base(
        tablePrefix,
        schema)
        {

        }
    }
}
