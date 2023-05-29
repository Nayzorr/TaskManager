using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers.Interfaces
{
    public interface ITaskPlanManager
    {
        Task<ResponseDTO<bool>> CreateUpdateTaskAsync(TaskCreateUpdateDTO taskCreateUpdateDTO, int currentUserId);
        Task<ResponseDTO<bool>> DeleteTaskAsync(int currentUserId, int taskId);
        Task<ResponseDTO<TaskDTO>> GetTaskById(int currentUserId, int taskId);
        Task<ResponseDTO<List<TaskDTO>>> GetTeamTasksAsync(int teamId, DateTime? scheduledDateFrom, DateTime? scheduledDateTo, int? taskPriorityId, int? taskStatusId);
        Task<ResponseDTO<List<TaskDTO>>> GetUserTasksAsync(int userId, DateTime? scheduledDateFrom, DateTime? scheduledDateTo, int? taskPriorityId, int? taskStatusId);
    }
}
