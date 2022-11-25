using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Api.Enums;
using TaskManager.Api.Managers.Interfaces;
using TaskManager.Api.Models;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger, IAccountManager accountManager)
        {
            _accountManager = accountManager;
            _logger = logger;
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
                _logger.LogError(ex.Message, ex);
                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));
            }
        }
    }
}