using MakeupStore.Models;

namespace MakeupStore.Services
{
    // interfata pentru serviciul cosului de cumparaturi
    public interface ICartService
    {
        // luam sau cream cosul utilizatorului
        Task<Cart> GetOrCreateCartAsync(int userId);

        // adaugam un produs in cos (sau marim cantitatea daca e deja acolo)
        Task AddToCartAsync(int userId, int productId, int quantity);

        // actualizam cantitatea unui produs din cos
        Task UpdateCartItemAsync(int cartItemId, int quantity);

        // stergem un produs din cos
        Task RemoveFromCartAsync(int cartItemId);

        // golim cosul complet
        Task ClearCartAsync(int userId);

        // calculam totalul cosului
        Task<decimal> GetCartTotalAsync(int userId);
    }
}
