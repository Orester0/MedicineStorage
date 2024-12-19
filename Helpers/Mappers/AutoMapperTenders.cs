using AutoMapper;
using MedicineStorage.DTOs;
using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Helpers.Mappers
{
    public class AutoMapperTenders : Profile
    {
        public AutoMapperTenders()
        {
            CreateMap<CreateTenderDTO, Tender>();
            CreateMap<Tender, ReturnTenderDTO>();

            CreateMap<CreateTenderItemDTO, TenderItem>();
            CreateMap<TenderItem, ReturnTenderItemDTO>();

            CreateMap<CreateTenderProposalDTO, TenderProposal>();
            CreateMap<TenderProposal, ReturnTenderProposalDTO>();

            CreateMap<CreateTenderProposalItemDTO, TenderProposalItem>();
            CreateMap<TenderProposalItem, ReturnTenderProposalItemDTO>();

        }
    }
}
