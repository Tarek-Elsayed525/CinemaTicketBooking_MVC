using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.View_Model
{
    public class MovieFormVM
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Minimum length is 3 characters")]
        [MaxLength(40, ErrorMessage = "Maximum length is 40 characters")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public IFormFile? MainImage { get; set; }

        public List<IFormFile>? SubImages { get; set; }

        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool Status { get; set; }


        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Cinema is required")]
        public int CinemaId { get; set; }


        public List<int> SelectedActorIds { get; set; } = new List<int>();
        public List<int> ExistingSubImageIds { get; set; } = new List<int>();
        public string? ExistingMainImage { get; set; }
        public List<CinemaTicketBooking.Models.MovieSubImage>? ExistingSubImages { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IEnumerable<SelectListItem>? Cinemas { get; set; }
        public IEnumerable<SelectListItem>? Actors { get; set; }

    }
}
