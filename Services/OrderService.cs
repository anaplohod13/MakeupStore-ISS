using MakeupStore.Models;
using MakeupStore.Repositories;

namespace MakeupStore.Services
{
    // serviciul de comenzi - logica de business pentru plasarea si gestionarea comenzilor
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        // plasam o comanda noua: cream comanda, adaugam elementele, golim cosul
        public async Task<Order?> PlaceOrderAsync(int userId, string shippingAddress)
        {
            // luam cosul utilizatorului cu toate produsele
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            // verificam daca cosul exista si nu e gol
            if (cart == null || !cart.CartItems.Any())
            {
                return null;
            }

            // calculam totalul comenzii
            var totalAmount = cart.CartItems.Sum(ci => ci.Product!.Price * ci.Quantity);

            // cream comanda noua
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                ShippingAddress = shippingAddress,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    // retinem pretul de la momentul comenzii
                    UnitPrice = ci.Product!.Price
                }).ToList()
            };

            // salvam comanda in baza de date
            var savedOrder = await _orderRepository.CreateAsync(order);

            // golim cosul dupa plasarea comenzii
            await _cartRepository.ClearCartAsync(cart.Id);

            return savedOrder;
        }

        // luam toate comenzile unui utilizator
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _orderRepository.GetByUserIdAsync(userId);
        }

        // luam detaliile unei comenzi specifice
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        // luam toate comenzile din sistem (pentru admin)
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }
    }
}
