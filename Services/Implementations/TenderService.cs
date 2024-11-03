using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.Interfaces;

namespace MedicineStorage.Services.Implementations
{
    public class TenderService(ITenderRepository _tenderRepository, ITenderProposalRepository _proposalRepository) : ITenderService
    {

        public async void CreateTender(Tender tender)
        {
            tender.DeadlineDate = DateTime.Now.AddDays(30);
            await _tenderRepository.AddAsync(tender);
        }

        public async void SubmitProposal(TenderProposal proposal)
        {
             var tender = await _tenderRepository.GetByIdAsync(proposal.TenderId);
            if (tender == null || DateTime.Now > tender.DeadlineDate)
                throw new InvalidOperationException("Proposal deadline has passed");

            await _proposalRepository.AddAsync(proposal);
        }

        public async void FinalizeAgreement(int tenderId, int proposalId)
        {
            var proposal = await _proposalRepository.GetByIdAsync(proposalId);
            if (proposal != null && proposal.TenderId == tenderId)
            {
                proposal.Status = ProposalStatus.Accepted;
                await _proposalRepository.Update(proposal);
            }
        }
    }
}
