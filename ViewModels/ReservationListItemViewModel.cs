using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels
{
    public class ReservationListItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        [Display(Name = "Arrival")]
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }

        [Display(Name = "Departure")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [Display(Name = "Total Amount")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalAmount { get; set; }

        public int TotalNights => (DepartureDate - ArrivalDate).Days;
    }
}