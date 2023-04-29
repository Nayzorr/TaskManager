using AutoMapper;
using TaskManager.Api.Accessors.Interfaces;
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

        public async Task<ResponseDTO<bool>> CreateUpdateTaskAsync(TaskCreateUpdateSelfUserDTO taskCreateUpdateDTO, int currentUserId)
        {
            if (taskCreateUpdateDTO.Id == default)
            {
                var newTask = _mapper.Map<DO.Task>(taskCreateUpdateDTO);

                var result = await _dbAccessor.CreateTaskAsync(newTask, currentUserId);
                return ResponseFormater.OK(result);
            }
            else
            {
                var existingtask = await _dbAccessor.GetTaskByIdAsync(taskCreateUpdateDTO.Id);

                if (existingtask is null)
                {
                    throw new Exception("No Task to update, this task doesn't exists");
                }

                if (taskCreateUpdateDTO.ParentId is not null)
                {
                    var parentTask = await _dbAccessor.GetTaskByIdAsync((int)taskCreateUpdateDTO.ParentId);

                    if (parentTask is null)
                    {
                        throw new Exception("Parent Task doesn't exists");
                    }
                }

                existingtask.Name = taskCreateUpdateDTO.Name;
                existingtask.TaskPriorityId = taskCreateUpdateDTO.TaskPriorityId;
                existingtask.TaskStatusId = taskCreateUpdateDTO.TaskStatusId;
                existingtask.DateScheduled = taskCreateUpdateDTO.DateScheduled;
                existingtask.Description = taskCreateUpdateDTO.Description;
                existingtask.ParentId = taskCreateUpdateDTO.ParentId;

                var result = await _dbAccessor.UpdateTaskInfoAsync(existingtask);
                return ResponseFormater.OK(result);
            }
        }
    }
}
