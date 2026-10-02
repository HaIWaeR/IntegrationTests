using Microsoft.AspNetCore.Mvc;
using UsersService.Data;
using UsersService.Models;

namespace UsersService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public List<User> GetAll()
        {
            return UsersStore.Users;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            User? user = UsersStore.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }

            return user;
        }

        [HttpPost]
        public ActionResult<User> Create(User newUser)
        {
            if (newUser.Name == "" || newUser.Email == "")
            {
                return BadRequest("Имя и почта обязательны");
            }

            UsersStore.Users.Add(newUser);

            return newUser;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            User? user = UsersStore.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("Пользователь не найден");
            }

            UsersStore.Users.Remove(user);

            return Ok("Пользователь удалён");
        }
    }
}
