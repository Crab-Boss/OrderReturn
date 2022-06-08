using OrderReturn.Dtos;
using OrderReturn.Dtos.DHL;
using OrderReturn.Dtos.GLS;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderReturn
{
    public interface IOrderReturnAppService : IApplicationService
    {
        /// <summary>
        /// 获取dhl标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ReturnLabelDto> GetDHLLabelAsync(GetDHLReturnLabelInput input);

        /// <summary>
        /// 获取gls标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ReturnLabelDto> GetGLSLabelAsync(GetGLSReturnLabelInput input);

        /// <summary>
        /// 获取某个文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<BlobDto> GetBlobAsync(GetBlobRequestDto input);


        Task EncryptPwdAsync();
    }
}
