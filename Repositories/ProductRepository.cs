using Microsoft.EntityFrameworkCore;
using MakeupStore.Data;
using MakeupStore.Models;

namespace MakeupStore.Repositories
{
    // implementarea repository-ului pentru produse
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // luam toate produsele impreuna cu categoria lor
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        // luam un produs dupa id, cu categoria inclusa
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // cautam produse dupa termen in nume, brand sau descriere
        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            var term = searchTerm.ToLower();
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(term)
                         || p.Brand.ToLower().Contains(term)
                         || p.Description.ToLower().Contains(term))
                .ToListAsync();
        }

        // filtram produsele dupa categorie si interval de pret
        public async Task<IEnumerable<Product>> FilterAsync(int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            // aplicam filtrul de categorie daca e specificat
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // aplicam filtrul de pret minim daca e specificat
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            // aplicam filtrul de pret maxim daca e specificat
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            return await query.ToListAsync();
        }

        // adaugam un produs nou in baza de date
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        // actualizam datele unui produs existent
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // stergem un produs dupa id
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        // luam toate categoriile disponibile in baza de date
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        // verificam daca un produs exista in baza de date
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }
    }
}
