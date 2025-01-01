namespace MedicineStorage.Models
{
    public class ServiceResult<T>   
    {
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public bool Success => !Errors.Any();
    }
}
