using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Capacity must be between 1 and 10")]
        public int Capacity { get; set; }

        [Required]
        public string Type { get; set; }

        public bool IsFree { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceForAdult { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceForChild { get; set; }
    }
}