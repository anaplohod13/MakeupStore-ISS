using MakeupStore.Models;

namespace MakeupStore.Services
{
    // interfata pentru serviciul de comenzi
    public interface IOrderService
    {
        // plasam o comanda noua din cosul utilizatorului
        Task<Order?> PlaceOrderAsync(int userId, string shippingAddress);

        // luam istoricul comenzilor unui utilizator
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);

        // luam detaliile unei comenzi
        Task<Order?> GetOrderByIdAsync(int id);

        // luam toate comenzile (admin)
        Task<IEnumerable<Order>> GetAllOrdersAsync();
    }
}
