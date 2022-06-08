using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace OrderReturn.DataSeed
{
    public class DHLConfigDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        public IDHLConfigRepository DHLConfigRepository { get; }
        public DHLConfigDataSeedContributor(IDHLConfigRepository dhlConfigRepository)
        {
            DHLConfigRepository = dhlConfigRepository;
        }

        public virtual async Task SeedAsync(DataSeedContext context)
        {
            var configs = new List<DHLConfig>();
            //configs.Add(new DHLConfig("1号配置", "123456", "123456"));
            //configs.Add(new DHLConfig("2号配置", "987654", "987654"));

            foreach (var item in configs)
            {
                if (!DHLConfigRepository.Any(x => x.Alias == item.Alias))
                {
                    await DHLConfigRepository.InsertAsync(item, true);
                }
            }
        }
    }
}
