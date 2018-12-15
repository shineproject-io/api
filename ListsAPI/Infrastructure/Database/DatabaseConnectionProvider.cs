using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ListsAPI.Infrastructure.Database
{
    public interface IDatabaseConnectionProvider
    {
        IDbConnection New();
    }

    public class DatabaseConnectionProvider : IDatabaseConnectionProvider
    {
        private readonly IConfiguration _configuration;

        public DatabaseConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection New()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}