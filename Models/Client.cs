using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone must be exactly 10 digits")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsAdult { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
    }
}