using System.ComponentModel.DataAnnotations;

namespace HelpingHands_V2.Models
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter your old password")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter your new password")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm your new password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
