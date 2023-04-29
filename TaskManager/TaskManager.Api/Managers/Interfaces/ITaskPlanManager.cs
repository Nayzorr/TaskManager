using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers.Interfaces
{
    public interface ITaskPlanManager
    {
        Task<ResponseDTO<bool>> CreateUpdateTaskAsync(TaskCreateUpdateSelfUserDTO taskCreateUpdateDTO, int currentUserId);
    }
}
