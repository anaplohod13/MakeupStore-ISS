using System.Security.Cryptography;
using System.Text;
using MakeupStore.Models;
using MakeupStore.Repositories;

namespace MakeupStore.Services
{
    // serviciul de utilizatori - gestioneaza autentificarea si inregistrarea
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // verificam credentialele si returnam utilizatorul daca sunt corecte
        public async Task<User?> LoginAsync(string email, string password)
        {
            // cautam utilizatorul dupa email
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            // verificam daca parola introdusa corespunde hash-ului stocat
            var passwordHash = HashPassword(password);
            if (user.PasswordHash != passwordHash)
            {
                return null;
            }

            return user;
        }

        // inregistram un utilizator nou dupa validare
        public async Task<User?> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            // verificam daca emailul e deja folosit
            if (await _userRepository.ExistsByEmailAsync(email))
            {
                return null;
            }

            // cream utilizatorul nou cu parola hash-uita
            var user = new User
            {
                Email = email,
                PasswordHash = HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                Role = UserRole.RegisteredUser,
                CreatedAt = DateTime.UtcNow
            };

            // salvam utilizatorul in baza de date
            await _userRepository.AddAsync(user);
            return user;
        }

        // luam datele unui utilizator dupa id
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        // verificam daca emailul e deja inregistrat in sistem
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.ExistsByEmailAsync(email);
        }

        // calculam hash-ul SHA256 al parolei
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
