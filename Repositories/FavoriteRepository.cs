using Microsoft.EntityFrameworkCore;
using MakeupStore.Data;
using MakeupStore.Models;

namespace MakeupStore.Repositories
{
    // implementarea repository-ului pentru favorite
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        // luam toate favoritele unui utilizator cu produsele incluse
        public async Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId)
        {
            return await _context.Favorites
                .Include(f => f.Product)
                    .ThenInclude(p => p!.Category)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.SavedAt)
                .ToListAsync();
        }

        // adaugam un produs la lista de favorite a utilizatorului
        public async Task AddAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        // stergem produsul din lista de favorite
        public async Task RemoveAsync(int userId, int productId)
        {
            var favorite = await GetAsync(userId, productId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        // verificam daca produsul este deja in lista de favorite
        public async Task<bool> ExistsAsync(int userId, int productId)
        {
            return await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);
        }

        // luam favoritul specific pentru user si produs
        public async Task<Favorite?> GetAsync(int userId, int productId)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
        }
    }
}
