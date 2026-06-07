using MakeupStore.Models;

namespace MakeupStore.Services
{
    // interfata pentru serviciul de favorite
    public interface IFavoriteService
    {
        // luam toate favoritele unui utilizator
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(int userId);

        // adaugam un produs la favorite
        Task AddToFavoritesAsync(int userId, int productId);

        // stergem un produs din favorite
        Task RemoveFromFavoritesAsync(int userId, int productId);

        // verificam daca produsul e deja la favorite
        Task<bool> IsProductFavoriteAsync(int userId, int productId);
    }
}
