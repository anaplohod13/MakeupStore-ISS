using Microsoft.EntityFrameworkCore;
using MakeupStore.Data;
using MakeupStore.Repositories;
using MakeupStore.Services;

// cream builder-ul aplicatiei
var builder = WebApplication.CreateBuilder(args);

// adaugam serviciile pentru MVC cu suport pentru Razor Views
builder.Services.AddControllersWithViews();

// configurare baza de date SQLite cu Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// configurare sesiune pentru autentificare
builder.Services.AddSession(options =>
{
    // sesiunea expira dupa 60 de minute
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// adaugam suport pentru HttpContext in servicii
builder.Services.AddHttpContextAccessor();

// inregistram repository-urile in containerul DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();

// inregistram serviciile de business logic in containerul DI
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();

// construim aplicatia
var app = builder.Build();

// configurare pipeline pentru cereri HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// activam redirectionarea HTTPS
app.UseHttpsRedirection();

// servim fisierele statice (CSS, JS, imagini)
app.UseStaticFiles();

// activam rutarea
app.UseRouting();

// activam sesiunea - trebuie inainte de configurarea rutelor
app.UseSession();

// configurare autorizare
app.UseAuthorization();

// configurare rutele implicite ale aplicatiei
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// initializam baza de date si adaugam datele initiale
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // aplicam migrarile si cream baza de date daca nu exista
        context.Database.EnsureCreated();

        // populam baza de date cu date initiale
        SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating/seeding the database.");
    }
}

// pornim aplicatia
app.Run();
