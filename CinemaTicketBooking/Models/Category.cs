using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Models
{
    public class Category
    {
        public int Id { get; set; }

        [MinLength(3, ErrorMessage = "اسم التصنيف يجب ألا يقل عن 3 حروف")]
        [MaxLength(40, ErrorMessage = "اسم التصنيف يجب ألا يزيد عن 40 حرفاً")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }

    }
}
