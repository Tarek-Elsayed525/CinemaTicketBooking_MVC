using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.View_Model
{
    public class LoginVM
    {
        public string UserNameorEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
