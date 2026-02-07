using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TestCRM.DAL.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException("Connection string is missing in appsettings.json");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
