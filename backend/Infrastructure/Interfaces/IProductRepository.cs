using backend.Core.Models;
using Microsoft.Data.SqlClient;

namespace backend.Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductEntity>> GetProductsAsync(SqlConnection? connection = null);
        Task<ProductEntity> GetProductAsync(Guid id, SqlConnection? connection = null);
        Task<ProductEntity> CreateProductAsync(ProductEntity product);
        Task<ProductEntity> UpdateProductAsync(Guid id, ProductEntity product);
        Task<ProductEntity> DeleteProductAsync(Guid id);
    }
}