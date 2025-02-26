using Microsoft.Data.SqlClient;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;
using backend.Core.Enums;
using Core.Exceptions;
using backend.Core.DTOs;

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

        public async Task<PaginatedResult<ProductEntity>> GetProductsByValuesAsync(int count, bool directionCount, double price, bool directionPrice, PaginationParameters paginationParams)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        await connection.OpenAsync();


                    var conditions = new List<string>
                    {
                        directionCount
                        ? "Count > @Count"
                        : "Count < @Count",
                        directionPrice
                        ? "Price > @Price"
                        : "Price < @Price"
                    };

                    string whereClause = string.Join(" AND ", conditions);

                    string countQuery = $"SELECT COUNT(*) FROM Products WHERE {whereClause}";
                    int totalCount;
                    using (var countCommand = new SqlCommand(countQuery, connection))
                    {
                        countCommand.Parameters.AddWithValue("@Count", count);
                        countCommand.Parameters.AddWithValue("@Price", price);

                        totalCount = (int)(await countCommand.ExecuteScalarAsync() ?? 0);
                    }

                    string itemsQuery = $@"
                        SELECT Id, Name, Count, Price, CreatedAt
                        FROM Products
                        WHERE {whereClause}
                        ORDER BY CreatedAt DESC
                        OFFSET @Offset ROWS
                        FETCH NEXT @PageSize ROWS ONLY
                    ";

                    var items = new List<ProductEntity>();

                    using (var itemsCommand = new SqlCommand(itemsQuery, connection))
                    {
                        itemsCommand.Parameters.AddWithValue("@Count", count);
                        itemsCommand.Parameters.AddWithValue("@Price", price);

                        int offset = paginationParams.Offset * paginationParams.Size;
                        itemsCommand.Parameters.AddWithValue("@Offset", offset);
                        itemsCommand.Parameters.AddWithValue("@PageSize", paginationParams.Size);

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
                if (connection.State != System.Data.ConnectionState.Open)
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
        if (connection.State != System.Data.ConnectionState.Open)
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
                    if (connection.State != System.Data.ConnectionState.Open)
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
