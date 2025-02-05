using Microsoft.Data.SqlClient;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;

namespace backend.Infrastructure.Repositories
{
  public class OrderRepository(IDBConnectionFactory connectionFactory) : IOrderRepository
  {
    public async Task<IEnumerable<OrderEntity>> GetOrdersAsync(SqlConnection? connection = null)
    {
      try
      {
        using (connection ??= connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          List<OrderEntity> orders = [];

          string query = "SELECT Id, ProductId, ProductCount, Price, CreatedAt, CustomerName FROM Orders";

          using (var command = new SqlCommand(query, connection))
          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              orders.Add(new OrderEntity
              {
                Id = reader.GetGuid(0),
                ProductId = reader.GetGuid(1),
                ProductCount = reader.GetInt32(2),
                Price = reader.GetDecimal(3),
                CreatedAt = reader.GetDateTime(4),
                CustomerName = reader.GetString(5)
              });
            }
          }

          return orders;
        }
      }
      catch (SqlException e)
      {
        throw new Exception("Error executing query", e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task<OrderEntity> GetOrderAsync(Guid id, SqlConnection? connection = null)
    {
      try
      {
        using (connection ??= connectionFactory.CreateConnection())
        {
          if(connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          OrderEntity? order = null;

          var query = "SELECT Id, ProductId, ProductCount, Price, CreatedAt, CustomerName FROM Orders WHERE Id = @Id";
          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
              order = new OrderEntity
              {
                Id = reader.GetGuid(0),
                ProductId = reader.GetGuid(1),
                ProductCount = reader.GetInt32(2),
                Price = reader.GetDecimal(3),
                CreatedAt = reader.GetDateTime(4),
                CustomerName = reader.GetString(5)
              };
            }
          }

          if (order == null)
            throw new KeyNotFoundException($"Order with ID {id} was not found");

          return order;
        }
      }
      catch (SqlException e)
      {
        throw new Exception("Error executing query", e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task<OrderEntity> CreateOrderAsync(OrderEntity order)
    {
      try
      {
        using (var connection = connectionFactory.CreateConnection())
        {
            await connection.OpenAsync();

          var query = @"
            INSERT INTO Orders (Id, ProductId, ProductCount, Price, CreatedAt, CustomerName) 
            OUTPUT INSERTED.Id 
            VALUES (@Id, @ProductId, @ProductCount, @Price, @CreatedAt, @CustomerName)";

          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Id", order.Id);
            command.Parameters.AddWithValue("@ProductId", order.ProductId);
            command.Parameters.AddWithValue("@ProductCount", order.ProductCount);
            command.Parameters.AddWithValue("@Price", order.Price);
            command.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);
            command.Parameters.AddWithValue("@CustomerName", order.CustomerName);

            await command.ExecuteScalarAsync();
          }

          return await GetOrderAsync(order.Id, connection);
        }
      }
      catch (SqlException e)
      {
        throw new Exception("Error inserting order into database", e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task<OrderEntity> UpdateOrderAsync(Guid id, OrderEntity order)
    {
      try
      {
        using (var connection = connectionFactory.CreateConnection())
        {
            await connection.OpenAsync();

          var query = @"
            UPDATE Orders 
            SET ProductId = @ProductId,
                ProductCount = @ProductCount,
                Price = @Price,
                CreatedAt = @CreatedAt,
                CustomerName = @CustomerName
            WHERE Id = @Id";

          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@ProductId", order.ProductId);
            command.Parameters.AddWithValue("@ProductCount", order.ProductCount);
            command.Parameters.AddWithValue("@Price", order.Price);
            command.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);
            command.Parameters.AddWithValue("@CustomerName", order.CustomerName);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
              throw new KeyNotFoundException($"Order with ID {id} was not found for update");
          }

          return await GetOrderAsync(id, connection);
        }
      }
      catch (SqlException e)
      {
        throw new Exception("Error updating order in database", e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task<OrderEntity> DeleteOrderAsync(Guid id)
    {
      try
      {
        using (var connection = connectionFactory.CreateConnection())
        {
            await connection.OpenAsync();

          var order = await GetOrderAsync(id, connection);

          var query = "DELETE FROM Orders WHERE Id = @Id";
          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Id", id);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
              throw new KeyNotFoundException($"Order with ID {id} was not found for deletion");
          }

          return order;
        }
      }
      catch (SqlException e)
      {
        throw new Exception("Error deleting order from database", e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }
  }
}
