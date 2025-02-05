using Microsoft.Data.SqlClient;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;

namespace backend.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDBConnectionFactory _connectionFactory;

        public ProductRepository(IDBConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsAsync(SqlConnection? connection = null)
        {
            try
            {
                using (connection ??= _connectionFactory.CreateConnection())
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        await connection.OpenAsync();

                    List<ProductEntity> products = [];

                    string query = "SELECT Id, Name, Price, ProductCount, CreatedAt FROM Products";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new ProductEntity
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2),
                                ProductCount = reader.GetInt32(3),
                                CreatedAt = reader.GetDateTime(4)
                            });
                        }
                    }

                    return products;
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
                        throw new KeyNotFoundException($"Product with ID {id} was not found");

                    return product;
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
                throw new Exception("Error inserting product into database", e);
            }
            catch (Exception e)
            {
                throw new Exception("Error getting DB connection", e);
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
                throw new Exception("Error updating product in database", e);
            }
            catch (Exception e)
            {
                throw new Exception("Error getting DB connection", e);
            }
        }

        public async Task<ProductEntity> DeleteProductAsync(Guid id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                if (connection.State != System.Data.ConnectionState.Open)
                    await connection.OpenAsync();

                var product = await GetProductAsync(id, connection);

                var query = "DELETE FROM Products WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"Product with ID {id} was not found for deletion");
                }

                return product;
            }
            catch (SqlException e)
            {
                throw new Exception("Error deleting product from database", e);
            }
            catch (Exception e)
            {
                throw new Exception("Error getting DB connection", e);
            }
        }
    }
}
