namespace MakeupStore.Models
{
    // enum pentru rolurile utilizatorilor in aplicatie
    public enum UserRole
    {
        RegisteredUser,
        Admin
    }

    // enum pentru statusul unei comenzi
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered
    }
}
