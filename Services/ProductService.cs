using MakeupStore.Models;
using MakeupStore.Repositories;

namespace MakeupStore.Services
{
    // serviciul de produse - logica de business pentru gestionarea produselor
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // luam toate produsele din baza de date
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        // luam un produs specific dupa id
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        // cautam produse dupa termenul introdus de utilizator
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _productRepository.SearchAsync(searchTerm);
        }

        // filtram produsele dupa categorie si interval de pret
        public async Task<IEnumerable<Product>> FilterProductsAsync(int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            return await _productRepository.FilterAsync(categoryId, minPrice, maxPrice);
        }

        // adaugam un produs nou in sistem (functie de admin)
        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddAsync(product);
        }

        // actualizam datele unui produs existent (functie de admin)
        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        // stergem un produs din sistem (functie de admin)
        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        // luam toate categoriile disponibile
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _productRepository.GetAllCategoriesAsync();
        }

        // verificam daca un produs exista in baza de date
        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _productRepository.ExistsAsync(id);
        }
    }
}
