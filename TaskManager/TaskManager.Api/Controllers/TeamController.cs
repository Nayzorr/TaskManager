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
    public class TeamController : ControllerBase
    {
        private readonly ITeamManager _teamManager;
        private readonly ILogger<TeamController> _logger;

        public TeamController(ILogger<TeamController> logger, ITeamManager teamManager)
        {
            _teamManager = teamManager;
            _logger = logger;
        }

        [HttpPost("CreateTeam")]
        [Authorize]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamDTO createTeamDTO)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int userId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _teamManager.CreateTeam(userId, createTeamDTO);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }

        [HttpDelete("DeleteTeam")]
        [Authorize]
        public async Task<IActionResult> DeleteTeam(int teamIdToDelete)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int userId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _teamManager.DeleteTeamAsync(userId, teamIdToDelete);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }

        [HttpPut("СhangeTeamName")]
        [Authorize]
        public async Task<IActionResult> СhangeTeamName([FromBody] ChangeTeamNameDTO changeTeamNameDto)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int userId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _teamManager.СhangeTeamName(userId, changeTeamNameDto);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }

        [HttpGet("GetTeamMainInfoByName/{teamName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTeamMainInfoByName(string teamName)
        {
            try
            {
                var result = await _teamManager.GetTeamMainInfoByName(teamName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }

        [HttpPost("CheckIfTeamNameUnique")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckIfTeamNameUnique([FromBody] CreateTeamDTO teamDto)
        {
            try
            {
                var result = await _teamManager.CheckIfTeamNameUnique(teamDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }

        [HttpPost("InvitePersonToTeam/{teamId}/{personToInviteUserName}")]
        [Authorize]
        public async Task<IActionResult> InvitePersonToTeam(int teamId, string personToInviteUserName)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int invtiterId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _teamManager.InvitePersonToTeam(invtiterId, teamId, personToInviteUserName);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }

        [HttpGet("GetMyTeamInvitations")]
        [Authorize]
        public async Task<IActionResult> GetMyInvitationsAsync()
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int userId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _teamManager.GetMyTeamInvitationsAsync(userId);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }

        [HttpPut("AcceptTeamInvitation")]
        [Authorize]
        public async Task<IActionResult> AcceptTeamInvitation(string teamName)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int userId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _teamManager.AcceptTeamInvitationAsync(userId, teamName);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }

        [HttpPut("RejectTeamInvitation")]
        [Authorize]
        public async Task<IActionResult> RejectTeamInvitation(string teamName)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int userId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _teamManager.RejectTeamInvitationAsync(userId, teamName);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }


        [HttpDelete("DeleteUserFromTheTeam/{userToDeleteId}/{teamName}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserFromTheTeam(int userToDeleteId, string teamName)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int teamCreatorId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _teamManager.DeleteUserFromTheTeamAsync(teamCreatorId, userToDeleteId, teamName);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ex, ErrorCodes.InternalServerException));
            }
        }
    }
}
