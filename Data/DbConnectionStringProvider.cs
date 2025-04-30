namespace MedicineStorage.Data
{
    public class DbConnectionStringProvider : IDbConnectionStringProvider
    {
        private string? _connectionString;

        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException("Connection string not set for current request.");
            return _connectionString;
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool HasConnectionString()
        {
            return !string.IsNullOrEmpty(_connectionString);
        }
    }

}
