namespace UsersService.Models
{
    public static class UsersStore
    {
        public static List<User> Users { get; } = new()
        {
            new User { Id = 1, Name = "Don Hose",   Email = "mars@mail.ru" },
            new User { Id = 2, Name = "Vits Scolieti", Email = "vitas@mail.ru" },
            new User { Id = 3, Name = "Oleg Muchacha",   Email = "arbuz5000w@mail.ru" }
        };
    }
}
