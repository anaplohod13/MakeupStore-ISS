using MakeupStore.Models;
using MakeupStore.Repositories;

namespace MakeupStore.Services
{
    // serviciul cosului de cumparaturi - logica de business pentru cos
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // luam cosul utilizatorului sau il cream daca nu exista
        public async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                // cream un cos nou pentru utilizator
                cart = await _cartRepository.CreateCartAsync(userId);
            }
            return cart;
        }

        // adaugam un produs in cos, sau marim cantitatea daca e deja prezent
        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            // luam sau cream cosul utilizatorului
            var cart = await GetOrCreateCartAsync(userId);

            // verificam daca produsul e deja in cos
            var existingItem = await _cartRepository.GetCartItemByProductAsync(cart.Id, productId);

            if (existingItem != null)
            {
                // marim cantitatea produsului existent
                existingItem.Quantity += quantity;
                await _cartRepository.UpdateItemAsync(existingItem);
            }
            else
            {
                // adaugam un element nou in cos
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _cartRepository.AddItemAsync(cartItem);
            }
        }

        // actualizam cantitatea unui produs din cos
        public async Task UpdateCartItemAsync(int cartItemId, int quantity)
        {
            var item = await _cartRepository.GetCartItemAsync(cartItemId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    // daca cantitatea e 0 sau negativa, stergem produsul din cos
                    await _cartRepository.RemoveItemAsync(cartItemId);
                }
                else
                {
                    item.Quantity = quantity;
                    await _cartRepository.UpdateItemAsync(item);
                }
            }
        }

        // stergem un produs din cos
        public async Task RemoveFromCartAsync(int cartItemId)
        {
            await _cartRepository.RemoveItemAsync(cartItemId);
        }

        // golim cosul dupa plasarea comenzii
        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                await _cartRepository.ClearCartAsync(cart.Id);
            }
        }

        // calculam totalul cosului de cumparaturi
        public async Task<decimal> GetCartTotalAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                return 0;
            }

            // suma totala = suma (pret produs * cantitate) pentru fiecare element
            return cart.CartItems.Sum(ci => ci.Product!.Price * ci.Quantity);
        }
    }
}
