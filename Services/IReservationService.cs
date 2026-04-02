using Hotel.Models;
namespace Hotel.Services
{
    public interface IReservationService
    {
        void CreateReservation(Reservation reservation);
        // Â IReservationService.cs
        IEnumerable<Reservation> GetAll();
        Reservation GetById(int id);
        void Delete(int id);

        decimal CalculateTotalAmount(Reservation reservation);

    }
}