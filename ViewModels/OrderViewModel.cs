using MakeupStore.Models;

namespace MakeupStore.ViewModels
{
    // view model pentru un element dintr-o comanda
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // calculam totalul pentru acest element
        public decimal TotalPrice => UnitPrice * Quantity;
    }

    // view model pentru afisarea detaliilor unei comenzi
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;

        // lista produselor din comanda
        public IEnumerable<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();

        // textul statusului pentru afisare
        public string StatusDisplayText => Status.ToString();
    }

    // view model pentru lista comenzilor unui utilizator
    public class OrderHistoryViewModel
    {
        public IEnumerable<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
    }
}
