namespace Hotel.ViewModels
{
    public class ClientIndexViewModel
    {
        public IEnumerable<ClientListItemViewModel> Clients { get; set; } = new List<ClientListItemViewModel>();
        public string Search { get; set; }
        public PagerViewModel Pager { get; set; } = new PagerViewModel();
    }

    public class ClientListItemViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdult { get; set; }

        public int ReservationCount { get; set; }
    }
}