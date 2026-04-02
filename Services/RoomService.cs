using Hotel.Data;
using Hotel.Models;
using Hotel.Services;
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

        // Връща всички стаи за Index изгледа
        public IEnumerable<Room> GetAllRooms()
        {
            return _db.Rooms.ToList();
        }

        // Логика за добавяне - ползва се от Админа [cite: 42]
        public void AddRoom(Room room)
        {
            _db.Rooms.Add(room);
            _db.SaveChanges();
        }

        // Филтриране по капацитет, тип и заетост [cite: 42]
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
            throw new NotImplementedException();
        }

        public void DeleteRoom(int id)
        {
            throw new NotImplementedException();
        }

        public Room GetRoomById(int id)
        {
            throw new NotImplementedException();
        }
    }
}