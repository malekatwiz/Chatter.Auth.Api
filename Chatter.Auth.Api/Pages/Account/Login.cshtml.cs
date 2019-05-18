using Chatter.Auth.Api.Models.Account;
using Chatter.Auth.MongoIdentity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Chatter.Auth.Api.Pages.Account
{
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public LoginAccountModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, false);
            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(Input.ReturnUrl))
                {
                    Input.ReturnUrl = "~/";
                }

                return Redirect(Input.ReturnUrl);
            }

            return Unauthorized();
        }
    }
}