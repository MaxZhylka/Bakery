using backend.Core.DTOs;

namespace backend.Core.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<ProductDTO> GetProducts();
        ProductDTO GetProduct(Guid id);
        ProductDTO CreateProduct(ProductDTO product);
        ProductDTO UpdateProduct(Guid id, ProductDTO product);
        ProductDTO DeleteProduct(Guid id);
    }
}