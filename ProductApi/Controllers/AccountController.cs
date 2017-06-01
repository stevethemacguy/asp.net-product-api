using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Entities;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("api/account")]
    public class AccountController: Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //        private readonly RoleManager<MyIdentityRole> roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //Should take in a user object with an email and password
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User { UserName = user.Email, Email = user.Email};
                //Pass the newly created user and their password to the _userManager to create it in the SQL DB.
                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                    //Now that a User was successfully created, go ahead and sign them in.
                    //isPersistent is whether to store a session cookie (if it's false) or a more permanent cookie if it's true.

                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    //_logger.LogInformation(3, "User created a new account with password.");

                    return Ok();
                    //If you were using a view this is how you would redirect
                    //return RedirectToLocal(returnUrl);
                }
                else
                {
                    //There was a problem, so include the errors in the model state that's returned by the register action
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    //.Net's default
                    return Unauthorized();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //private IActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
    }
}
