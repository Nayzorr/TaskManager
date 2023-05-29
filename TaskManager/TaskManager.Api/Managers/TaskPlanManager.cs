using AutoMapper;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.DO;
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

                var result = await _dbAccessor.CreateTaskAsync(newTask, currentUserId, taskCreateUpdateDTO.TeamMembersUserIds, taskCreateUpdateDTO.TeamId);
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

                var result = await _dbAccessor.UpdateTaskAsync(existingtask, currentUserId, taskCreateUpdateDTO.TeamMembersUserIds, taskCreateUpdateDTO.TeamId);
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

        public async Task<ResponseDTO<TaskDTO>> GetTaskById(int currentUserId, int taskId)
        {
            var existingtask = await _dbAccessor.GetTaskFullInfokByIdAsync(taskId);

            if (existingtask is null)
            {
                throw new Exception("Task doesn't exists");
            }

            var mappedTask = _mapper.Map<TaskDTO>(existingtask);

            var childTasks = await _dbAccessor.GetChildTasksFullInfoByTaskIdAsync(existingtask.Id);
            var mappedChildTasks = _mapper.Map<List<TaskDTO>>(childTasks);
            mappedTask.SubTasks = mappedChildTasks;

            return ResponseFormater.OK(mappedTask);
        }

        public async Task<ResponseDTO<List<TaskDTO>>> GetTeamTasksAsync(int teamId, DateTime? scheduledDateFrom, DateTime? scheduledDateTo, int? taskPriorityId, int? taskStatusId)
        {
            var teamTasks = await _dbAccessor.GetTasksFullInfokByTeamIdAsync(teamId, scheduledDateFrom, scheduledDateTo, taskPriorityId, taskStatusId);

            if (teamTasks is null)
            {
                throw new Exception("Tasks don't exists");
            }

            var mappedTasks = _mapper.Map<List<TaskDTO>>(teamTasks);

            foreach (var mappedTask in mappedTasks)
            {
                var childTasks = await _dbAccessor.GetChildTasksFullInfoByTaskIdAsync(mappedTask.Id);
                var mappedChildTasks = _mapper.Map<List<TaskDTO>>(childTasks);
                mappedTask.SubTasks = mappedChildTasks;
            }

            return ResponseFormater.OK(mappedTasks);
        }

        public async Task<ResponseDTO<List<TaskDTO>>> GetUserTasksAsync(int userId, DateTime? scheduledDateFrom, DateTime? scheduledDateTo, int? taskPriorityId, int? taskStatusId)
        {
            var userTasks = await _dbAccessor.GetTasksFullInfokByUserIdAsync(userId, scheduledDateFrom, scheduledDateTo, taskPriorityId, taskStatusId);

            if (userTasks is null)
            {
                throw new Exception("Tasks don't exists");
            }

            var mappedTasks = _mapper.Map<List<TaskDTO>>(userTasks);

            foreach (var mappedTask in mappedTasks)
            {
                var childTasks = await _dbAccessor.GetChildTasksFullInfoByTaskIdAsync(mappedTask.Id);
                var mappedChildTasks = _mapper.Map<List<TaskDTO>>(childTasks);
                mappedTask.SubTasks = mappedChildTasks;
            }          

            return ResponseFormater.OK(mappedTasks);
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
