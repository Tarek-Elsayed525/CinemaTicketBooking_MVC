using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaTicketBooking.Models
{
    public class ApplicationUserOTP
    {
        public int id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string OTP { get; set; }
        public bool IsValid { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime CreatedAt { get; set; }



        public ApplicationUserOTP(string applicationUserId, string otp)
        {
            ApplicationUserId = applicationUserId;
            OTP = otp;
            IsValid = true;
            ValidTo = DateTime.UtcNow.AddMinutes(10);
            CreatedAt = DateTime.UtcNow;
        }
        public ApplicationUserOTP()
        {
        }


    }
}
