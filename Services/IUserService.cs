using MakeupStore.Models;

namespace MakeupStore.Services
{
    // interfata pentru serviciul de utilizatori
    public interface IUserService
    {
        // autentificam utilizatorul si returnăm datele lui
        Task<User?> LoginAsync(string email, string password);

        // inregistram un utilizator nou
        Task<User?> RegisterAsync(string email, string password, string firstName, string lastName);

        // luam datele unui utilizator dupa id
        Task<User?> GetByIdAsync(int id);

        // verificam daca emailul e deja folosit
        Task<bool> EmailExistsAsync(string email);
    }
}
