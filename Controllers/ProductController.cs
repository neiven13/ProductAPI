using Microsoft.AspNetCore.Mvc;
using ProductAPI.Entities;
using ProductAPI.Entities.Enums;
using ProductAPI.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("v1")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost("products")]
        public async Task<IActionResult> AddProductAsync(CreateProduct modelProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.TryParse<ProductType>(modelProduct.Type, true, out var productType))
            {
                return BadRequest("Product type is not valid!");
            }

            var product = new Product
            {
                Name = modelProduct.Name,
                ProductPrice = modelProduct.Price,
                Type = productType
            };

            try
            {
                await _productRepository.AddProductAsync(product);
                return Created($"v1/products/{product.Id}", product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> PutProductAsync(int id, [FromBody] UpdateProduct modelProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            if (!Enum.TryParse<ProductType>(modelProduct.Type, true, out var productType))
                return BadRequest("Product type is not valid!");

            try
            {
                product.Type = productType;
                product.Name = modelProduct.Name;
                product.ProductPrice = modelProduct.Price;

                await _productRepository.UpdateProductAsync(product);
                return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("products/dashboard")]
        public async Task<IActionResult> GetDashboardResultAsync()
        {
            var products = await _productRepository.GetProductsAsync();

            var materialProducts = products.Where(p => p.Type == ProductType.Material);
            var servicoProducts = products.Where(p => p.Type == ProductType.Serviço);

            var materialInfo = new
            {
                Count = materialProducts.Count(),
                AveragePrice = materialProducts.Any() ? materialProducts.Average(p => p.ProductPrice) : 0
            };

            var servicoInfo = new
            {
                Count = servicoProducts.Count(),
                AveragePrice = servicoProducts.Any() ? servicoProducts.Average(p => p.ProductPrice) : 0
            };

            var result = new 
            {
                Material = materialInfo,
                Serviço = servicoInfo
            };

            return Ok(result);
        }
    }
}
