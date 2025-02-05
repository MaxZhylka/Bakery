using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace backend.Core.Services 
{
    public class ProductsService(IProductRepository productsRepository, IMapper mapper) : IProductsService
    {
        private readonly IProductRepository _productsRepository = productsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            IEnumerable<ProductEntity> products = await _productsRepository.GetProductsAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProduct(Guid id)
        {
            var product = await _productsRepository.GetProductAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> CreateProduct(ProductDTO product)
        {
            var productEntity = _mapper.Map<ProductEntity>(product);
            productEntity.Id = Guid.NewGuid();
            var createdProduct = await _productsRepository.CreateProductAsync(productEntity);
            return _mapper.Map<ProductDTO>(createdProduct);
        }

        public async Task<ProductDTO> UpdateProduct(Guid id, ProductDTO product)
        {
            var productEntity = _mapper.Map<ProductEntity>(product);
            var updatedProduct = await _productsRepository.UpdateProductAsync(id, productEntity);
            return _mapper.Map<ProductDTO>(updatedProduct);
        }

        public async Task<ProductDTO> DeleteProduct(Guid id)
        {
            var deletedProduct = await _productsRepository.DeleteProductAsync(id);
            return _mapper.Map<ProductDTO>(deletedProduct);
        }
    }
}