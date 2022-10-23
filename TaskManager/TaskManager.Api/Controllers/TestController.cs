using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [HttpPost]
        [Route("api/Test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Test");
        }
    }
}