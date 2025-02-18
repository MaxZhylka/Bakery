using Microsoft.Data.SqlClient;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;
using Core.Exceptions;
using backend.Core.Enums;
using backend.Core.DTOs;


namespace backend.Infrastructure.Repositories
{
  public class OrderRepository(IDBConnectionFactory connectionFactory) : IOrderRepository
  {
    private readonly IDBConnectionFactory _connectionFactory = connectionFactory;
    public async Task<PaginatedResult<OrderDTO>> GetOrdersAsync(PaginationParameters paginationParameters, SqlConnection? connection = null)
    {
      try
      {
        using (connection ??= _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          const string countQuery = "SELECT COUNT(*) FROM Orders";
          int totalCount;
          using (var countCommand = new SqlCommand(countQuery, connection))
          {
            totalCount = (int)(await countCommand.ExecuteScalarAsync() ?? 0);
          }

          string itemsQuery = @"
                        SELECT Id, ProductId, CustomerName, ProductCount, Price, CreatedAt
                        FROM Orders
                        ORDER BY CreatedAt DESC
                        OFFSET @Offset ROWS
                        FETCH NEXT @PageSize ROWS ONLY
                    ";

          var items = new List<OrderDTO>();

          using (var itemsCommand = new SqlCommand(itemsQuery, connection))
          {
            int offset = paginationParameters.Offset * paginationParameters.Size;
            itemsCommand.Parameters.AddWithValue("@Offset", offset);
            itemsCommand.Parameters.AddWithValue("@PageSize", paginationParameters.Size);

            using var reader = await itemsCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
              var product = new OrderDTO
              {
                Id = reader.GetGuid(0),
                ProductId = reader.GetGuid(1),
                CustomerName = reader.GetString(2),
                ProductCount = reader.GetInt32(3),
                Price = reader.GetDecimal(4),
                CreatedAt = reader.GetDateTime(5)
              };
              items.Add(product);
            }
          }

          return new PaginatedResult<OrderDTO>
          {
            Total = totalCount,
            Data = items
          };
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.GetProducts, e);
      }
      catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }

    public async Task<PaginatedResult<OrderDTO>> GetOrdersByUserIdAsync(PaginationParameters paginationParameters, Guid userId)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          const string countQuery = @"
                SELECT COUNT(*) 
                FROM Orders
                WHERE UserId = @UserId
            ";
          int totalCount;
          using (var countCommand = new SqlCommand(countQuery, connection))
          {
            countCommand.Parameters.AddWithValue("@UserId", userId);
            totalCount = (int)(await countCommand.ExecuteScalarAsync() ?? 0);
          }

          string itemsQuery = @"
                SELECT Id, ProductId, CustomerName, ProductCount, Price, CreatedAt
                FROM Orders
                WHERE UserId = @UserId
                ORDER BY CreatedAt DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY
            ";

          var items = new List<OrderDTO>();

          using (var itemsCommand = new SqlCommand(itemsQuery, connection))
          {
            int offset = paginationParameters.Offset * paginationParameters.Size;
            itemsCommand.Parameters.AddWithValue("@UserId", userId);
            itemsCommand.Parameters.AddWithValue("@Offset", offset);
            itemsCommand.Parameters.AddWithValue("@PageSize", paginationParameters.Size);

            using var reader = await itemsCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
              var orderDto = new OrderDTO
              {
                Id = reader.GetGuid(0),
                ProductId = reader.GetGuid(1),
                CustomerName = reader.GetString(2),
                ProductCount = reader.GetInt32(3),
                Price = reader.GetDecimal(4),
                CreatedAt = reader.GetDateTime(5)
              };
              items.Add(orderDto);
            }
          }

          return new PaginatedResult<OrderDTO>
          {
            Total = totalCount,
            Data = items
          };
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.GetProducts, e);
      }
      catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }

    public async Task<OrderEntity> GetOrderAsync(Guid id, SqlConnection? connection = null)
    {
      try
      {
        using (connection ??= _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
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
            throw new OrderNotFoundException(id);

          return order;
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.GetOrder, e);
      }
      catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }

    public async Task<OrderEntity> CreateOrderAsync(OrderEntity order)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
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
        throw new DatabaseOperationException(Operations.CreateOrder, e);
      }
      catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }

    public async Task<OrderEntity> UpdateOrderAsync(Guid id, OrderEntity order)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
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
        throw new DatabaseOperationException(Operations.UpdateOrder, e);
      }
      catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }

    public async Task<OrderEntity> DeleteOrderAsync(Guid id)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
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
        throw new DatabaseOperationException(Operations.DeleteOrder, e);
      }
      catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }
  }
}
