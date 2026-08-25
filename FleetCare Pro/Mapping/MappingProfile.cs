using AutoMapper;
using FleetCare_Pro.Models;
using FleetCare_Pro.Models.ViewModels;

namespace FleetCare_Pro.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VehicleFormViewModel, Vehicle>()
                .ForMember(dest => dest.VehicleImageURL, opt => opt.Ignore()).ReverseMap();

            CreateMap<ServiceFormViewModel, ServiceRecord>()
                .ForMember(dest => dest.TotalCost, opt => opt.Ignore()) // هنحسبها في الكنترولر 
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ServiceRecordStatus.Completed))
                .ForMember(dest => dest.InvoiceDocumentPath, opt => opt.Ignore());

            CreateMap<ServiceItemFormViewModel, ServiceLineItem>();
        }
    }
}
