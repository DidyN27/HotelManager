using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Account Active")]
        public bool IsActive { get; set; }


        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; } 

        [Required]
        [Display(Name = "EGN")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "EGN must be exactly 10 digits.")]
        public string EGN { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}