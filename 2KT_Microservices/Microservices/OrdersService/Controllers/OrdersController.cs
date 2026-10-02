using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Data;
using OrdersService.Models;

namespace OrdersService.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IHttpClientFactory httpClientFactory) : ControllerBase
{
    private readonly HttpClient _usersClient = httpClientFactory.CreateClient("UsersService");

    [HttpGet]
    public List<Order> GetAll()
    {
        return OrdersStore.Orders;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        Order? order = OrdersStore.Orders.FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound("Заказ не найден");
        }

        Stopwatch stopwatch = Stopwatch.StartNew();
        HttpResponseMessage response;

        try
        {
            response = await _usersClient.GetAsync($"api/users/{order.UserId}");
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "Сервис пользователей недоступен");
        }
        catch (TaskCanceledException)
        {
            return StatusCode(503, "Сервис пользователей не ответил вовремя");
        }

        stopwatch.Stop();
        Response.Headers["X-Users-Service-Time"] = stopwatch.ElapsedMilliseconds.ToString();

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound("Пользователь этого заказа не найден");
        }

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(502, "Сервис пользователей вернул ошибку");
        }

        UserDto? user = await response.Content.ReadFromJsonAsync<UserDto>();

        return Ok(new { order, user });
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create(Order newOrder)
    {
        if (newOrder.Product == "" || newOrder.Quantity <= 0)
        {
            return BadRequest("Укажите товар и количество больше 0");
        }

        HttpResponseMessage response;

        try
        {
            response = await _usersClient.GetAsync($"api/users/{newOrder.UserId}");
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "Сервис пользователей недоступен");
        }
        catch (TaskCanceledException)
        {
            return StatusCode(503, "Сервис пользователей не ответил вовремя");
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return BadRequest("Пользователь с таким id не существует");
        }

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(502, "Сервис пользователей вернул ошибку");
        }

        OrdersStore.Orders.Add(newOrder);

        return newOrder;
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Order? order = OrdersStore.Orders.FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound("Заказ не найден");
        }

        OrdersStore.Orders.Remove(order);

        return Ok("Заказ удалён");
    }
}