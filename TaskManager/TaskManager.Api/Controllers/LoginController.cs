using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Api.Managers.Interfaces;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        public LoginController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            try
            {
                var token = await _accountManager.Authentificate(userLogin);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return NotFound("User not found");
            }
        }
    }
}