using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaTicketBooking.Models
{
    public class MovieSubImage
    {
        public int Id { get; set; }
        public string SubImage { get; set; }
        [ForeignKey("MovieId")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
