using OrdersService.Models;

namespace OrdersService.Data
{
    public static class OrdersStore
    {
        public static List<Order> Orders { get; } = new()
        {
            new Order { UserId = 1, Product = "ЭлектропылеSоS", Quantity = 1 },
            new Order { UserId = 2, Product = "Мышь",    Quantity = 2 },
            new Order { UserId = 3, Product = "Монитор", Quantity = 1 }
        };
    }
}
