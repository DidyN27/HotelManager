using Hotel.Data;
using Hotel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotel.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _db;

        public ReservationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public decimal CalculateTotalAmount(Reservation res)
        {

            var room = _db.Rooms.Find(res.RoomId);
            if (room == null) return 0;


            int nights = (res.CheckOutDate - res.CheckInDate).Days;
            if (nights <= 0) nights = 1;

            decimal total = 0;

            if (res.Clients != null)
            {
                foreach (var client in res.Clients)
                {
                    if (client.IsAdult)
                        total += room.PriceForAdult * nights;
                    else
                        total += room.PriceForChild * nights;
                }

                int clientCount = res.Clients.Count;
                if (res.IsAllInclusive)
                    total += (20 * clientCount * nights);
                else if (res.HasBreakfast)
                    total += (10 * clientCount * nights);
            }

            return total;
        }

        public void CreateReservation(Reservation res)
        {
            res.TotalAmount = CalculateTotalAmount(res);
            _db.Reservations.Add(res);

            var room = _db.Rooms.Find(res.RoomId);
            if (room != null) room.IsFree = false;

            _db.SaveChanges();
        }

        public IEnumerable<Reservation> GetAll(int page, int pageSize)
        {
            return _db.Reservations
                .Include(r => r.ReservedRoom)
                .Include(r => r.Clients)
                .Skip((page - 1) * pageSize) 
                .Take(pageSize)              
                .ToList();
        }
        public Reservation GetById(int id)
            => _db.Reservations.Include(r => r.Clients).FirstOrDefault(r => r.ReservationId == id);

        public void Delete(int id)
        {
            var res = _db.Reservations.Find(id);
            if (res != null)
            {
                var room = _db.Rooms.Find(res.RoomId);
                if (room != null) room.IsFree = true;

                _db.Reservations.Remove(res);
                _db.SaveChanges();
            }
        }
    }
}