using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels
{
    public class ReservationCreateViewModel
    {
        [Required(ErrorMessage = "Please select a room")]
        [Display(Name = "Room")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Check-in date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in Date")]
        public DateTime CheckInDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Check-out date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-out Date")]
        public DateTime CheckOutDate { get; set; } = DateTime.Now.AddDays(1);

        [Display(Name = "Include Breakfast")]
        public bool HasBreakfast { get; set; }

        [Display(Name = "All Inclusive")]
        public bool IsAllInclusive { get; set; }

        [Required(ErrorMessage = "Please select at least one client")]
        [Display(Name = "Clients")]
        public List<int> SelectedClientIds { get; set; } = new List<int>();

        // Lists for the dropdown menus in the View
        public IEnumerable<SelectListItem>? AvailableRooms { get; set; }
        public IEnumerable<SelectListItem>? AllClients { get; set; }
    }
}