using MakeupStore.Models;

namespace MakeupStore.Repositories
{
    // interfata pentru repository-ul de comenzi
    public interface IOrderRepository
    {
        // luam o comanda dupa id cu toate detaliile
        Task<Order?> GetByIdAsync(int id);

        // luam toate comenzile unui utilizator
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

        // cream o comanda noua
        Task<Order> CreateAsync(Order order);

        // actualizam statusul comenzii
        Task UpdateAsync(Order order);

        // luam toate comenzile (pentru admin)
        Task<IEnumerable<Order>> GetAllAsync();
    }
}
