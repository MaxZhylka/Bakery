
using backend.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;


namespace backend.Infrastructure.Database
{
  public class DBConnectionFactory : IDBConnectionFactory
  {
    private readonly string _connectionString;
    public DBConnectionFactory(IConfiguration configuration)
    {
      _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("DefaultConnection string is null");
    }

    public SqlConnection CreateConnection()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
