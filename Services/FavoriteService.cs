using MakeupStore.Models;
using MakeupStore.Repositories;

namespace MakeupStore.Services
{
    // serviciul de favorite - logica de business pentru lista de dorinte
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        // luam toate produsele favorite ale utilizatorului
        public async Task<IEnumerable<Favorite>> GetUserFavoritesAsync(int userId)
        {
            return await _favoriteRepository.GetByUserIdAsync(userId);
        }

        // adaugam un produs la favorite daca nu e deja acolo
        public async Task AddToFavoritesAsync(int userId, int productId)
        {
            // verificam daca produsul e deja la favorite
            var exists = await _favoriteRepository.ExistsAsync(userId, productId);
            if (!exists)
            {
                var favorite = new Favorite
                {
                    UserId = userId,
                    ProductId = productId,
                    SavedAt = DateTime.UtcNow
                };
                await _favoriteRepository.AddAsync(favorite);
            }
        }

        // stergem un produs din lista de favorite
        public async Task RemoveFromFavoritesAsync(int userId, int productId)
        {
            await _favoriteRepository.RemoveAsync(userId, productId);
        }

        // verificam daca un produs e in lista de favorite a utilizatorului
        public async Task<bool> IsProductFavoriteAsync(int userId, int productId)
        {
            return await _favoriteRepository.ExistsAsync(userId, productId);
        }
    }
}
