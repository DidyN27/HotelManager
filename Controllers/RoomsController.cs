using Hotel.Models;
using Hotel.Services;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [Authorize(Roles = "Admin,Employee")] 
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public IActionResult Index(int? capacity, string type, bool? isFree)
        {
            var rooms = _roomService.GetFilteredRooms(capacity, type, isFree);
            return View(rooms);
        }

        [Authorize(Roles = "Admin")] 
        public IActionResult Create()
        {
            return View(new RoomCreateEditViewModel { IsFree = true });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomCreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var room = new Room
                {
                    RoomNumber = model.RoomNumber,
                    Capacity = model.Capacity,
                    Type = model.Type.ToString(),
                    IsFree = model.IsFree,
                    PriceForAdult = model.PriceAdult,
                    PriceForChild = model.PriceChild
                };

                _roomService.AddRoom(room);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var room = _roomService.GetRoomById(id);
            if (room == null) return NotFound();

            var model = new RoomCreateEditViewModel
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Capacity = room.Capacity,
                IsFree = room.IsFree,
                PriceAdult = room.PriceForAdult,
                PriceChild = room.PriceForChild,
                Type = Enum.Parse<RoomTypes>(room.Type)
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RoomCreateEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var room = new Room
                {
                    Id = model.Id,
                    RoomNumber = model.RoomNumber,
                    Capacity = model.Capacity,
                    Type = model.Type.ToString(),
                    IsFree = model.IsFree,
                    PriceForAdult = model.PriceAdult,
                    PriceForChild = model.PriceChild
                };

                _roomService.UpdateRoom(room);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var room = _roomService.GetRoomById(id);
            if (room == null) return NotFound();

            var model = new RoomListItemViewModel
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _roomService.DeleteRoom(id);
            return RedirectToAction(nameof(Index));
        }
    }
}