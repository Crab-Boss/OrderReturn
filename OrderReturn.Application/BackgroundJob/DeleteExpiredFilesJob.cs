using Hangfire;
using Microsoft.Extensions.Logging;
using OrderReturn.BlobContainer;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Linq;
using Volo.Abp.Uow;

namespace OrderReturn.BackgroundJob
{
    [Queue("delete_expired_files")]
    [AutomaticRetry(Attempts = 5)]
    public class DeleteExpiredFilesJob : AsyncBackgroundJob<DeleteExpiredFilesJobArgs>, ITransientDependency
    {
        protected IOrderReturnHistoryRepository OrderReturnHistoryRepository { get; }
        protected IAsyncQueryableExecuter AsyncExecuter { get; }
        protected IBlobContainer<DHLPdfContainer> DHLBlobContainer { get; }
        protected IBlobContainer<GLSPdfContainer> GLSBlobContainer { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        public DeleteExpiredFilesJob(IOrderReturnHistoryRepository orderReturnHostoryRepository
            , IAsyncQueryableExecuter asyncExecuter
            , IBlobContainer<DHLPdfContainer> dhlBlobContainer
            , IBlobContainer<GLSPdfContainer> glsBlobContainer
            , IUnitOfWorkManager unitOfWorkManager)
        {
            OrderReturnHistoryRepository = orderReturnHostoryRepository;
            AsyncExecuter = asyncExecuter;
            DHLBlobContainer = dhlBlobContainer;
            GLSBlobContainer = glsBlobContainer;
            UnitOfWorkManager = unitOfWorkManager;
        }
        public override async Task ExecuteAsync(DeleteExpiredFilesJobArgs args)
        {
            try
            {
                Logger.LogInformation($"开始执行-删除过期文件任务-{DateTime.Now.ToString("HH:mm:ss.fff")}");
                using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
                {
                    var now = DateTime.Now;
                    var targetTime = DateTime.Now.Date.AddDays(-7);
                    var models = await AsyncExecuter.ToListAsync(OrderReturnHistoryRepository.Where(x => x.CreationTime < targetTime && x.IsDeleted == false));
                    if (models != null && models.Any())
                    {
                        foreach (var item in models)
                        {
                            if (item.LogisticsType == LogisticsType.DHL)
                            {
                                if(await DHLBlobContainer.ExistsAsync(item.FileName))
                                {
                                    await DHLBlobContainer.DeleteAsync(item.FileName);
                                }
                            }
                            else
                            {
                                if(await GLSBlobContainer.ExistsAsync(item.FileName))
                                {
                                    await GLSBlobContainer.DeleteAsync(item.FileName);
                                }
                            }

                            item.SetIsDeleted();
                            await OrderReturnHistoryRepository.UpdateAsync(item);
                        }
                        await uow.CompleteAsync();
                    }
                }

                Logger.LogInformation($"执行成功-删除过期文件任务-{DateTime.Now.ToString("HH:mm:ss.fff")}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"DeleteExpiredFilesJob--{Newtonsoft.Json.JsonConvert.SerializeObject(args)}--执行失败--{ex.Message}");
                throw new BackgroundJobExecutionException("A background job execution is failed. See inner exception for details.", ex)
                {
                    JobType = "delete_expired_files",
                    JobArgs = args,
                };
            }
            
        }
    }
}
