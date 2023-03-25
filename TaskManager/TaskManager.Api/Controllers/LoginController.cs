using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Enums;
using TaskManager.Api.Managers.Interfaces;
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

                if (!ModelState.IsValid || userLogin is null)
                {
                    return BadRequest(ResponseFormater.Error(ErrorCodes.InvalidModel));
                }

                var token = await _accountManager.Authentificate(userLogin);

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }
    }
}