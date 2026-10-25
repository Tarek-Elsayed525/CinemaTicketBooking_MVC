using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}
