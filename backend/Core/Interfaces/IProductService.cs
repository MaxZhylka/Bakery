using backend.Core.DTOs;
using backend.Core.Models;

namespace backend.Core.Interfaces
{
    public interface IProductsService
    {
        Task<PaginatedResult<ProductEntity>> GetProducts(PaginationParameters paginationParameters);

        Task<PaginatedResult<ProductEntity>> GetProductsByValues(int count, bool directionCount, double price, bool directionPrice, PaginationParameters paginationParams);
        Task<ProductDTO> GetProduct(Guid id);
        Task<ProductDTO> CreateProduct(ProductDTO product);
        Task<ProductDTO> UpdateProduct(Guid id, ProductDTO product);
        Task<ProductDTO> DeleteProduct(Guid id);

        Task<IEnumerable<ProductSalesDto>> GetProductSales();
    }
}