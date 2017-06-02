using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class UserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //DataType.password is used to hide the charactors on the screen. Compare just compares the value of confirmPassword
        //to Password, but I think this is more for if you're using a Razor view.
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //Whether the user is an admin or not. Used to set the role with the identity framework
        public string Role { get; set; }

        //A string description of the role to make accessing the roleType easier (not required by the Identity Framework).
        //This will be a custom "claim", which is basically just a key/value pair that we're adding the to user
        public string RoleType { get; set; }

        //public string FirstName{ get; set; }
        //public string LastName { get; set; }
    }
}
