using Hotel.Data;
using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, int pg = 1)
        {
            var clientsQuery = _context.Clients.Include(c => c.Reservations).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                clientsQuery = clientsQuery.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search) || c.Email.Contains(search) || c.PhoneNumber.Contains(search));
            }

            int pageSize = 10;
            int recsCount = await clientsQuery.CountAsync();

            var pager = new PagerViewModel(recsCount, pg, pageSize);

            int recSkip = (pager.CurrentPage - 1) * pageSize;

            var data = await clientsQuery
                .Skip(recSkip)
                .Take(pager.PageSize)
                .Select(c => new Hotel.ViewModels.ClientListItemViewModel
                {
                    Id = c.ClientID,
                    FullName = c.FirstName + " " + c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    IsAdult = c.IsAdult,
                    ReservationCount = c.Reservations.Count()
                })
                .ToListAsync();

            var viewModel = new ClientIndexViewModel
            {
                Clients = data,
                Pager = pager,
                Search = search 
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .Include(c => c.Reservations)
                .FirstOrDefaultAsync(m => m.ClientID == id);

            if (client == null) return NotFound();

            return View(client);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ClientCreateEditViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = new Client
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    IsAdult = model.IsAdult
                };

                _context.Add(client);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            var model = new ClientCreateEditViewModel
            {
                Id = client.ClientID,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                IsAdult = client.IsAdult
            };
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientCreateEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var client = await _context.Clients.FindAsync(id);
                if (client == null) return NotFound();

                client.FirstName = model.FirstName;
                client.LastName = model.LastName;
                client.Email = model.Email;
                client.PhoneNumber = model.PhoneNumber;
                client.IsAdult = model.IsAdult;

                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientID == id);

            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Reservations) 
                .FirstOrDefaultAsync(m => m.ClientID == id);

            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientID == id);
        }


        public async Task<IActionResult> Reservations(int? id, int pg = 1)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .Include(c => c.Reservations)
                .FirstOrDefaultAsync(m => m.ClientID == id);

            if (client == null) return NotFound();

            // 1. Get the total count for THIS client's reservations
            int totalItems = client.Reservations.Count();
            int pageSize = 5; // History pages are usually shorter

            var pager = new PagerViewModel(totalItems, pg, pageSize);
            int recSkip = (pager.CurrentPage - 1) * pageSize;

            // 2. Filter and Page the data
            var reservationData = client.Reservations
                .OrderByDescending(r => r.CheckInDate) // Show newest first
                .Skip(recSkip)
                .Take(pager.PageSize)
                .Select(r => new ClientReservationItemViewModel
                {
                    Id = r.ReservationId,
                    RoomNumber = r.ReservedRoom?.RoomNumber ?? "N/A",
                    CheckInDate = r.CheckInDate,
                    CheckOutDate = r.CheckOutDate,
                    TotalAmount = r.TotalAmount
                }).ToList();

            var viewModel = new ClientReservationsViewModel
            {
                ClientId = client.ClientID,
                ClientName = $"{client.FirstName} {client.LastName}",
                Reservations = reservationData,
                Pager = pager 
            };

            return View(viewModel);
        }
    }
}