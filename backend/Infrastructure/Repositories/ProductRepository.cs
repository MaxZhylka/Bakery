using System.Data;
using backend.Core.DTOs;
using backend.Core.Enums;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;
using Core.Exceptions;
using Microsoft.Data.SqlClient;

namespace backend.Infrastructure.Repositories
{
	public class ProductRepository(IDBConnectionFactory connectionFactory) : IProductRepository
	{
		private readonly IDBConnectionFactory _connectionFactory = connectionFactory;

		public async Task<PaginatedResult<ProductEntity>> GetProductsAsync(PaginationParameters paginationParameters, SqlConnection? connection = null)
		{
			try
			{
				using (connection ??= _connectionFactory.CreateConnection())
				{
					if (connection.State != System.Data.ConnectionState.Open)
						await connection.OpenAsync();

					const string countQuery = "SELECT COUNT(*) FROM Products";
					int totalCount;
					using (var countCommand = new SqlCommand(countQuery, connection))
					{
						totalCount = (int)(await countCommand.ExecuteScalarAsync() ?? 0);
					}

					string itemsQuery = @"
                        SELECT Id, Name, ProductCount, Price, CreatedAt
                        FROM Products
                        ORDER BY CreatedAt DESC
                        OFFSET @Offset ROWS
                        FETCH NEXT @PageSize ROWS ONLY
                    ";

					var items = new List<ProductEntity>();

					using (var itemsCommand = new SqlCommand(itemsQuery, connection))
					{
						int offset = paginationParameters.Offset * paginationParameters.Size;
						itemsCommand.Parameters.AddWithValue("@Offset", offset);
						itemsCommand.Parameters.AddWithValue("@PageSize", paginationParameters.Size);

						using var reader = await itemsCommand.ExecuteReaderAsync();
						while (await reader.ReadAsync())
						{
							var product = new ProductEntity
							{
								Id = reader.GetGuid(0),
								Name = reader.GetString(1),
								ProductCount = reader.GetInt32(2),
								Price = reader.GetDecimal(3),
								CreatedAt = reader.GetDateTime(4)
							};
							items.Add(product);
						}
					}

					return new PaginatedResult<ProductEntity>
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

		public async Task<PaginatedResult<ProductEntity>> GetProductsByValuesAsync(
				int? count, bool? directionCount,
				double? price, bool? directionPrice,
				PaginationParameters paginationParams)
		{

			try
			{
				using (var connection = _connectionFactory.CreateConnection())
				{
					if (connection.State != ConnectionState.Open)
						await connection.OpenAsync();

					var conditions = new List<string>();
					var countParameters = new List<SqlParameter>();

					if (count.HasValue && directionCount.HasValue)
					{
						conditions.Add(directionCount.Value ? "ProductCount > @Count" : "ProductCount < @Count");
						countParameters.Add(new SqlParameter("@Count", SqlDbType.Int) { Value = count.Value });
					}

					if (price.HasValue && directionPrice.HasValue)
					{
						conditions.Add(directionPrice.Value ? "Price > @Price" : "Price < @Price");
						countParameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Value = price.Value });
					}

					string whereClause = conditions.Count != 0 ? "WHERE " + string.Join(" AND ", conditions) : "";

					string countQuery = $"SELECT COUNT(*) FROM Products {whereClause}";
					int totalCount;
					using (var countCommand = new SqlCommand(countQuery, connection))
					{
						countCommand.Parameters.AddRange(countParameters.ToArray());
						totalCount = (int)(await countCommand.ExecuteScalarAsync() ?? 0);
					}

					string itemsQuery = $@"
                SELECT Id, Name, ProductCount, Price, CreatedAt
                FROM Products
                {whereClause}
                ORDER BY CreatedAt DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY
            ";

					var itemsParameters = new List<SqlParameter>();

					foreach (var param in countParameters)
					{
						if (param.ParameterName == "@Count" && count.HasValue)
						{
							itemsParameters.Add(new SqlParameter("@Count", SqlDbType.Int) { Value = count.Value });
						}
						else if (param.ParameterName == "@Price" && price.HasValue)
						{
							itemsParameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Value = price.Value });
						}
					}
					itemsParameters.Add(new SqlParameter("@Offset", SqlDbType.Int)
					{
						Value = paginationParams.Offset * paginationParams.Size
					});
					itemsParameters.Add(new SqlParameter("@PageSize", SqlDbType.Int)
					{
						Value = paginationParams.Size
					});

					var items = new List<ProductEntity>();
					using (var itemsCommand = new SqlCommand(itemsQuery, connection))
					{
						itemsCommand.Parameters.AddRange(itemsParameters.ToArray());

						using var reader = await itemsCommand.ExecuteReaderAsync();
						while (await reader.ReadAsync())
						{
							var product = new ProductEntity
							{
								Id = reader.GetGuid(0),
								Name = reader.GetString(1),
								ProductCount = reader.GetInt32(2),
								Price = reader.GetDecimal(3),
								CreatedAt = reader.GetDateTime(4)
							};
							items.Add(product);
						}
					}

					return new PaginatedResult<ProductEntity>
					{
						Total = totalCount,
						Data = items
					};
				}
			}
			catch (SqlException e)
			{
				Console.WriteLine(e);
				throw new DatabaseOperationException(Operations.GetProducts, e);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw new Exception($"Error getting DB connection: {e.Message}", e);
			}
		}


		public async Task<ProductEntity> GetProductAsync(Guid id, SqlConnection? connection = null)
		{
			try
			{
				using (connection ??= _connectionFactory.CreateConnection())
				{
					if (connection.State != System.Data.ConnectionState.Open)
						await connection.OpenAsync();

					ProductEntity? product = null;

					var query = "SELECT Id, Name, Price, ProductCount, CreatedAt FROM Products WHERE Id = @Id";
					using (var command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@Id", id);

						using var reader = await command.ExecuteReaderAsync();
						if (await reader.ReadAsync())
						{
							product = new ProductEntity
							{
								Id = reader.GetGuid(0),
								Name = reader.GetString(1),
								Price = reader.GetDecimal(2),
								ProductCount = reader.GetInt32(3),
								CreatedAt = reader.GetDateTime(4)
							};
						}
					}

					if (product == null)
						throw new ProductNotFoundException(id);

					return product;
				}
			}
			catch (SqlException e)
			{
				throw new DatabaseOperationException(Operations.GetProduct, e);
			}
			catch (Exception e)
			{
				throw new Exception($"Error getting DB connection{e}");
			}
		}

		public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
		{
			try
			{
				using var connection = _connectionFactory.CreateConnection();
				if (connection.State != ConnectionState.Open)
					await connection.OpenAsync();

				var query = @"
                    INSERT INTO Products (Id, Name, Price, ProductCount, CreatedAt) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Id, @Name, @Price, @ProductCount, @CreatedAt)";

				using (var command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Id", product.Id);
					command.Parameters.AddWithValue("@Name", product.Name);
					command.Parameters.AddWithValue("@Price", product.Price);
					command.Parameters.AddWithValue("@ProductCount", product.ProductCount);
					command.Parameters.AddWithValue("@CreatedAt", product.CreatedAt);

					await command.ExecuteScalarAsync();
				}

				return await GetProductAsync(product.Id, connection);
			}
			catch (SqlException e)
			{
				throw new DatabaseOperationException(Operations.CreateProduct, e);
			}
			catch (Exception e)
			{
				throw new Exception($"Error getting DB connection{e}");
			}
		}

		public async Task<ProductEntity> UpdateProductAsync(Guid id, ProductEntity product)
		{
			try
			{
				using var connection = _connectionFactory.CreateConnection();
				if (connection.State != System.Data.ConnectionState.Open)
					await connection.OpenAsync();

				var query = @"
                    UPDATE Products 
                    SET Name = @Name,
                        Price = @Price,
                        ProductCount = @ProductCount,
                        CreatedAt = @CreatedAt
                    WHERE Id = @Id";

				using (var command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Id", id);
					command.Parameters.AddWithValue("@Name", product.Name);
					command.Parameters.AddWithValue("@Price", product.Price);
					command.Parameters.AddWithValue("@ProductCount", product.ProductCount);
					command.Parameters.AddWithValue("@CreatedAt", product.CreatedAt);

					var rowsAffected = await command.ExecuteNonQueryAsync();
					if (rowsAffected == 0)
						throw new KeyNotFoundException($"Product with ID {id} was not found for update");
				}

				return await GetProductAsync(id, connection);
			}
			catch (SqlException e)
			{
				throw new DatabaseOperationException(Operations.UpdateProduct, e);
			}
			catch (Exception e)
			{
				throw new Exception($"Error getting DB connection{e}");
			}
		}

		public async Task<ProductEntity> DeleteProductAsync(Guid id)
		{
			try
			{
				using var connection = _connectionFactory.CreateConnection();
				if (connection.State != ConnectionState.Open)
					await connection.OpenAsync();

				var query = @"
            DELETE FROM Products 
            OUTPUT DELETED.Id, DELETED.Name, DELETED.Price, DELETED.ProductCount
            WHERE Id = @Id";

				using (var command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Id", id);

					using var reader = await command.ExecuteReaderAsync();
					if (await reader.ReadAsync())
					{
						return new ProductEntity
						{
							Id = reader.GetGuid(0),
							Name = reader.GetString(1),
							Price = reader.GetDecimal(2),
							ProductCount = reader.GetInt32(3)
						};
					}
					else
					{
						throw new KeyNotFoundException($"Product with ID {id} was not found for deletion");
					}
				}
			}
			catch (SqlException e)
			{
				throw new DatabaseOperationException(Operations.DeleteProduct, e);
			}
			catch (Exception e)
			{
				throw new Exception($"Error getting DB connection {e}");
			}
		}


		public async Task<IEnumerable<ProductSalesDto>> GetProductSalesAsync()
		{
			try
			{
				using (var connection = _connectionFactory.CreateConnection())
				{
					if (connection.State != ConnectionState.Open)
						await connection.OpenAsync();

					string query = @"
                SELECT 
                    p.Name AS ProductName,
                    COALESCE(SUM(o.ProductCount), 0) AS TotalSold
                FROM Orders o
                RIGHT JOIN Products p ON o.ProductId = p.Id
                GROUP BY p.Name
                ORDER BY TotalSold ASC;
            ";

					var result = new List<ProductSalesDto>();

					using (var command = new SqlCommand(query, connection))
					{
						using var reader = await command.ExecuteReaderAsync();
						while (await reader.ReadAsync())
						{
							var dto = new ProductSalesDto
							{
								ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
								TotalSold = reader.GetInt32(reader.GetOrdinal("TotalSold"))
							};
							result.Add(dto);
						}
					}

					return result;
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

	}
}
