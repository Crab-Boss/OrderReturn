using Hangfire;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderReturn.Dtos;
using OrderReturn.Dtos.DHL;
using OrderReturn.Dtos.GLS;
using OrderReturn.Dtos.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Linq;
using Volo.Abp.Uow;

namespace OrderReturn.BackgroundJob
{
    [Queue("push_to_os")]
    [AutomaticRetry(Attempts = 5)]
    public class PushToOSJob : AsyncBackgroundJob<PushToOSJobArgs>, ITransientDependency
    {
        protected IOrderReturnHistoryRepository OrderReturnHistoryRepository { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IAsyncQueryableExecuter AsyncExecuter { get; }
        protected IHttpClientFactory HttpClientFactory { get; }

        public PushToOSJob(IOrderReturnHistoryRepository orderReturnHostoryRepository
            , IAsyncQueryableExecuter asyncExecuter
            , IUnitOfWorkManager unitOfWorkManager
            , IHttpClientFactory httpClientFactory)
        {
            OrderReturnHistoryRepository = orderReturnHostoryRepository;
            AsyncExecuter = asyncExecuter;
            UnitOfWorkManager = unitOfWorkManager;
            HttpClientFactory = httpClientFactory;
        }

        public override async Task ExecuteAsync(PushToOSJobArgs args)
        {
            try
            {
                Logger.LogInformation($"开始执行-推送数据至OS-{DateTime.Now.ToString("HH:mm:ss.fff")}");

                using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
                {
                    var now = DateTime.Now;
                    var models = await AsyncExecuter.ToListAsync(OrderReturnHistoryRepository.Where(x => x.OSId == null && ((x.ErrorCount == null ? 0 : x.ErrorCount) < 3)));

                    if (models != null && models.Any())
                    {
                        foreach (var model in models)
                        {
                            //推送到OS
                            var request = new PushToOsRequest()
                            {
                                OrderId = model.OrderNumber,
                                TrackingNumber = model.TrackingId,
                                Reason = "",
                                KeyId = model.Id.ToString(),
                            };
                            if (model.LogisticsType == LogisticsType.DHL)
                            {
                                var param = JsonConvert.DeserializeObject<DHLReturnRequest>(model.ParamJson);
                                request.CustomerName = param.SenderAddress?.Name1;
                                request.CustomerEmail = param.Email;
                                request.CustomerAddress = param.SenderAddress?.GetFullAddress();
                            }
                            else
                            {
                                var param = JsonConvert.DeserializeObject<GLSReturnRequest>(model.ParamJson);
                                request.CustomerName = param.Sender?.FullName;
                                request.CustomerEmail = param.Sender?.Email;
                                request.CustomerAddress = param.Sender?.Address?.GetFullAddress();
                                request.Reason = param.ReturnReason;
                            }

                            var client = HttpClientFactory.CreateClient();
                            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                            {
                                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                            };
                            var content = new StringContent(JsonConvert.SerializeObject(request, jsonSerializerSettings));
                            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            PushToOsResponse responseDto;

                            try
                            {
                                var response = await (await client.PostAsync("http://os.nbhooya.com/ExternalApi/returnlabel/PushReturnForHappyPlanet", content)).Content.ReadAsStringAsync();
                                responseDto = JsonConvert.DeserializeObject<PushToOsResponse>(response);

                                if (responseDto.Code != "200")
                                {
                                    model.SetErrorMessage(responseDto.Message);
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                model.SetErrorMessage(ex.Message);
                                continue;
                            }

                            model.SetOSInfo(responseDto.Data, responseDto.Timestamp ?? DateTime.Now);
                        }
                    }
                    
                    await uow.CompleteAsync();
                }

                Logger.LogInformation($"执行成功-推送数据至OS-{DateTime.Now.ToString("HH:mm:ss.fff")}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"PushToOSJob--{JsonConvert.SerializeObject(args)}--执行失败--{ex.Message}");
                throw new BackgroundJobExecutionException("A background job execution is failed. See inner exception for details.", ex)
                {
                    JobType = "push_to_os",
                    JobArgs = args,
                };
            }
        }
    }
}
