using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models
{
    public class LoginModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter your username")]
        [Display(Name ="Username")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string? Password { get; set; }

        public string? UserType { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Email { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
    }
}
