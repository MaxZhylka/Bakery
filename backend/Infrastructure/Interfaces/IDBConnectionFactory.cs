using MySqlConnector;

namespace backend.Infrastructure.Interfaces
{
  public interface IDBConnectionFactory
  {
    MySqlConnection CreateConnection();
  }
}