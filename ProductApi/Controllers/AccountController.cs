using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using ProductApi.Entities;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("api/account")]
    public class AccountController: Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager; //should this be <user>?
        private ILogger<AccountController> _logger;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpGet("getuserroles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserRoles()
        {
            //Get the current User
            //System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            //bool IsAdmin = currentUser.IsInRole("Admin");
            //var id = _userManager.GetUserId(User); // Get user id:

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Get the roles for the user
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(roles);
            }
            return Ok("unauthorized");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //Should take in a user object with an email and password
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User {
                    UserName = user.Email,
                    Email = user.Email,
                };
                //Pass the newly created user and their password to the _userManager to create it in the SQL DB.
                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    //If the roles do not yet exist in the Database, then create the roles in the DB. This is a one-time creation
                    //If the roles already exists in the DB, then this won't do anything.
                    if (!await _roleManager.RoleExistsAsync("basic"))
                        await _roleManager.CreateAsync(new IdentityRole("basic")); //IdentityRole
                    if (!await _roleManager.RoleExistsAsync("admin"))
                        await _roleManager.CreateAsync(new IdentityRole("admin"));

                    //A claim is just a key/value pair. I'm adding a custom "roleType" claim to make it easier for the FE to use the roleType.
                    await _userManager.AddClaimAsync(newUser, new Claim("roleType", user.RoleType));

                    //Add the role to the new user
                    await _userManager.AddToRoleAsync(newUser, user.Role);

                    //If you want to send an email registration, see the sample .Net Core authorization project.

                    //Now that a User was successfully created, go ahead and sign them in.
                    //isPersistent is whether to store a session cookie (if it's false) or a more permanent cookie if it's true.
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");

                    return Ok();
                    //return Ok(newUser);
                    //If you were using a view this is how you would redirect
                    //return RedirectToLocal(returnUrl);
                }
                else
                {
                    //There was a problem, so include the errors in the model state that's returned by the register action
                    foreach (var error in result.Errors)
                    {
                        //The string errorMessages actually names the object "errorMessages" so you can easily access it on the front-end
                        ModelState.AddModelError("errorMessages", error.Description);
                    }

                    return BadRequest(ModelState);
                    //.Net's default
                    //return Unauthorized();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //Delete a user
        // POST: Users/Delete/5
        //[HttpPost("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());
        //    var result = await _userManager.DeleteAsync(user);
        //    return RedirectToAction("Index");
        //}

        [HttpPost("logout")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            //Remove the cookie from the browser
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return Ok();
            //return Redirect("home");
            //return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody]LoginModel model, string returnUrl = null)
        {
            //ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");

                    //If they were on their way to a url, send them there after they're logged in.

                    //Redirect on the front-end. Just return the result so we have the login credentials.
                    //return Ok(result);

                    //Redirect with .Net
                    return RedirectToLocal(returnUrl);
                    //return Redirect("home");
                    //return Redirect(returnUrl);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                //}
                //if (result.IsLockedOut)
                //{
                //    _logger.LogWarning(2, "User account locked out.");
                //    return View("Lockout");
                //}
                else
                {
                    //The string errorMessages actually names the object "errorMessages" so you can easily access it on the front-end
                    ModelState.AddModelError("errorMessages", "Invalid login attempt.");
                    return BadRequest(ModelState);
                }
            }

            //If we got this far, something failed.
            return BadRequest(ModelState);
        }

        //Prevent open redirect security issue
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Ok();
                //return Redirect("home");
                //return RedirectToAction("Index", "Home");
            }
        }
    }
}
