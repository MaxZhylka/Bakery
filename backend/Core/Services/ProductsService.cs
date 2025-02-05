using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace backend.Core.Services 
{
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _productsRepository;
        private readonly IMapper _mapper;

        public ProductsService(IProductRepository productsRepository, IMapper mapper)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            var products = _productsRepository.GetProductsAsync().Result;
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public ProductDTO GetProduct(Guid id)
        {
            var product = _productsRepository.GetProductAsync(id).Result;
            return _mapper.Map<ProductDTO>(product);
        }

        public ProductDTO CreateProduct(ProductDTO product)
        {
            var productEntity = _mapper.Map<ProductEntity>(product);
            productEntity.Id = Guid.NewGuid(); // Генерация GUID
            var createdProduct = _productsRepository.CreateProductAsync(productEntity).Result;
            return _mapper.Map<ProductDTO>(createdProduct);
        }

        public ProductDTO UpdateProduct(Guid id, ProductDTO product)
        {
            var productEntity = _mapper.Map<ProductEntity>(product);
            var updatedProduct = _productsRepository.UpdateProductAsync(id, productEntity).Result;
            return _mapper.Map<ProductDTO>(updatedProduct);
        }

        public ProductDTO DeleteProduct(Guid id)
        {
            var deletedProduct = _productsRepository.DeleteProductAsync(id).Result;
            return _mapper.Map<ProductDTO>(deletedProduct);
        }
    }
}