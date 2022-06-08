using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderReturn.BlobContainer;
using OrderReturn.Dtos;
using OrderReturn.Dtos.DHL;
using OrderReturn.Dtos.GLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Caching;
using Volo.Abp.Emailing;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;
using Microsoft.Extensions.FileProviders;

namespace OrderReturn
{
    public class OrderReturnAppService : ApplicationService, IOrderReturnAppService
    {
        protected IHttpClientFactory HttpClientFacgory { get; }
        protected IConfiguration Configuration { get; }
        protected OrderReturnDHLOptions OrderReturnDHLOptions { get; }
        protected OrderReturnGLSOptions OrderReturnGLSOptions { get; }
        protected IBlobContainer<DHLPdfContainer> DHLBlobContainer { get; }
        protected IBlobContainer<GLSPdfContainer> GLSBlobContainer { get; }
        protected IOrderReturnHistoryRepository OrderReturnHistoryRepository { get; }
        protected IDistributedCache<string> IPCache { get; }
        protected IEmailSender EmailSender { get; }
        protected ISettingEncryptionService SettingEncryptionService { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }
        protected IVirtualFileProvider VirtualFileProvider { get; }
        public OrderReturnAppService(IHttpClientFactory httpClientFacgory
            , IConfiguration configuration
            , IOptions<OrderReturnDHLOptions> dhlOptions
            , IOptions<OrderReturnGLSOptions> glsOptions
            , IBlobContainer<DHLPdfContainer> dhlBlobContainer
            , IBlobContainer<GLSPdfContainer> glsBlobContainer
            , IOrderReturnHistoryRepository orderReturnHostoryRepository
            , IDistributedCache<string> ipCache
            , IEmailSender emailSender
            , ISettingEncryptionService settingEncryptionService
            , ISettingDefinitionManager settingDefinitionManager
            , IVirtualFileProvider virtualFileProvider)
        {
            HttpClientFacgory = httpClientFacgory;
            Configuration = configuration;
            OrderReturnDHLOptions = dhlOptions.Value;
            OrderReturnGLSOptions = glsOptions.Value;
            DHLBlobContainer = dhlBlobContainer;
            GLSBlobContainer = glsBlobContainer;
            OrderReturnHistoryRepository = orderReturnHostoryRepository;
            IPCache = ipCache;
            EmailSender = emailSender;
            SettingEncryptionService = settingEncryptionService;
            SettingDefinitionManager = settingDefinitionManager;
            VirtualFileProvider = virtualFileProvider;
        }

        public async Task<ReturnLabelDto> GetDHLLabelAsync(GetDHLReturnLabelInput input)
        {
            //TODO 相同Ip十分钟内不允许多次请求
            try
            {
                var client = HttpClientFacgory.CreateClient();

                Encoding encoding = Encoding.UTF8;
                var authorization = encoding.EncodeBase64($"{OrderReturnDHLOptions.ApplicationId}:{OrderReturnDHLOptions.ApplicationToken}");
                var token = encoding.EncodeBase64($"{OrderReturnDHLOptions.UserName}:{OrderReturnDHLOptions.Password}");

                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {authorization}");
                client.DefaultRequestHeaders.Add("DPDHL-User-Authentication-Token", token);

                var dhlInput = ObjectMapper.Map<GetDHLReturnLabelInput, DHLReturnRequest>(input);
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
                var str = JsonConvert.SerializeObject(dhlInput, jsonSerializerSettings);
                var content = new StringContent(str);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(OrderReturnDHLOptions.Url, content);
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = await response.Content.ReadAsStringAsync();

                var dhlResponse = JsonConvert.DeserializeObject<DHLReturnResponse>(responseBody);

                var id = GuidGenerator.Create();
                var fileName = $"dhl-return-{id}.pdf";

                #region 存储文件
                byte[] bt = Convert.FromBase64String(dhlResponse.LabelData);
                await DHLBlobContainer.SaveAsync(fileName, bt);
                #endregion

                #region 数据库记录
                //var inputJson = JsonConvert.SerializeObject(input, jsonSerializerSettings);
                var model = new OrderReturnHistory(id, LogisticsType.DHL, input.OrderNumber, str, dhlResponse.ShipmentNumber);
                model.SetFileName(fileName);

                await OrderReturnHistoryRepository.InsertAsync(model);
                await CurrentUnitOfWork.SaveChangesAsync();
                #endregion

                #region 发送邮件
                try
                {
                    if (!input.Email.IsNullOrWhiteSpace())
                    {
                        MailMessage mailMsg = new MailMessage();
                        //附件
                        var stream = new MemoryStream(bt);
                        mailMsg.Attachments.Add(new Attachment(stream, fileName));
                        //body
                        mailMsg.Body = VirtualFileProvider.GetFileInfo("/File/DHLEmail.html").ReadAsString();
                        mailMsg.IsBodyHtml = true;
                        mailMsg.Subject = "Return label for your return shipment to FDS GmbH";
                        mailMsg.To.Add(input.Email);
                        await EmailSender.SendAsync(mailMsg);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, LogLevel.Error);
                }
                #endregion

                return new ReturnLabelDto
                {
                    Id = model.Id,
                    FileName = model.FileName,
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }   
        }

        public async Task<ReturnLabelDto> GetGLSLabelAsync(GetGLSReturnLabelInput input)
        {
            try
            {
                var glsRequest = ObjectMapper.Map<GetGLSReturnLabelInput, GLSReturnRequest>(input);
                var client = HttpClientFacgory.CreateClient();
                client.DefaultRequestHeaders.Add("apikey", OrderReturnGLSOptions.Apikey);

                
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
                var str = JsonConvert.SerializeObject(glsRequest, jsonSerializerSettings);
                var content = new StringContent(str);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(OrderReturnGLSOptions.Url, content);
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = await response.Content.ReadAsStringAsync();

                var glsResponse = JsonConvert.DeserializeObject<GLSReturnResponse>(responseBody);

                var id = GuidGenerator.Create();
                var fileName = $"gls-return-{id}.pdf";


                #region 文件下载到本地
                var imgClient = HttpClientFacgory.CreateClient();
                byte[] bytes;
                try
                {
                    bytes = await imgClient.GetByteArrayAsync(glsResponse.SignedLabelUrl);
                    await GLSBlobContainer.SaveAsync(fileName, bytes);
                }
                catch (Exception ex)
                {
                    Logger.LogError("获取图片异常", ex);
                    throw new UserFriendlyException(ex.Message);
                }
                #endregion

                #region 数据库记录
                //var inputJson = JsonConvert.SerializeObject(input, jsonSerializerSettings);
                var model = new OrderReturnHistory(id, LogisticsType.GLS, glsRequest.OrderNumber, str, glsResponse.ParcelNumberWithChecksum);
                model.SetFileName(fileName);

                await OrderReturnHistoryRepository.InsertAsync(model);
                await CurrentUnitOfWork.SaveChangesAsync();
                #endregion

                return new ReturnLabelDto
                {
                    Id = model.Id,
                    FileName = fileName,
                };
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, "获取GLSLable出错");
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<BlobDto> GetBlobAsync(GetBlobRequestDto input)
        {
            byte[] blob;
            if(input.LogisticsType == LogisticsType.DHL)
            {
                blob = await DHLBlobContainer.GetAllBytesOrNullAsync(input.Name);
            }
            else
            {
                blob = await GLSBlobContainer.GetAllBytesOrNullAsync(input.Name);
            }
            
            if(blob == null)
            {
                throw new UserFriendlyException("The file cannot be found!");
            }

            return new BlobDto
            {
                Name = input.Name,
                Content = blob
            };
        }

        public async Task EncryptPwdAsync()
        {
            var setting = SettingDefinitionManager.Get(EmailSettingNames.Smtp.Password);
            var psd = SettingEncryptionService.Encrypt(setting, "Hooya5000");

            var fileName = "dhl-return-3d188ca9-14da-0103-2873-3a023758fb38.pdf";
            byte[] bt;
            bt = await DHLBlobContainer.GetAllBytesOrNullAsync(fileName);
            MailMessage mailMsg = new MailMessage();
            //附件
            var stream = new MemoryStream(bt);
            mailMsg.Attachments.Add(new Attachment(stream, fileName));
            //body
            var file = VirtualFileProvider.GetFileInfo("/File/DHLEmail.html");
            var fileContent = file.ReadAsString();
            mailMsg.Body = fileContent;
            mailMsg.IsBodyHtml = true;
            mailMsg.Subject = "Return label for your return shipment to FDS GmbH";
            mailMsg.To.Add("jie.yao@nbhooya.com");

            await EmailSender.SendAsync(mailMsg);
        }
    }
}
