using MakeupStore.Models;

namespace MakeupStore.Repositories
{
    // interfata pentru repository-ul de produse
    public interface IProductRepository
    {
        // luam toate produsele cu categoriile incluse
        Task<IEnumerable<Product>> GetAllAsync();

        // luam un produs dupa id
        Task<Product?> GetByIdAsync(int id);

        // cautam produse dupa termen de cautare (nume, brand, descriere)
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);

        // filtram produsele dupa categorie si interval de pret
        Task<IEnumerable<Product>> FilterAsync(int? categoryId, decimal? minPrice, decimal? maxPrice);

        // adaugam un produs nou
        Task AddAsync(Product product);

        // actualizam un produs existent
        Task UpdateAsync(Product product);

        // stergem un produs
        Task DeleteAsync(int id);

        // luam toate categoriile disponibile
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        // verificam daca un produs exista
        Task<bool> ExistsAsync(int id);
    }
}
