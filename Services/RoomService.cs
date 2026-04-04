using Hotel.Data;
using Hotel.Models;
using Hotel.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Hotel.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _db;

        public RoomService(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _db.Rooms.ToList();
        }

        
        public void AddRoom(Room room)
        {
            _db.Rooms.Add(room);
            _db.SaveChanges();
        }

        public IEnumerable<Room> GetFilteredRooms(int? capacity, string type, bool? isFree)
        {
            var query = _db.Rooms.AsQueryable();

            if (capacity.HasValue)
                query = query.Where(r => r.Capacity == capacity);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(r => r.Type == type);

            if (isFree.HasValue)
                query = query.Where(r => r.IsFree == isFree);

            return query.ToList();
        }

        public void UpdateRoom(Room room)
        {
            _db.Rooms.Update(room);
            _db.SaveChanges();
        }

        public void DeleteRoom(int id)
        {
            var room = _db.Rooms.Find(id);
            if (room != null)
            {
                _db.Rooms.Remove(room);
                _db.SaveChanges();
            }
        }

        public Room GetRoomById(int id)
        {
            return _db.Rooms.Find(id);
        }

        public void RefreshRoomStatuses()
        {
            var expiredRooms = _db.Rooms
                .Where(r => !r.IsFree && !_db.Reservations
                    .Any(res => res.RoomId == r.Id && res.CheckOutDate > DateTime.Now))
                .ToList();

            foreach (var room in expiredRooms)
            {
                room.IsFree = true;
            }
            _db.SaveChanges();
        }
    }
}