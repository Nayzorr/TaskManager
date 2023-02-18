using Microsoft.AspNetCore.Authorization;
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
    public class RegisterController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger, IAccountManager accountManager)
        {
            _accountManager = accountManager;
            _logger = logger;
        }
        
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            try
            {
                if (!ModelState.IsValid || userDto is null)
                {
                    return BadRequest(ResponseFormater.Error(ErrorCodes.InvalidModel));
                }

                var result = await _accountManager.Register(userDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }
    }
}
