﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Api.Enums;
using TaskManager.Api.Managers;
using TaskManager.Api.Managers.Interfaces;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskPlanManager _taskPlanManager;
        private readonly ILogger<UserController> _logger;

        public TaskController(ILogger<UserController> logger, ITaskPlanManager taskPlanManager)
        {
            _taskPlanManager = taskPlanManager;
            _logger = logger;
        }

        [HttpPost("CreateUpdateTask")]
        [Authorize]
        public async Task<IActionResult> CreateUpdateTaskAsync([FromBody] TaskCreateUpdateDTO taskCreateUpdateDTO)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int currentUserId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _taskPlanManager.CreateUpdateTaskAsync(taskCreateUpdateDTO, currentUserId);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));
            }
        }

        [HttpDelete("DeleteTask")]
        [Authorize]
        public async Task<IActionResult> DeleteTaskAsync(int taskId)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int currentUserId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _taskPlanManager.DeleteTaskAsync(currentUserId, taskId);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));
            }
        }

        [HttpGet("GetTaskById/{taskId}")]
        [Authorize]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    int currentUserId = int.Parse(identity?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value);
                    var result = await _taskPlanManager.GetTaskById(currentUserId, taskId);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));
            }
        }

        [HttpGet("GetUserTasks/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserTasksAsync(int userId, DateTime? scheduledDateFrom, DateTime? scheduledDateTo)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    var result = await _taskPlanManager.GetUserTasksAsync(userId, scheduledDateFrom, scheduledDateTo);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));
            }
        }

        [HttpGet("GetTeamTasks/{teamId}")]
        [Authorize]
        public async Task<IActionResult> GetTeamTasksAsync(int teamId, DateTime? scheduledDateFrom, DateTime? scheduledDateTo)
        {
            try
            {
                if (HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    var result = await _taskPlanManager.GetTeamTasksAsync(teamId, scheduledDateFrom, scheduledDateTo);
                    return Ok(result);
                }

                return BadRequest(ResponseFormater.Error(ErrorCodes.EntityNotFound));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseFormater.Error(ErrorCodes.InternalServerException));
            }
        }
    }
}