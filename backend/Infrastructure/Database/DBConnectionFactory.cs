
using backend.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;


namespace backend.Infrastructure.Database
{
  public class DBConnectionFactory : IDBConnectionFactory
  {
    private readonly string _connectionString;
    public DBConnectionFactory()
    {
      _connectionString = "Server=192.168.0.104,1433;Database=Lab_4;User ID=flameplay;Password=1234;TrustServerCertificate=True;" ?? throw new ArgumentNullException("DefaultConnection string is null");
    }

    public SqlConnection CreateConnection()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
