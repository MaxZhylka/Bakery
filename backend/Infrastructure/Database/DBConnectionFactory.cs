
using backend.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;


namespace backend.Infrastructure.Database
{
  public class DBConnectionFactory : IDBConnectionFactory
  {
    private readonly string _connectionString;
    public DBConnectionFactory(IConfiguration configuration)
    {
      _connectionString = DotNetEnv.Env.GetString("Connection_String") ?? throw new ArgumentNullException("DefaultConnection string is null");
    }

    public SqlConnection CreateConnection()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
