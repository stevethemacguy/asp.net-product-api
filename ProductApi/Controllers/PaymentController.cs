using System.Collections;
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
    [Route("api/payment")]
    public class PaymentController: Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;
        
        public PaymentController(UserManager<User> userManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

    }
}
