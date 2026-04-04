using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels
{
    public class ClientCreateEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone must be exactly 10 digits")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Is Adult?")]
        public bool IsAdult { get; set; }
    }
}