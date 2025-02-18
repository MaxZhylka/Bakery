using Microsoft.Data.SqlClient;
using backend.Infrastructure.Interfaces;

using backend.Core.DTOs;
using backend.Core.Enums;
using Core.Exceptions;
using Microsoft.CodeAnalysis.Emit;
using backend.Core.Models;


namespace backend.Infrastructure.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly IDBConnectionFactory _connectionFactory;

    public UserRepository(IDBConnectionFactory connectionFactory)
    {
      _connectionFactory = connectionFactory;
    }

    public async Task<UserDTO> GetUserAsync(Guid id)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          UserDTO? user = null;

          var query = "SELECT Id, Name, Email, Role FROM Users WHERE Id = @Id";
          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
              user = new UserDTO
              {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Role = Enum.Parse<UserRole>(reader.GetString(3))
              };
            }
          }

          if (user == null)
            throw new UserNotFoundException(id);

          return user;
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.GetUser, e);
      }
     catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }

    public async Task<PaginatedResult<UserDTO>> GetUsersAsync(PaginationParameters paginationParameters)
    {
      try
      {
        using var connection = _connectionFactory.CreateConnection();
        if (connection.State != System.Data.ConnectionState.Open)
          await connection.OpenAsync();

        const string countQuery = @"SELECT COUNT(*) FROM Users";
        using var countCommand = new SqlCommand(countQuery, connection);
        var totalRecords = (int)(await countCommand.ExecuteScalarAsync() ?? 0);

        const string dataQuery = @"
            SELECT 
                Id, 
                Name, 
                Email, 
                Role 
            FROM Users
            ORDER BY Name ASC
            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

        using var dataCommand = new SqlCommand(dataQuery, connection);
        dataCommand.Parameters.AddWithValue("@Offset", paginationParameters.Offset * paginationParameters.Size);
        dataCommand.Parameters.AddWithValue("@Limit", paginationParameters.Size);

        var users = new List<UserDTO>();
        using var reader = await dataCommand.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
          users.Add(new UserDTO
          {
            Id = reader.GetGuid(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            Role = Enum.Parse<UserRole>(reader.GetString(3))
          });
        }

        return new PaginatedResult<UserDTO>
        {
          Data = users.ToArray(),
          Total = totalRecords,
        };
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.GetUsers, e);
      }
     catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }
    public async Task<UserDTO> DeleteUserAsync(Guid id)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          var user = await GetUserAsync(id);

          var query = "DELETE FROM Users WHERE Id = @Id";
          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Id", id);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
              throw new KeyNotFoundException($"User with ID {id} was not found for deletion");
          }

          return user;
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.DeleteUser, e);
      }
     catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }

    public async Task<UserDTO> UpdateUserAsync(Guid id, UserDTO user)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          var query = @"
                        UPDATE Users 
                        SET Name = @Name,
                        Email = @Email,
                        Role = @Role
                        WHERE Id = @Id";

          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Role", user.Role);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
              throw new KeyNotFoundException($"User with ID {id} was not found for update");
          }

          return await GetUserAsync(id);
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.UpdateUser, e);
      }
     catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }

    public async Task<UserDTO> CreateUserAsync(UserEntity user)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          var query = @"
                        INSERT INTO Users (Id, Name, Email, Role, Password, CreatedAt) 
                        OUTPUT INSERTED.Id 
                        VALUES (@Id, @Name, @Email, @Role, @Password, @CreatedAt)";

          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Role", user.Role);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);

            await command.ExecuteScalarAsync();
          }

          return await GetUserAsync(user.Id);
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.CreateUser, e);
      }
      catch (Exception e)
      {
        throw new Exception($"Error getting DB connection{e}");
      }
    }
  }
}