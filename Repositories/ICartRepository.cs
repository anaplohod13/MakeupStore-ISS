using MakeupStore.Models;

namespace MakeupStore.Repositories
{
    // interfata pentru repository-ul cosului de cumparaturi
    public interface ICartRepository
    {
        // luam cosul unui utilizator impreuna cu produsele din el
        Task<Cart?> GetCartByUserIdAsync(int userId);

        // cream un cos nou pentru utilizator
        Task<Cart> CreateCartAsync(int userId);

        // adaugam un produs in cos
        Task AddItemAsync(CartItem cartItem);

        // actualizam cantitatea unui produs din cos
        Task UpdateItemAsync(CartItem cartItem);

        // stergem un produs din cos
        Task RemoveItemAsync(int cartItemId);

        // golim complet cosul (dupa plasarea comenzii)
        Task ClearCartAsync(int cartId);

        // luam un element din cos dupa id
        Task<CartItem?> GetCartItemAsync(int cartItemId);

        // verificam daca un produs e deja in cos
        Task<CartItem?> GetCartItemByProductAsync(int cartId, int productId);
    }
}
