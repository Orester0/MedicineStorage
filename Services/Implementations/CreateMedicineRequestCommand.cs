using MediatR;
using MedicineStorage.Models.Medicine;

namespace MedicineStorage.Services.Implementations
{
    public class CreateMedicineRequestCommand : IRequest<MedicineRequest>
    {
        public int DoctorId { get; set; }
        public List<MedicineRequestItem> Items { get; set; }
    }

    public class CreateMedicineRequestHandler
    : IRequestHandler<CreateMedicineRequestCommand, MedicineRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMedicineRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MedicineRequest> Handle(
            CreateMedicineRequestCommand command,
            CancellationToken cancellationToken)
        {
            // Implementation
            return new MedicineRequest();
        }
    }
}
