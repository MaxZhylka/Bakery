using System.Data;
using backend.Core.DTOs;
using backend.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;

namespace backend.Infrastructure.Repositories
{
	public class ReportRepository(IDBConnectionFactory connectionFactory) : IReportRepository
	{
		private readonly IDBConnectionFactory _connectionFactory = connectionFactory;

		public async Task<IEnumerable<ProductReportDto>> GetReportByProductAsync()
		{
			try
			{
				using var connection = _connectionFactory.CreateConnection();
				if (connection.State != ConnectionState.Open)
					await connection.OpenAsync();

				const string query = @"
                    SELECT 
                        p.Id AS ProductId,
                        p.Name AS ProductName,
                        SUM(o.ProductCount) AS TotalSold,
                        SUM(o.Price) AS TotalRevenue
                    FROM Orders o
                    JOIN Products p ON o.ProductId = p.Id
                    GROUP BY p.Id, p.Name
                    ORDER BY TotalSold DESC";

				using var command = new SqlCommand(query, connection);
				var results = new List<ProductReportDto>();

				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					results.Add(new ProductReportDto
					{
						ProductId = reader.GetGuid(0),
						ProductName = reader.GetString(1),
						TotalSold = reader.GetInt32(2),
						TotalRevenue = reader.GetDecimal(3)
					});
				}

				return results;
			}
			catch (SqlException e)
			{
				throw new Exception($"Database error while getting product report: {e.Message}", e);
			}
		}

		public async Task<IEnumerable<CustomerReportDto>> GetReportByCustomerAsync()
		{
			try
			{
				using var connection = _connectionFactory.CreateConnection();
				if (connection.State != ConnectionState.Open)
					await connection.OpenAsync();

				const string query = @"
                    SELECT 
                        u.Id AS CustomerId,
                        u.Name AS CustomerName,
                        COUNT(o.Id) AS TotalOrders,
                        SUM(o.Price) AS TotalSpent
                    FROM Users u
                    JOIN Orders o ON u.Id = o.CustomerId
                    GROUP BY u.Id, u.Name
                    ORDER BY TotalSpent DESC";

				using var command = new SqlCommand(query, connection);
				var results = new List<CustomerReportDto>();

				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					results.Add(new CustomerReportDto
					{
						CustomerId = reader.GetGuid(0),
						CustomerName = reader.GetString(1),
						TotalOrders = reader.GetInt32(2),
						TotalSpent = reader.GetDecimal(3)
					});
				}

				return results;
			}
			catch (SqlException e)
			{
				throw new Exception($"Database error while getting customer report: {e.Message}", e);
			}
		}

		public async Task<IEnumerable<OrderReportDto>> GetAllOrdersReportAsync()
		{
			try
			{
				using var connection = _connectionFactory.CreateConnection();
				if (connection.State != ConnectionState.Open)
					await connection.OpenAsync();

				const string query = @"
                    SELECT 
                        o.Id AS OrderId,
                        u.Name AS CustomerName,
                        p.Name AS ProductName,
                        o.ProductCount,
                        o.Price,
                        o.CreatedAt
                    FROM Orders o
                    JOIN Users u ON o.CustomerId = u.Id
                    JOIN Products p ON o.ProductId = p.Id
                    ORDER BY o.CreatedAt DESC";

				using var command = new SqlCommand(query, connection);
				var results = new List<OrderReportDto>();

				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					results.Add(new OrderReportDto
					{
						OrderId = reader.GetGuid(0),
						CustomerName = reader.GetString(1),
						ProductName = reader.GetString(2),
						ProductCount = reader.GetInt32(3),
						Price = reader.GetDecimal(4),
						CreatedAt = reader.GetDateTime(5)
					});
				}

				return results;
			}
			catch (SqlException e)
			{
				throw new Exception($"Database error while getting all orders report: {e.Message}", e);
			}
		}

		public async Task<IEnumerable<OrderTrendsByCustomerDto>> GetOrderTrendsByCustomerAsync()
		{
			try
			{
				using var connection = _connectionFactory.CreateConnection();
				if (connection.State != ConnectionState.Open)
					await connection.OpenAsync();

				const string query = @"
                    SELECT 
                        u.Name AS CustomerName,
                        FORMAT(o.CreatedAt, 'yyyy-MM') AS OrderMonth,
                        COUNT(o.Id) AS TotalOrders
                    FROM Orders o
                    JOIN Users u ON o.CustomerId = u.Id
                    GROUP BY u.Name, FORMAT(o.CreatedAt, 'yyyy-MM')
                    ORDER BY OrderMonth DESC";

				using var command = new SqlCommand(query, connection);
				var results = new List<OrderTrendsByCustomerDto>();

				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					results.Add(new OrderTrendsByCustomerDto
					{
						CustomerName = reader.GetString(0),
						OrderMonth = reader.GetString(1),
						TotalOrders = reader.GetInt32(2)
					});
				}

				return results;
			}
			catch (SqlException e)
			{
				throw new Exception($"Database error while getting order trends by customer: {e.Message}", e);
			}
		}

		public async Task<IEnumerable<OrderTrendsByProductDto>> GetOrderTrendsByProductAsync()
		{
			try
			{
				using var connection = _connectionFactory.CreateConnection();
				if (connection.State != ConnectionState.Open)
					await connection.OpenAsync();

				const string query = @"
                    SELECT 
                        p.Name AS ProductName,
                        FORMAT(o.CreatedAt, 'yyyy-MM') AS OrderMonth,
                        SUM(o.ProductCount) AS TotalSold
                    FROM Orders o
                    JOIN Products p ON o.ProductId = p.Id
                    GROUP BY p.Name, FORMAT(o.CreatedAt, 'yyyy-MM')
                    ORDER BY OrderMonth DESC";

				using var command = new SqlCommand(query, connection);
				var results = new List<OrderTrendsByProductDto>();

				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					results.Add(new OrderTrendsByProductDto
					{
						ProductName = reader.GetString(0),
						OrderMonth = reader.GetString(1),
						TotalSold = reader.GetInt32(2)
					});
				}

				return results;
			}
			catch (SqlException e)
			{
				throw new Exception($"Database error while getting order trends by product: {e.Message}", e);
			}
		}
	}
}
