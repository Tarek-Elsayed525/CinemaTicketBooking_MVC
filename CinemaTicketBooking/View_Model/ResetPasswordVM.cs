using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.View_Model
{
    public class ResetPasswordVM
    {

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password), Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
