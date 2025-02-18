using backend.Core.DTOs;
using backend.Core.Models;
using Microsoft.Data.SqlClient;

namespace backend.Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        Task<PaginatedResult<ProductEntity>> GetProductsAsync(PaginationParameters paginationParameters, SqlConnection? connection = null);

        Task<PaginatedResult<ProductEntity>> GetProductsByValuesAsync(int count, bool directionCount, double price, bool directionPrice, PaginationParameters paginationParams);
        Task<ProductEntity> GetProductAsync(Guid id, SqlConnection? connection = null);
        Task<ProductEntity> CreateProductAsync(ProductEntity product);
        Task<ProductEntity> UpdateProductAsync(Guid id, ProductEntity product);
        Task<ProductEntity> DeleteProductAsync(Guid id);
        Task<IEnumerable<ProductSalesDto>> GetProductSalesAsync();
    }
}