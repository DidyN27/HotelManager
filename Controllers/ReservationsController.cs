using Microsoft.AspNetCore.Mvc;
using Hotel.Services;
using Hotel.Models;
using Hotel.ViewModels; // Трябва ти за новия ViewModel
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hotel.Data; // Добави това, за да вижда базата директно

[Authorize]
public class ReservationsController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IRoomService _roomService;
    private readonly ApplicationDbContext _context; // Ползваме контекста директно за клиентите

    public ReservationsController(IReservationService resService, IRoomService roomService, ApplicationDbContext context)
    {
        _reservationService = resService;
        _roomService = roomService;
        _context = context;
    }

    // 1. Поправен Index - махаме параметрите, за да съвпадне със сървиса
    public IActionResult Index()
    {
        var reservations = _reservationService.GetAll();
        return View(reservations);
    }

    // 2. Поправен Create (GET)
    public IActionResult Create()
    {
        var viewModel = new ReservationCreateViewModel
        {
            // Използваме GetFilteredRooms (както е в твоя сървис)
            AvailableRooms = _roomService.GetFilteredRooms(null, null, true)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"Room No.{r.RoomNumber}"
                }),

            // Взимаме клиентите директно от базата
            AllClients = _context.Clients.Select(c => new SelectListItem
            {
                Value = c.ClientID.ToString(),
                Text = $"{c.FirstName} {c.LastName}"
            })
        };

        return View(viewModel);
    }
}