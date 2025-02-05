using backend.Core.DTOs;

namespace backend.Core.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProduct(Guid id);
        Task<ProductDTO> CreateProduct(ProductDTO product);
        Task<ProductDTO> UpdateProduct(Guid id, ProductDTO product);
        Task<ProductDTO> DeleteProduct(Guid id);
    }
}