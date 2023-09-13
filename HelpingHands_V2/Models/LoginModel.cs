using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name ="Username")]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string? Password { get; set; }

        public string? UserType { get; set; }
    }
}
