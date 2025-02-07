using Microsoft.AspNetCore.Mvc;
using backend.Core.Interfaces;
using backend.Core.DTOs;
using System.Threading.Tasks;
using Core.Attributes;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsService productsService) : ControllerBase
    {
        private readonly IProductsService _productsService = productsService;

        [ErrorHandler]
        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> Get()
        {
            return await _productsService.GetProducts();
        }

        [ErrorHandler]
        [HttpGet("{id}")]
        public async Task<ProductDTO> Get(Guid id)
        {
            return await _productsService.GetProduct(id);
        }

        [ErrorHandler]
        [HttpPost]
        public async Task<ProductDTO> Post([FromBody] ProductDTO product)
        {
            return await _productsService.CreateProduct(product);
        }

        [ErrorHandler]
        [HttpPut("{id}")]
        public async Task<ProductDTO> Put(Guid id, [FromBody] ProductDTO product)
        {
            return await _productsService.UpdateProduct(id, product);
        }

        [ErrorHandler]
        [HttpDelete("{id}")]
        public async Task<ProductDTO> Delete(Guid id)
        {
            return await _productsService.DeleteProduct(id);
        }
    }
}