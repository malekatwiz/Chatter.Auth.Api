using Chatter.Auth.Api.Models.Account;
using Chatter.Auth.MongoIdentity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Chatter.Auth.Api.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;

        public RegisterModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public RegisterAccountModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                return new JsonResult(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return BadRequest();
        }
    }
}