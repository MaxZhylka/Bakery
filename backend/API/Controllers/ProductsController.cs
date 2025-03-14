using System.Threading.Tasks;
using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Core.Models;
using Core.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsService productsService) : ControllerBase
    {
        private readonly IProductsService _productsService = productsService;

        [ErrorHandler]
        [Authorize(Roles = "Admin,Manager,User")]
        [HttpGet]
        public async Task<PaginatedResult<ProductEntity>> GetProducts([FromQuery] PaginationParameters paginationParams)
        {
            return await _productsService.GetProducts(paginationParams);
        }

        [ErrorHandler]
        [Authorize(Roles = "Admin,Manager,User")]
        [HttpGet("GetByValues")]
        public async Task<PaginatedResult<ProductEntity>> GetProductsByValues([FromQuery] ProductFilterDto productFilter)
        {
            return await _productsService.GetProductsByValues(
                productFilter.Count,
                productFilter.DirectionCount,
                productFilter.Price,
                productFilter.DirectionPrice,
                new PaginationParameters { Size = productFilter.Size, Offset = productFilter.Offset });
        }

        [ErrorHandler]
        [Authorize(Roles = "Admin,Manager,User")]
        [HttpGet("{id}")]
        public async Task<ProductDTO> GetProduct(Guid id)
        {
            return await _productsService.GetProduct(id);
        }

        [ErrorHandler]
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<ProductDTO> CreateProduct([FromBody] ProductDTO product)
        {
            return await _productsService.CreateProduct(product);
        }

        [ErrorHandler]
        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
        public async Task<ProductDTO> UpdateProduct(Guid id, [FromBody] ProductDTO product)
        {
            return await _productsService.UpdateProduct(id, product);
        }

        [ErrorHandler]
        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{id}")]
        public async Task<ProductDTO> DeleteProduct(Guid id)
        {
            return await _productsService.DeleteProduct(id);
        }

        [ErrorHandler]
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("Sales")]
        public async Task<IEnumerable<ProductSalesDto>> GetProductSales()
        {
            return await _productsService.GetProductSales();
        }
    }
}