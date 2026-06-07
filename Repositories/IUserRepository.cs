using MakeupStore.Models;

namespace MakeupStore.Repositories
{
    // interfata pentru repository-ul de utilizatori
    public interface IUserRepository
    {
        // cauta un utilizator dupa id
        Task<User?> GetByIdAsync(int id);

        // cauta un utilizator dupa email
        Task<User?> GetByEmailAsync(string email);

        // adauga un utilizator nou in baza de date
        Task AddAsync(User user);

        // actualizeaza datele unui utilizator
        Task UpdateAsync(User user);

        // verifica daca exista un utilizator cu emailul dat
        Task<bool> ExistsByEmailAsync(string email);
    }
}
