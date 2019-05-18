using System.ComponentModel.DataAnnotations;

namespace Chatter.Auth.Api.Models.Account
{
    public class RegisterAccountModel
    {
        [Required, Display(Name = "First Name"), MaxLength(50)]
        public string FirstName { get; set; }

        [Required, Display(Name = "First Name"), MaxLength(50)]
        public string LastName { get; set; }

        [Required, Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmedPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
