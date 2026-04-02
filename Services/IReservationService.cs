using Hotel.Models;
namespace Hotel.Services
{
    public interface IReservationService
    {
        void CreateReservation(Reservation reservation);
        IEnumerable<Reservation> GetAll(int page, int pageSize);
        Reservation GetById(int id);
        void Delete(int id);

        decimal CalculateTotalAmount(Reservation reservation);

    }
}