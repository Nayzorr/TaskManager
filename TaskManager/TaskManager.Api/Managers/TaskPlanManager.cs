using AutoMapper;
using Microsoft.AspNetCore.Connections.Features;
using System.Threading.Tasks;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.DO;
using TaskManager.Api.Helpers;
using TaskManager.Api.Managers.Interfaces;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers
{
    public class TaskPlanManager : ITaskPlanManager
    {
        private readonly IDBAccessor _dbAccessor;
        private readonly IMapper _mapper;

        public TaskPlanManager(IDBAccessor dBAccessor, IMapper mapper)
        {
            _dbAccessor = dBAccessor;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<bool>> CreateUpdateTaskAsync(TaskCreateUpdateDTO taskCreateUpdateDTO, int currentUserId)
        {
            await CheckParentTaskIdValid(taskCreateUpdateDTO);

            if (taskCreateUpdateDTO.Id == default)
            {
                var newTask = _mapper.Map<DO.Task>(taskCreateUpdateDTO);

                var result = await _dbAccessor.CreateTaskAsync(newTask, currentUserId, taskCreateUpdateDTO.TeamMemberUserId, taskCreateUpdateDTO.TeamId);
                return ResponseFormater.OK(result);
            }
            else
            {
                var existingtask = await _dbAccessor.GetTaskByIdAsync(taskCreateUpdateDTO.Id);

                if (existingtask is null)
                {
                    throw new Exception("No Task to update, this task doesn't exists");
                }

                existingtask.Name = taskCreateUpdateDTO.Name;
                existingtask.TaskPriorityId = taskCreateUpdateDTO.TaskPriorityId;
                existingtask.TaskStatusId = taskCreateUpdateDTO.TaskStatusId;
                existingtask.DateScheduled = taskCreateUpdateDTO.DateScheduled;
                existingtask.Description = taskCreateUpdateDTO.Description;
                existingtask.ParentId = taskCreateUpdateDTO.ParentId;

                var result = await _dbAccessor.UpdateTaskAsync(existingtask, currentUserId, taskCreateUpdateDTO.TeamMemberUserId, taskCreateUpdateDTO.TeamId);
                return ResponseFormater.OK(result);
            }
        }

        public async Task<ResponseDTO<bool>> DeleteTaskAsync(int currentUserId, int taskId)
        {
            var existingtask = await _dbAccessor.GetTaskByIdAsync(taskId);

            if (existingtask is null)
            {
                throw new Exception("No Task to update, this task doesn't exists");
            }

            var childTasks = await _dbAccessor.GetChildTasksByTaskIdAsync(existingtask.Id);

            var result = await _dbAccessor.DeleteTaskAsync(existingtask, childTasks);
            return ResponseFormater.OK(result);
        }

        private async System.Threading.Tasks.Task CheckParentTaskIdValid(TaskCreateUpdateDTO taskCreateUpdateDTO)
        {
            if (taskCreateUpdateDTO.ParentId == 0 || taskCreateUpdateDTO.Id == taskCreateUpdateDTO.ParentId)
            {
                taskCreateUpdateDTO.ParentId = null;
            }

            if (taskCreateUpdateDTO.ParentId.HasValue)
            {
                var parentTask = await _dbAccessor.GetTaskByIdAsync(taskCreateUpdateDTO.ParentId.Value);

                if (parentTask == null)
                {
                    throw new Exception("Parent task does not exist.");
                }
            }
        }
    }
}
