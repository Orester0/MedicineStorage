namespace MedicineStorage.Models
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }

}
