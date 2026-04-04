namespace Hotel.ViewModels
{
    public class ClientReservationsViewModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public IEnumerable<ClientReservationItemViewModel> Reservations { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class ClientReservationItemViewModel
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }

        public PagerViewModel Pager { get; set; } = new PagerViewModel();
    }
}