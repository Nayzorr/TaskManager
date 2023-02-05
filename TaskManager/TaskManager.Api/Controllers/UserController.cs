using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Api.Enums;
using TaskManager.Api.Managers.Interfaces;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IAccountManager accountManager)
        {
            _accountManager = accountManager;
            _logger = logger;
        }

        [HttpGet("GetUserMainInfo")]
        [Authorize]
        public async Task<IActionResult> GetUserMainInfo()
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int userId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var userData = await _accountManager.GetUserById(userId);
                    return Ok(userData);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));
            }
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangeUserPassword(UserLogin userLogin)
        {
            try
            {
                if (!ModelState.IsValid || userLogin is null)
                {
                    return BadRequest(ResponseFormater.Error(ErrorCodes.InvalidModel));
                }

                var result = await _accountManager.ChangeUserPassword(userLogin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));
            }
        }

        [HttpPost("ChangeFriendStatus")]
        [Authorize]
        public async Task<IActionResult> ChangeFriendStatus([FromBody] UserFriendDTO userFriendDto)
        {
            try
            {
                if (!ModelState.IsValid || userFriendDto is null)
                {
                    return BadRequest(ResponseFormater.Error(ErrorCodes.InvalidModel));
                }

                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int currentUserId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _accountManager.ChangeFriendStatus(currentUserId, userFriendDto);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));
            }
        }
    }
}
