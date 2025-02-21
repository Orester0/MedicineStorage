using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineSupplyRepository : IGenericRepository<MedicineSupply>
    {
        Task<(IEnumerable<MedicineSupply>, int)> GetByParamsAsync(MedicineSupplyParams parameters);
    }
}
