using AutoMapper;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Mappers
{
    public class AutoMapperTenders : Profile
    {
        public AutoMapperTenders()
        {
            CreateMap<Tender, ReturnTenderDTO>()
               .ForMember(dto => dto.Items, opt => opt.MapFrom(src => src.TenderItems))
               .ForMember(dto => dto.Proposals, opt => opt.MapFrom(src => src.TenderProposals));
            CreateMap<CreateTenderDTO, Tender>();

            CreateMap<TenderItem, ReturnTenderItemDTO>()
                .ForMember(dto => dto.Medicine, opt => opt.MapFrom(src => src.Medicine));
            CreateMap<CreateTenderItemDTO, TenderItem>();

            CreateMap<TenderProposal, ReturnTenderProposalDTO>()
               .ForMember(dto => dto.Items, opt => opt.MapFrom(src => src.Items))
               .ForMember(dto => dto.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser));
            CreateMap<CreateTenderProposalDTO, TenderProposal>();

            CreateMap<TenderProposalItem, ReturnTenderProposalItemDTO>()
                .ForMember(dto => dto.Medicine, opt => opt.MapFrom(src => src.Medicine));
            CreateMap<CreateTenderProposalItemDTO, TenderProposalItem>();

        }
    }
}
