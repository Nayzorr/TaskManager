using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers.Interfaces
{
    public interface ITeamManager
    {
        Task<ResponseDTO<bool>> AddUserToTheTeam(int teamCreatorId, int userToAddId);
        Task<ResponseDTO<bool>> CheckIfTeamNameUnique(CreateTeamDTO teamDto);
        Task<ResponseDTO<bool>> CreateTeam(int userId, CreateTeamDTO createTeamDTO);
        Task<ResponseDTO<TeamInfoDTO>>  GetTeamMainInfoByName(string teamName);
        Task<ResponseDTO<bool>> InvitePersonToTeam(int inviterId, int teamId, string personToInviteUserName);
    }
}
