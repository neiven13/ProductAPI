using System.Collections.Generic;
using System.Threading.Tasks;
using ProductAPI.Entities;
using ProductAPI.Entities.Enums;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<List<Product>> GetProductsAsync();
}
