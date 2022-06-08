using AutoMapper;
using OrderReturn.Dtos;
using OrderReturn.Dtos.DHL;
using OrderReturn.Dtos.GLS;
using Volo.Abp.AutoMapper;

namespace OrderReturn
{
    public class OrderReturnApplicationModuleAutoMapperProfile : Profile
    {
        public OrderReturnApplicationModuleAutoMapperProfile()
        {

            CreateMap<GetDHLReturnLabelInput, DHLReturnRequest>()
                .Ignore(x => x.CustomerReference)
                .Ignore(x => x.ShipmentReference)
                .ForMember(x => x.ReturnDocumentType, opt => opt.MapFrom(src => "SHIPMENT_LABEL"));

            CreateMap<SenderAddressDto, SenderAddress>()
                .ForMember(x => x.Name1, opt => opt.MapFrom(src => src.Name))
                .Ignore(x => x.Name2)
                .Ignore(x => x.Name3);

            CreateMap<GetGLSReturnLabelInput, GLSReturnRequest>()
                .ForMember(x => x.PortalUuid, opt => opt.MapFrom(src => "-costway"))
                .ForMember(x => x.LanguageCode, opt => opt.MapFrom(src => "en"));

        }
    }
}
