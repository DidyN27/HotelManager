using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize] // Трябва да си логнат, за да виждаш каквото и да е
public class RoomsController : Controller
{
    // Този метод е достъпен за всички служители
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin")] // Само Админ може да вижда формата за добавяне
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult Create(Room room)
    {
        // Логика за запис
        return RedirectToAction(nameof(Index));
    }
}