using MakeupStore.Models;

namespace MakeupStore.Repositories
{
    // interfata pentru repository-ul de favorite
    public interface IFavoriteRepository
    {
        // luam toate favoritele unui utilizator
        Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId);

        // adaugam un produs la favorite
        Task AddAsync(Favorite favorite);

        // stergem un produs din favorite
        Task RemoveAsync(int userId, int productId);

        // verificam daca produsul e deja la favorite
        Task<bool> ExistsAsync(int userId, int productId);

        // luam un favorit dupa user si produs
        Task<Favorite?> GetAsync(int userId, int productId);
    }
}
