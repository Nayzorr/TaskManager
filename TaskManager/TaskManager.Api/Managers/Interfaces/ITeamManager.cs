using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers.Interfaces
{
    public interface ITeamManager
    {
        Task<ResponseDTO<bool>> CreateTeam(int userId, CreateTeamDTO createTeamDTO);
    }
}
