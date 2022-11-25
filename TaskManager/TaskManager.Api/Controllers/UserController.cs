﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Api.Managers.Interfaces;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountManager _accountManager;

        public UserController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
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

                return NotFound("User not authorized");
            }
            catch (Exception ex)
            {
                return NotFound("User not authorized");
            }
        }
    }
}
