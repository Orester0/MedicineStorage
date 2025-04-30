using MedicineStorage.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Helpers
{
    public static class DbUsersGenerator
    {
        public static async Task CreateDatabaseUsers(IServiceProvider services, IConfiguration configuration)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.EnsureCreatedAsync();
            }

            await using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                // Create roles
                CreateDatabaseRole(connection, "NonAuthorisedUser");
                CreateDatabaseRole(connection, "BaseUser");
                CreateDatabaseRole(connection, "AdminUser");

                // Grant permissions
                GrantNonAuthorisedUserPermissions(connection);
                GrantBaseUserPermissions(connection);
                GrantAdminUserPermissions(connection);

                // Create logins/users with roles
                CreateUserWithRole(connection, "guest_user", "StrongPassword123!", "NonAuthorisedUser");
                CreateUserWithRole(connection, "standard_user", "StrongPassword123!", "BaseUser");
                CreateUserWithRole(connection, "admin_user", "StrongPassword123!", "AdminUser");
            }
        }

        private static void CreateDatabaseRole(SqlConnection conn, string roleName)
        {
            string sql = $"""
            IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '{roleName}')
            BEGIN
                CREATE ROLE [{roleName}];
            END
            """;
            using var cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        private static void GrantNonAuthorisedUserPermissions(SqlConnection conn)
        {
            string sql = """
                GRANT SELECT ON SCHEMA::dbo TO NonAuthorisedUser;
                GRANT INSERT, UPDATE ON OBJECT::dbo.RefreshTokens TO NonAuthorisedUser;
                GRANT EXECUTE ON SCHEMA::dbo TO NonAuthorisedUser;
            """;
            using var cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        private static void GrantBaseUserPermissions(SqlConnection conn)
        {
            string sql = """
                GRANT SELECT, INSERT, UPDATE ON SCHEMA::dbo TO BaseUser;



                GRANT SELECT ON SCHEMA::dbo TO BaseUser;
                GRANT EXECUTE ON SCHEMA::dbo TO BaseUser;
                GRANT SELECT ON OBJECT::dbo.Medicines TO BaseUser;
                GRANT SELECT ON OBJECT::dbo.MedicineCategories TO BaseUser;
                GRANT SELECT, INSERT, UPDATE ON OBJECT::dbo.MedicineRequests TO BaseUser;
                GRANT SELECT, INSERT, UPDATE ON OBJECT::dbo.MedicineUsages TO BaseUser;
                GRANT SELECT, INSERT, UPDATE ON OBJECT::dbo.Audits TO BaseUser;
                GRANT SELECT, INSERT, UPDATE ON OBJECT::dbo.AuditItems TO BaseUser;
                GRANT SELECT, INSERT, UPDATE ON OBJECT::dbo.AuditNote TO BaseUser;
            """;
            using var cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        private static void GrantAdminUserPermissions(SqlConnection conn)
        {
            string sql = """
                GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO AdminUser;
                GRANT SELECT, INSERT, UPDATE, DELETE ON OBJECT::dbo.MedicineSupplies TO AdminUser;
                GRANT EXECUTE ON SCHEMA::dbo TO AdminUser;
            """;
            using var cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }


        private static void CreateUserWithRole(SqlConnection conn, string loginName, string password, string roleName)
        {
            string createLogin = $"""
            IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '{loginName}')
            BEGIN
                CREATE LOGIN [{loginName}] WITH PASSWORD = '{password}';
            END
            """;

            string createUser = $"""
            IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '{loginName}')
            BEGIN
                CREATE USER [{loginName}] FOR LOGIN [{loginName}];
                ALTER ROLE [{roleName}] ADD MEMBER [{loginName}];
            END
            """;

            using var cmd1 = new SqlCommand(createLogin, conn);
            cmd1.ExecuteNonQuery();

            using var cmd2 = new SqlCommand(createUser, conn);
            cmd2.ExecuteNonQuery();
        }
    }
}
//SELECT
//    s.session_id,
//    s.login_name AS[User],
//    s.last_request_start_time,
//    s.last_request_end_time
//FROM 
//    sys.dm_exec_sessions s
//WHERE 
//    s.is_user_process = 1
//ORDER BY 
//    s.last_request_start_time DESC;
