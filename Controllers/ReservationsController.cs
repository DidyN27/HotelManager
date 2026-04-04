using Microsoft.AspNetCore.Mvc;
using Hotel.Services;
using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hotel.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

//[Authorize(Roles = "Admin,Employee")]
public class ReservationsController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IRoomService _roomService;
    private readonly ApplicationDbContext _context;

    public ReservationsController(IReservationService resService, IRoomService roomService, ApplicationDbContext context)
    {
        _reservationService = resService;
        _roomService = roomService;
        _context = context;
    }

    public IActionResult Index()
    {
        var reservations = _reservationService.GetAll();
        return View(reservations);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new ReservationCreateViewModel
        {
            AvailableRooms = _roomService.GetFilteredRooms(null, null, true)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = $"Room No.{r.RoomNumber} (Cap: {r.Capacity})"
                }),

            AllClients = _context.Clients.Select(c => new SelectListItem
            {
                Value = c.ClientID.ToString(),
                Text = $"{c.FirstName} {c.LastName} ({c.Email})"
            })
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservation = new Reservation
            {
                RoomId = model.RoomId,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                HasBreakfast = model.HasBreakfast,
                IsAllInclusive = model.IsAllInclusive,
                UserId = userId,
                Clients = await _context.Clients
                    .Where(c => model.SelectedClientIds.Contains(c.ClientID))
                    .ToListAsync()
            };

            _reservationService.CreateReservation(reservation);
            return RedirectToAction(nameof(Index));
        }

        model.AvailableRooms = _roomService.GetFilteredRooms(null, null, true)
            .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.RoomNumber });
        model.AllClients = _context.Clients
            .Select(c => new SelectListItem { Value = c.ClientID.ToString(), Text = $"{c.FirstName} {c.LastName}" });

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _reservationService.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var reservation = _reservationService.GetById(id);

        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }
}