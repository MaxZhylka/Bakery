using Microsoft.Data.SqlClient;
using backend.Core.Models;
using backend.Core.DTOs;
using backend.Core.Enums;
using backend.Infrastructure.Interfaces;
using Core.Exceptions;

namespace backend.Infrastructure.Repositories
{
  public class AuthRepository : IAuthRepository
  {
    private readonly IDBConnectionFactory _connectionFactory;

    public AuthRepository(IDBConnectionFactory connectionFactory)
    {
      _connectionFactory = connectionFactory;
    }

    public async Task<UserEntity> GetUserByEmailAsync(string email)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          UserEntity? user = null;

          var query = "SELECT Id, Name, Email, Password, Role FROM Users WHERE Email = @Email";
          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Email", email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
              user = new UserEntity
              {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                Role = Enum.Parse<UserRole>(reader.GetString(4))
              };
            }
          }

          if (user == null)
            throw new UserNotFoundException(email);

          return user;
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.GetUser, e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task<UserEntity> GetUserByRefreshTokenAsync(string refreshToken)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          UserEntity? user = null;

          var query = @"SELECT u.Id, u.Name, u.Email, u.Password, u.Role 
                        FROM Users u 
                        INNER JOIN RefreshTokens rt ON u.Id = rt.UserId 
                        WHERE rt.RefreshToken = @Token";
          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Token", refreshToken);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
              user = new UserEntity
              {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                Role = Enum.Parse<UserRole>(reader.GetString(4))
              };
            }
          }

          if (user == null)
            throw new UserNotFoundByRefreshException();

          return user;
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.GetUser, e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task<RefreshTokenEntity> SaveRefreshTokenAsync(Guid userId, string refreshToken, string deviceId)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          var query = @"
                        INSERT INTO RefreshTokens (Id, UserId, RefreshToken, DeviceId, Expiration) 
                        VALUES (@Id, @UserId, @Token, @DeviceId, @Expiration)";
          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@Id", Guid.NewGuid());
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Token", refreshToken);
            command.Parameters.AddWithValue("@DeviceId", deviceId);
            command.Parameters.AddWithValue("@Expiration", DateTime.UtcNow.AddDays(30));

            await command.ExecuteNonQueryAsync();
          }

          return new RefreshTokenEntity
          {
            UserId = userId,
            RefreshToken = refreshToken,
            DeviceId = deviceId,
            Expiration = DateTime.UtcNow.AddDays(30)
          };
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.Register, e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task<bool> CheckTokenByDeviceIdAsync(string deviceId)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          var query = "SELECT COUNT(*) FROM RefreshTokens WHERE DeviceId = @DeviceId AND Expiration > @CurrentDate";
          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@DeviceId", deviceId);
            command.Parameters.AddWithValue("@CurrentDate", DateTime.UtcNow);

            object? result = await command.ExecuteScalarAsync();
            int count = (result as int?) ?? 0;
            return count > 0;
          }
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.Refresh, e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task<UserDTO?> CheckTokenAsync(string refreshToken, string deviceId)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          var query = @"SELECT u.Id, u.Name, u.Email, u.Role 
                                  FROM RefreshTokens rt
                                  JOIN Users u ON rt.UserId = u.Id
                                  WHERE rt.DeviceId = @DeviceId 
                                        AND rt.Expiration > @CurrentDate 
                                        AND rt.RefreshToken = @Token";

          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@DeviceId", deviceId);
            command.Parameters.AddWithValue("@Token", refreshToken);
            command.Parameters.AddWithValue("@CurrentDate", DateTime.UtcNow);

            using (var reader = await command.ExecuteReaderAsync())
            {
              if (await reader.ReadAsync())
              {
                return new UserDTO
                {
                  Id = reader.GetGuid(0),
                  Name = reader.GetString(1),
                  Email = reader.GetString(2),
                  Role = Enum.Parse<UserRole>(reader.GetString(3))
                };
              }
            }
          }
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.Refresh, e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }

      return null;
    }

    public async Task<RefreshTokenEntity> UpdateRefreshTokenAsync(string oldRefreshToken, string refreshToken, string deviceId)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          var query = @"
                        UPDATE RefreshTokens
                        SET RefreshToken = @Token, Expiration = @Expiration, DeviceId = @DeviceId
                        WHERE RefreshToken = @OldToken";

          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@OldToken", oldRefreshToken);
            command.Parameters.AddWithValue("@Token", refreshToken);
            command.Parameters.AddWithValue("@DeviceId", deviceId);
            command.Parameters.AddWithValue("@Expiration", DateTime.UtcNow.AddDays(30));

            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
              throw new KeyNotFoundException("Refresh token not found for update");
          }

          return new RefreshTokenEntity
          {
            UserId = Guid.Empty,
            RefreshToken = refreshToken,
            DeviceId = deviceId,
            Expiration = DateTime.UtcNow.AddDays(30)
          };
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.Refresh, e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task<RefreshTokenEntity> UpdateRefreshTokenByIdAsync(Guid userId, string refreshToken, string oldDeviceId, string deviceId)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          var query = @"
                        UPDATE RefreshTokens
                        SET RefreshToken = @Token, Expiration = @Expiration, DeviceId = @DeviceId
                        WHERE UserId = @UserId AND DeviceId = @OldDeviceId";

          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Token", refreshToken);
            command.Parameters.AddWithValue("@DeviceId", deviceId);
            command.Parameters.AddWithValue("@OldDeviceId", oldDeviceId);
            command.Parameters.AddWithValue("@Expiration", DateTime.UtcNow.AddDays(30));

            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
              throw new KeyNotFoundException("Refresh token not found for update by id");
          }

          return new RefreshTokenEntity
          {
            UserId = userId,
            RefreshToken = refreshToken,
            DeviceId = deviceId,
            Expiration = DateTime.UtcNow.AddDays(30)
          };
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.Refresh, e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }

    public async Task DeleteRefreshTokenAsync(string refreshToken)
    {
      try
      {
        using (var connection = _connectionFactory.CreateConnection())
        {
          if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

          var query = "DELETE FROM RefreshTokens WHERE RefreshToken = @RefreshToken";
          using (var command = new SqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@RefreshToken", refreshToken);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
              throw new KeyNotFoundException("Refresh token not found for deletion");
          }
        }
      }
      catch (SqlException e)
      {
        throw new DatabaseOperationException(Operations.Refresh, e);
      }
      catch (Exception e)
      {
        throw new Exception("Error getting DB connection", e);
      }
    }
  }
}
