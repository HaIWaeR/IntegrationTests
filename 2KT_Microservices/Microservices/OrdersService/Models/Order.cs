namespace OrdersService.Models
{
    public class Order
    {
        private static int _nextId = 1;
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public Order()
        {
            Id = _nextId;
            _nextId++;
        }
    }
}
