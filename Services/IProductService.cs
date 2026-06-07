using MakeupStore.Models;

namespace MakeupStore.Services
{
    // interfata pentru serviciul de produse
    public interface IProductService
    {
        // luam toate produsele
        Task<IEnumerable<Product>> GetAllProductsAsync();

        // luam un produs dupa id
        Task<Product?> GetProductByIdAsync(int id);

        // cautam produse dupa termen
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);

        // filtram produsele dupa criterii
        Task<IEnumerable<Product>> FilterProductsAsync(int? categoryId, decimal? minPrice, decimal? maxPrice);

        // adaugam un produs nou (admin)
        Task AddProductAsync(Product product);

        // actualizam un produs (admin)
        Task UpdateProductAsync(Product product);

        // stergem un produs (admin)
        Task DeleteProductAsync(int id);

        // luam toate categoriile pentru filtrare si formulare
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        // verificam daca produsul exista
        Task<bool> ProductExistsAsync(int id);
    }
}
