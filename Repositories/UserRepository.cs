using Microsoft.EntityFrameworkCore;
using MakeupStore.Data;
using MakeupStore.Models;

namespace MakeupStore.Repositories
{
    // implementarea repository-ului pentru utilizatori
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // luam utilizatorul dupa id
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // cautam utilizatorul dupa adresa de email
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        // adaugam un utilizator nou in baza de date
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // actualizam datele utilizatorului in baza de date
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // verificam daca exista un user cu emailul dat (pentru validare la inregistrare)
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
