namespace MakeupStore.Models
{
    // modelul pentru utilizatorii aplicatiei
    public class User
    {
        public int Id { get; set; }

        // emailul unic al utilizatorului
        public string Email { get; set; } = string.Empty;

        // parola stocata ca hash
        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // rolul utilizatorului: RegisteredUser sau Admin
        public UserRole Role { get; set; } = UserRole.RegisteredUser;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // relatiile cu alte entitati
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public Cart? Cart { get; set; }
    }
}
