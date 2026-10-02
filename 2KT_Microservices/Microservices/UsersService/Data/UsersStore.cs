using UsersService.Models;

namespace UsersService.Data
{
    public static class UsersStore
    {
        public static List<User> Users { get; } = new()
        {
            new User { Name = "Don Hose",   Email = "mars@mail.ru" },
            new User { Name = "Vits Scolieti", Email = "vitas@mail.ru" },
            new User { Name = "Oleg Muchacha",   Email = "arbuz5000w@mail.ru" }
        };
    }
}
