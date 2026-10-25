
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace CinemaTicketBooking.Models
{
    [PrimaryKey(nameof(MovieId), nameof(ActorId))]
    public class MovieActor
    {
        [ForeignKey("MovieId")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [ForeignKey("ActorId")]
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
