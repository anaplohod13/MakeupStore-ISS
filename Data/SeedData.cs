using MakeupStore.Models;
using System.Security.Cryptography;
using System.Text;

namespace MakeupStore.Data
{
    // clasa pentru popularea bazei de date cu date initiale
    public static class SeedData
    {
        // metoda care verifica daca baza de date e goala si adauga date initiale
        public static void Initialize(AppDbContext context)
        {
            // daca exista deja categorii, nu mai adaugam date
            if (context.Categories.Any())
            {
                return;
            }

            // adaugam categoriile de produse cosmetice
            var categories = new List<Category>
            {
                new Category { Name = "Foundation" },
                new Category { Name = "Lipstick" },
                new Category { Name = "Eyeshadow" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // luam categoriile pentru a le folosi la produse
            var foundation = context.Categories.First(c => c.Name == "Foundation");
            var lipstick = context.Categories.First(c => c.Name == "Lipstick");
            var eyeshadow = context.Categories.First(c => c.Name == "Eyeshadow");

            // adaugam produsele initiale in baza de date
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Luminous Silk Foundation",
                    Description = "A lightweight, buildable coverage foundation that gives your skin a natural luminous finish. Suitable for all skin types.",
                    Price = 325.00m,
                    Brand = "Giorgio Armani",
                    StockQuantity = 50,
                    ImageUrl = "/images/foundation1.jpg",
                    CategoryId = foundation.Id
                },
                new Product
                {
                    Name = "Pro Filt'r Soft Matte Foundation",
                    Description = "Long-wearing, full-coverage foundation with a soft matte finish. Oil-free formula that controls shine all day.",
                    Price = 210.00m,
                    Brand = "Fenty Beauty",
                    StockQuantity = 75,
                    ImageUrl = "/images/foundation2.jpg",
                    CategoryId = foundation.Id
                },
                new Product
                {
                    Name = "Double Wear Foundation",
                    Description = "24-hour wear, waterproof foundation that stays fresh all day and all night. Medium to full buildable coverage.",
                    Price = 260.00m,
                    Brand = "Estee Lauder",
                    StockQuantity = 60,
                    ImageUrl = "/images/foundation3.jpg",
                    CategoryId = foundation.Id
                },
                new Product
                {
                    Name = "Ruby Woo Lipstick",
                    Description = "Iconic retro matte lipstick in a timeless true red shade. Long-lasting formula with intense color payoff.",
                    Price = 110.00m,
                    Brand = "MAC Cosmetics",
                    StockQuantity = 100,
                    ImageUrl = "/images/lipstick1.jpg",
                    CategoryId = lipstick.Id
                },
                new Product
                {
                    Name = "Velvet Matte Lipstick",
                    Description = "Rich, creamy matte lipstick with a velvety smooth texture. Available in a range of bold and neutral shades.",
                    Price = 90.00m,
                    Brand = "NYX Professional",
                    StockQuantity = 120,
                    ImageUrl = "/images/lipstick2.jpg",
                    CategoryId = lipstick.Id
                },
                new Product
                {
                    Name = "Rouge Dior",
                    Description = "Iconic Dior lip color with a couture finish. Enriched with a hydrating floral lip care formula.",
                    Price = 245.00m,
                    Brand = "Dior",
                    StockQuantity = 45,
                    ImageUrl = "/images/lipstick3.jpg",
                    CategoryId = lipstick.Id
                },
                new Product
                {
                    Name = "Naked Eyeshadow Palette",
                    Description = "12 versatile neutral shades perfect for everyday and smoky eye looks. Includes matte, shimmer, and satin finishes.",
                    Price = 270.00m,
                    Brand = "Urban Decay",
                    StockQuantity = 35,
                    ImageUrl = "/images/eyeshadow1.jpg",
                    CategoryId = eyeshadow.Id
                },
                new Product
                {
                    Name = "Rose Gold Eyeshadow Palette",
                    Description = "A stunning collection of rose gold, pink, and copper shades. Perfect for creating glamorous evening looks.",
                    Price = 240.00m,
                    Brand = "Too Faced",
                    StockQuantity = 40,
                    ImageUrl = "/images/eyeshadow2.jpg",
                    CategoryId = eyeshadow.Id
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            // adaugam utilizatorii initiali (admin si user obisnuit)
            var users = new List<User>
            {
                new User
                {
                    Email = "admin@makeup.com",
                    PasswordHash = HashPassword("Admin123"),
                    FirstName = "Admin",
                    LastName = "Store",
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Email = "user@makeup.com",
                    PasswordHash = HashPassword("User123"),
                    FirstName = "Jane",
                    LastName = "Doe",
                    Role = UserRole.RegisteredUser,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // metoda pentru a calcula hash-ul parolei folosind SHA256
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
