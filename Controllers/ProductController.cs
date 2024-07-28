using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductAPI.Contexts;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Entities;
using Microsoft.EntityFrameworkCore;
using ProductAPI.ViewModels;
using ProductAPI.Entities.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("v1")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ProductContext _context;
        public ProductController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _context.Products.AsNoTracking().ToListAsync();
            return Ok(products);
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
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
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
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

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            if (!Enum.TryParse<ProductType>(modelProduct.Type, true, out var productType))
                return BadRequest("Product type is not valid!");

            try
            {
                product.Type = productType;
                product.Name = modelProduct.Name;
                product.ProductPrice = modelProduct.Price;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
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
            var products = await _context.Products.AsNoTracking().ToListAsync();

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
