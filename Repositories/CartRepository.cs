using Microsoft.EntityFrameworkCore;
using MakeupStore.Data;
using MakeupStore.Models;

namespace MakeupStore.Repositories
{
    // implementarea repository-ului pentru cosul de cumparaturi
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        // luam cosul utilizatorului cu toate produsele incluse
        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p!.Category)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // cream un cos nou pentru utilizator
        public async Task<Cart> CreateCartAsync(int userId)
        {
            var cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        // adaugam un produs in cosul de cumparaturi
        public async Task AddItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        // actualizam cantitatea unui produs din cos
        public async Task UpdateItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        // stergem un produs din cos
        public async Task RemoveItemAsync(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        // stergem toate produsele din cos dupa plasarea comenzii
        public async Task ClearCartAsync(int cartId)
        {
            var items = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        // luam un element din cos dupa id-ul lui
        public async Task<CartItem?> GetCartItemAsync(int cartItemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }

        // verificam daca produsul e deja in cos pentru a actualiza cantitatea
        public async Task<CartItem?> GetCartItemByProductAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }
    }
}
