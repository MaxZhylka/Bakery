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

        public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
        {
            SqlConnection connection = _connectionFactory.CreateConnection();
            List<ProductEntity> products = new List<ProductEntity>();

            await connection.OpenAsync();

            string query = "SELECT Id, Name, Price, ProductCount, CreatedAt FROM Products";
            using (var command = new SqlCommand(query, connection))
            {
                using var reader = await command.ExecuteReaderAsync();
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

        public async Task<ProductEntity> GetProductAsync(Guid id)
        {
            SqlConnection connection = _connectionFactory.CreateConnection();
            ProductEntity product = null;

            await connection.OpenAsync();

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

            if (product == null) throw new Exception("Product not found");

            return product;
        }

        public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
        {
            SqlConnection connection = _connectionFactory.CreateConnection();
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

            return await GetProductAsync(product.Id);
        }

        public async Task<ProductEntity> UpdateProductAsync(Guid id, ProductEntity product)
        {
            SqlConnection connection = _connectionFactory.CreateConnection();
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

                await command.ExecuteNonQueryAsync();
            }

            return await GetProductAsync(id);
        }

        public async Task<ProductEntity> DeleteProductAsync(Guid id)
        {
            SqlConnection connection = _connectionFactory.CreateConnection();
            var product = await GetProductAsync(id);
            if (product == null) return null;

            await connection.OpenAsync();

            var query = "DELETE FROM Products WHERE Id = @Id";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }

            return product;
        }
    }
}