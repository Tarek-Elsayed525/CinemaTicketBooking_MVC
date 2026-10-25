using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaTicketBooking.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string MainImage { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("CinemaId")]
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<MovieSubImage> MoviesSubImages { get; set; } = new List<MovieSubImage>();
        public ICollection<MovieActor> MovieActors { get; set; }
    }
}
