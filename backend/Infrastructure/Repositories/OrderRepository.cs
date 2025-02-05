using Microsoft.Data.SqlClient;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;


namespace backend.Infrastructure.Repositories
{
  public class OrderRepository(IDBConnectionFactory connectionFactory) : IOrderRepository
  {

            public async Task<IEnumerable<OrderEntity>> GetOrdersAsync()
    {
      SqlConnection connection = connectionFactory.CreateConnection();
      List<OrderEntity> orders = new List<OrderEntity>();

      await connection.OpenAsync();

      string query = "SELECT Id, ProductId, ProductCount, Price, CreatedAt, CustomerName FROM Orders";
      using (var command = new SqlCommand(query, connection))
      {
        using var reader = await command.ExecuteReaderAsync();
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

    public async Task<OrderEntity> GetOrderAsync(Guid id)
    {
      SqlConnection connection = connectionFactory.CreateConnection();
      OrderEntity order = null;

      await connection.OpenAsync();

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

      if(order == null) throw new Exception("Order not found");

      return order;
    }
    public async Task<OrderEntity> CreateOrderAsync(OrderEntity order)
    {
    Console.WriteLine(order.Price);
    SqlConnection connection = connectionFactory.CreateConnection();
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

      return await GetOrderAsync(order.Id);
    }

    public async Task<OrderEntity> UpdateOrderAsync(Guid id, OrderEntity order)
    {
      SqlConnection connection = connectionFactory.CreateConnection();
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

        await command.ExecuteNonQueryAsync();
      }

      return await GetOrderAsync(id);
    }

    public async Task<OrderEntity> DeleteOrderAsync(Guid id)
    {
      SqlConnection connection = connectionFactory.CreateConnection();
      var order = await GetOrderAsync(id);
      if (order == null) return null;


      await connection.OpenAsync();

      var query = "DELETE FROM Orders WHERE Id = @Id";

      using (var command = new SqlCommand(query, connection))
      {
        command.Parameters.AddWithValue("@Id", id);
        await command.ExecuteNonQueryAsync();
      }

      return order;
    }
  }
}
