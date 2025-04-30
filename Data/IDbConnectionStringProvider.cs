namespace MedicineStorage.Data
{
    public interface IDbConnectionStringProvider
    {
        string GetConnectionString();
        void SetConnectionString(string connectionString);
        bool HasConnectionString();
    }

}
