using System.ComponentModel.DataAnnotations;

namespace Chatter.Auth.Api.Models.Account
{
    public class LoginAccountModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
