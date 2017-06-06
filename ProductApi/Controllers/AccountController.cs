using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductApi.Entities;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Authorize]
    [Route("api/account")]
    public class AccountController: Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager; //should this be <user>?
        private readonly ILogger<AccountController> _logger;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpGet("getcurrentuser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                //Limit the information that we return for the user.
                var userObject = new
                {
                    email = user.Email,
                    id = user.Id,
                    userName = user.UserName,
                    roles = await _userManager.GetRolesAsync(user)
                };

                return Ok(userObject);
            }
        }

        [HttpGet("getusers")]
        //Use this to restrict this action to admins. This works but returns a 404 if the logged in user does not have the admin role.
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers()
        {
            //Get the complete list of users
            var users = await _userManager.Users.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            //Build a list of users that contains only the data we want to expose to the front-end
            var userList = new ArrayList();
            foreach (var curUser in users)
            {
                var tempUser = new UserModel()
                {
                    Role = (await _userManager.GetRolesAsync(curUser)).FirstOrDefault(),
                    RoleType = (await _userManager.GetRolesAsync(curUser)).FirstOrDefault(),
                    Email = curUser.Email
                };
                userList.Add(tempUser);
            }
            return Ok(userList);
        }

        [HttpGet("getuserroles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserRoles()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Return the roles for the user (an array of roles)
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(roles);
            }
            //Returning 401 or 400 results in an empty response on the FE. Using a 200 response is a work around.
            return Ok("unauthorized");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //Creates a new User with the user object passed and assigns a role to that user (either "basic" or "admin").
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
                    //If the roles do not yet exist in the Database, then create the roles in the DB. 
                    //This is a one-time creation. If the roles already exists, then this has no effect.
                    if (!await _roleManager.RoleExistsAsync("basic"))
                        await _roleManager.CreateAsync(new IdentityRole("basic")); //IdentityRole
                    if (!await _roleManager.RoleExistsAsync("admin"))
                        await _roleManager.CreateAsync(new IdentityRole("admin"));

                    //A claim is just a key/value pair. Add a custom "roleType" claim to make it easier for the FE to use the roleType.
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
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //Delete a user. I haven't tested this, but it might be needed down the role.
        // POST: Users/Delete/5
        //[HttpPost("Delete")]
        ////[ValidateAntiForgeryToken]
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
            //If we were using .Net Razor views, we could redirect the browser with a 302 response instead.
            //return Redirect("home");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody]LoginModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");

                    //If they were on their way to a url, send them there after they're logged in.
                    //This hasn't been tested, but the FE should be able to redirect them. You could also return an Ok()
                    //with the return URL intead of a redirect, since the redirect might only work with a Razor view.
                    return RedirectToLocal(returnUrl);
                    
                    //Redirect with .Net
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
                    //IMPORTANT: The string "errorMessages" actually names the object "errorMessages" so you can access it on the front-end
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
                //Since our front-end views use angular, we have to return a 200 instead of doing a direct redirect.
                return Ok(returnUrl);
                
                //If you're using Razor views, you can re-direct the user directly like this:
                //return RedirectToAction("Index", "Home");
            }
        }
    }
}
