using ProductApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers
{
    [Route("api/testdatabase")]
    public class DummyDbController : Controller
    {
        private ProductApiContext _context;

        public DummyDbController(ProductApiContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
