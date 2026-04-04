using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModels
{
    public class UserListItemViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "EGN")]
        public string EGN { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Hired Date")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Left Date")]
        [DataType(DataType.Date)]
        public DateTime? DismissalDate { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}