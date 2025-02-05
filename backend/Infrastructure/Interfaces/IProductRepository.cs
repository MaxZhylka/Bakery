using backend.Core.Models;

namespace backend.Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductEntity>> GetProductsAsync();
        Task<ProductEntity> GetProductAsync(Guid id);
        Task<ProductEntity> CreateProductAsync(ProductEntity product);
        Task<ProductEntity> UpdateProductAsync(Guid id, ProductEntity product);
        Task<ProductEntity> DeleteProductAsync(Guid id);
    }
}