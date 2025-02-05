using Microsoft.Data.SqlClient;

namespace backend.Infrastructure.Interfaces
{
  public interface IDBConnectionFactory
  {
    SqlConnection CreateConnection();
  }
}