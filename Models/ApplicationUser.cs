using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required] public string FirstName { get; set; }
        [Required] public string MiddleName { get; set; }
        [Required] public string LastName { get; set; }
        [Required, StringLength(10)] public string? EGN { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? DismissalDate { get; set; }
    }
}