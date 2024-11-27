using System.ComponentModel.DataAnnotations;

namespace CarRentalService.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public int Year { get; set; }
        [Required]
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
