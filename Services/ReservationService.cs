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

            if (res.Clients != null && res.Clients.Any())
            {
                foreach (var client in res.Clients)
                {
                    total += (client.IsAdult ? room.PriceForAdult : room.PriceForChild) * nights;
                }

                decimal extraRatePerPerson = 0;
                if (res.IsAllInclusive)
                {
                    extraRatePerPerson = 20; 
                }
                else if (res.HasBreakfast)
                {
                    extraRatePerPerson = 10; 
                }

                total += (extraRatePerPerson * res.Clients.Count * nights);
            }

            return total;
        }

        
        public void CreateReservation(Reservation res)
        {
            res.TotalAmount = CalculateTotalAmount(res);

            var room = _db.Rooms.Find(res.RoomId);
            if (room != null)
            {
                room.IsFree = false;
            }

            _db.Reservations.Add(res);
            _db.SaveChanges();
        }

        
        public IEnumerable<Reservation> GetAll()
        {
            return _db.Reservations
                .Include(r => r.ReservedRoom)
                .Include(r => r.Clients)
                .Include(r => r.User)
                .ToList();
        }

        
        public Reservation GetById(int id)
        {
            return _db.Reservations
                .Include(r => r.ReservedRoom)
                .Include(r => r.Clients)
                .Include(r => r.User)
                .FirstOrDefault(r => r.ReservationId == id);
        }

        
        public void Delete(int id)
        {
            var res = _db.Reservations.Find(id);
            if (res != null)
            {
                var room = _db.Rooms.Find(res.RoomId);
                if (room != null)
                {
                    room.IsFree = true;
                }

                _db.Reservations.Remove(res);
                _db.SaveChanges();
            }
        }
    }
}