using Chatter.Auth.Api.Models.Account;
using Chatter.Auth.MongoIdentity.Entities;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Chatter.Auth.Api.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IEventService _eventService;

        [BindProperty(SupportsGet = true)]
        public LoginAccountModel Input { get; set; }

        public LoginModel(SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService interactionService, UserManager<ApplicationUser> userManager, IEventService eventService)
        {
            _signInManager = signInManager;
            _interactionService = interactionService;
            _userManager = userManager;
            _eventService = eventService;
        }

        public void OnGet(string returnUrl)
        {
            Input = new LoginAccountModel();
            Input.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var context = await _interactionService.GetAuthorizationContextAsync(Input.ReturnUrl);
            //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, false);
            var user = await _userManager.FindByNameAsync(Input.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                await _eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, $"{user.FirstName} {user.LastName}"));
                AuthenticationProperties props = null;
                if (Input.RememberMe)
                {
                    props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    };
                }
                await HttpContext.SignInAsync(user.Id, user.UserName, props);

                if (context != null)
                {
                    if (context.ClientId.Equals("Chatter.App"))
                    {
                        return Redirect(Input.ReturnUrl);
                    }
                }

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