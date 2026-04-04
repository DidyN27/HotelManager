using Hotel.Models;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels
{
    public class RoomCreateEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Capacity must be between 1 and 10")]
        public int Capacity { get; set; }

        [Required]
        public RoomTypes Type { get; set; }

        [Display(Name = "Is Available?")]
        public bool IsFree { get; set; }

        [Required]
        [Display(Name = "Price for Adult ($)")]
        [Range(0, 10000)]
        public decimal PriceAdult { get; set; }

        [Required]
        [Display(Name = "Price for Child ($)")]
        [Range(0, 10000)]
        public decimal PriceChild { get; set; }
    }
}