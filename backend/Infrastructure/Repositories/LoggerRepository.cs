using System.Data;
using backend.Core.DTOs;
using backend.Core.Enums;
using backend.Core.Interfaces;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;
using Core.Exceptions;
using Microsoft.Data.SqlClient;

namespace backend.Infrastructure.Repositories
{
    public class LoggerRepository(IDBConnectionFactory connectionFactory) : ILoggerRepository
    {
        private readonly IDBConnectionFactory connectionFactory = connectionFactory;

        public async Task<UserActionLog> SaveLogAsync(UserActionLog log)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                const string query = @"
                    INSERT INTO UserActionLogs (Id, UserId, Operation, Details, Timestamp) 
                    OUTPUT INSERTED.Id, INSERTED.UserId, INSERTED.Operation, INSERTED.Details, INSERTED.Timestamp
                    VALUES (@Id, @UserId, @Operation, @Details, @Timestamp)";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", log.Id);
                command.Parameters.AddWithValue("@UserId", log.UserId);
                command.Parameters.AddWithValue("@Operation", log.Operation.ToString());
                command.Parameters.AddWithValue("@Details", log.Details);
                command.Parameters.AddWithValue("@Timestamp", log.Timestamp);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new UserActionLog
                    {
                        Id = reader.GetGuid(0),
                        UserId = reader.GetGuid(1),
                        Operation = Enum.Parse<Operations>(reader.GetString(2)),
                        Details = reader.GetString(3),
                        Timestamp = reader.GetDateTime(4)
                    };
                }

                throw new LogCreationException();
            }
            catch (SqlException e)
            {
                throw new DatabaseOperationException(Operations.SaveLog, e);
            }
            catch (Exception e)
            {
                throw new Exception("Error getting DB connection", e);
            }
        }

        public async Task<UserActionDTO[]> GetLogsByUserIdAsync(Guid userId)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                const string query = @"
                    SELECT 
                        l.Id,
                        l.UserId,
                        u.UserName,
                        u.Email,
                        l.Operation,
                        l.Details,
                        l.Timestamp
                    FROM UserActionLogs l
                    JOIN Users u ON l.UserId = u.Id
                    WHERE l.UserId = @UserId
                    ORDER BY l.Timestamp DESC";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                var results = new List<UserActionDTO>();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var dto = new UserActionDTO
                    {
                        Id = reader.GetGuid(0),
                        UserId = reader.GetGuid(1),
                        UserName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Operation = Enum.Parse<Operations>(reader.GetString(4)),
                        Details = reader.GetString(5),
                        Timestamp = reader.GetDateTime(6)
                    };
                    results.Add(dto);
                }

                return results.ToArray();
            }
            catch (SqlException e)
            {
                throw new DatabaseOperationException(Operations.GetLogsByUserID, e);
            }
            catch (Exception e)
            {
                throw new Exception("Error getting DB connection", e);
            }
        }

        public async Task<UserActionDTO[]> GetAllLogsAsync(PaginationParameters parameters)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                const string query = @"
                    SELECT 
                        l.Id,
                        l.UserId,
                        u.UserName,
                        u.Email,
                        l.Operation,
                        l.Details,
                        l.Timestamp
                    FROM UserActionLogs l
                    JOIN Users u ON l.UserId = u.Id
                    ORDER BY l.Timestamp DESC
                    OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Offset", (parameters.From - 1) * parameters.Size);
                command.Parameters.AddWithValue("@Limit", parameters.Size);

                var results = new List<UserActionDTO>();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var dto = new UserActionDTO
                    {
                        Id = reader.GetGuid(0),
                        UserId = reader.GetGuid(1),
                        UserName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Operation = Enum.Parse<Operations>(reader.GetString(4)),
                        Details = reader.GetString(5),
                        Timestamp = reader.GetDateTime(6)
                    };
                    results.Add(dto);
                }

                return results.ToArray();
            }
            catch (SqlException e)
            {
                throw new DatabaseOperationException(Operations.GetAllLogs, e);
            }
            catch (Exception e)
            {
                throw new Exception("Error getting DB connection", e);
            }
        }

        public async Task<UserActionDTO> GetLogByIdAsync(Guid logId)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                const string query = @"
                    SELECT 
                        l.Id,
                        l.UserId,
                        u.UserName,
                        u.Email,
                        l.Operation,
                        l.Details,
                        l.Timestamp
                    FROM UserActionLogs l
                    JOIN Users u ON l.UserId = u.Id
                    WHERE l.Id = @Id";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", logId);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new UserActionDTO
                    {
                        Id = reader.GetGuid(0),
                        UserId = reader.GetGuid(1),
                        UserName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Operation = Enum.Parse<Operations>(reader.GetString(4)),
                        Details = reader.GetString(5),
                        Timestamp = reader.GetDateTime(6)
                    };
                }

                throw new LogNotFoundException(logId);
            }
            catch (SqlException e)
            {
                throw new DatabaseOperationException(Operations.GetLog, e);
            }
            catch (Exception e)
            {
                throw new Exception("Error getting DB connection", e);
            }
        }
    }
}
