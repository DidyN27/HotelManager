using Hotel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ReservationsController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IRoomService _roomService;
    private readonly IUserService _clientService;

    public ReservationsController(IReservationService resService, IRoomService roomService, IClientService clientService)
    {
        _reservationService = resService;
        _roomService = roomService;
        _clientService = clientService;
    }

    // Списък с резервации
    public IActionResult Index(int page = 1, int pageSize = 10)
    {
        // Извикваме метода с параметрите за странициране
        var reservations = _reservationService.GetAll(page, pageSize);

        // Тези два реда трябва да сочат към правилните методи в твоите сървиси
        ViewBag.Rooms = _roomService.GetAllRooms(); // или GetFilteredRooms()
        ViewBag.Clients = _clientService.GetAll(); // увери се, че имаш _clientService

        return View(reservations);
    }

    // Създаване на резервация
    public IActionResult Create()
    {
        // Трябва да заредиш свободните стаи и наличните клиенти в Dropdown менюта
        ViewBag.Rooms = _roomService.GetAvailableRooms();
        ViewBag.Clients = _clientService.GetAllClients();
        return View();
    }

    [HttpPost]
    public IActionResult Create(ReservationViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Тук бизнес слоят изчислява сумата автоматично преди записа
            _reservationService.CreateReservation(model, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // Детайли - показва пълна информация и списък с настанените клиенти
    public IActionResult Details(int id)
    {
        var details = _reservationService.GetDetails(id);
        if (details == null) return NotFound();
        return View(details);
    }
}