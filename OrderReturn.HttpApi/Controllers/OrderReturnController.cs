using Microsoft.AspNetCore.Mvc;
using OrderReturn.Dtos;
using OrderReturn.Dtos.DHL;
using OrderReturn.Dtos.GLS;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace OrderReturn.Controllers
{
    /// <summary>
    /// 订单退货
    /// </summary>
    [Area("order-return")]
    [ControllerName("订单退货")]
    [Route("api/order-return")]
    public class OrderReturnController : AbpController
    {
        protected IOrderReturnAppService OrderReturnAppService { get; }
        public OrderReturnController(IOrderReturnAppService orderReturnAppService)
        {
            OrderReturnAppService = orderReturnAppService;
        }

        [HttpPost]
        [Route("get-dhl-label")]
        public Task<ReturnLabelDto> GetDHLLabelAsync([FromBody]GetDHLReturnLabelInput input)
        {
            return OrderReturnAppService.GetDHLLabelAsync(input);
        }

        [HttpPost]
        [Route("get-gls-label")]
        public Task<ReturnLabelDto> GetGLSLabelAsync([FromBody]GetGLSReturnLabelInput input)
        {
            return OrderReturnAppService.GetGLSLabelAsync(input);
        }

        [HttpGet]
        [Route("download/{type}/{fileName}")]
        public async Task<IActionResult> DownloadAsync(string fileName,LogisticsType type)
        {
            var fileDto = await OrderReturnAppService.GetBlobAsync(new GetBlobRequestDto { Name = fileName, LogisticsType = type});
            return File(fileDto.Content, "application/octet-stream", fileDto.Name);
        }
#if DEBUG
        [HttpGet]
        [Route("encrypt-pwd")]
        public Task EncryptPwdAsync()
        {
            return OrderReturnAppService.EncryptPwdAsync();
        }
#endif
    }
}
