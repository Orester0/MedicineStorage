using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Helpers
{
    public class AutoMapperTenders : Profile
    {
        public AutoMapperTenders()
        {
            // Tender Mappings
            CreateMap<CreateTenderDTO, Tender>()
                .ForMember(dest => dest.PublishDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.Proposals, opt => opt.Ignore());

            CreateMap<Tender, ReturnTenderDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            // TenderItem Mappings
            CreateMap<CreateTenderItemDTO, TenderItem>();
            CreateMap<TenderItem, ReturnTenderItemDTO>();

            // TenderProposal Mappings
            CreateMap<CreateTenderProposalDTO, TenderProposal>()
                .ForMember(dest => dest.SubmissionDate, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Tender, opt => opt.Ignore())
                .ForMember(dest => dest.Distributor, opt => opt.Ignore());

            CreateMap<TenderProposal, ReturnTenderProposalDTO>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            // TenderProposalItem Mappings
            CreateMap<CreateTenderProposalItemDTO, TenderProposalItem>();
            CreateMap<TenderProposalItem, ReturnTenderProposalItemDTO>();

        }
    }
}
