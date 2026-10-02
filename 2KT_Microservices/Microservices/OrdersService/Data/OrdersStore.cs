using OrdersService.Models;

namespace OrdersService.Data
{
    public static class OrdersStore
    {
        public static List<Order> Orders { get; } = new()
        {
            new Order { Id = 1, UserId = 1, Product = "ЭлектропылеSоS", Quantity = 1 },
            new Order { Id = 2, UserId = 2, Product = "Мышь",    Quantity = 2 },
            new Order { Id = 3, UserId = 3, Product = "Монитор", Quantity = 1 }
        };
    }
}
