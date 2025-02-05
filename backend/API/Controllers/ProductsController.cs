using Microsoft.AspNetCore.Mvc;
using backend.Core.Interfaces;
using backend.Core.DTOs;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsService productsService) : ControllerBase
    {
        private readonly IProductsService _productsService = productsService;

        [HttpGet]
        public IEnumerable<ProductDTO> Get()
        {
            return _productsService.GetProducts();
        }

        [HttpGet("{id}")]
        public ProductDTO Get(Guid id)
        {
            return _productsService.GetProduct(id);
        }

        [HttpPost]
        public ProductDTO Post([FromBody] ProductDTO product)
        {
            return _productsService.CreateProduct(product);
        }

        [HttpPut("{id}")]
        public ProductDTO Put(Guid id, [FromBody] ProductDTO product)
        {
            return _productsService.UpdateProduct(id, product);
        }

        [HttpDelete("{id}")]
        public ProductDTO Delete(Guid id)
        {
            return _productsService.DeleteProduct(id);
        }
    }
}