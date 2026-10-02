namespace UsersService.Models
{
    public class User
    {
        private static int _nextId = 1;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public User()
        {
            Id = _nextId;
            _nextId++;
        }

    }
}
