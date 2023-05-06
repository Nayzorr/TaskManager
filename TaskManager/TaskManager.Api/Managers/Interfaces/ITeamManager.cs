using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers.Interfaces
{
    public interface ITeamManager
    {
        Task<ResponseDTO<bool>> AcceptTeamInvitationAsync(int userToAddId, string teamName);
        Task<ResponseDTO<bool>> CheckIfTeamNameUnique(CreateTeamDTO teamDto);
        Task<ResponseDTO<bool>> CreateTeam(int userId, CreateTeamDTO createTeamDTO);
        Task<ResponseDTO<bool>> DeleteTeamAsync(int userId, int teamIdToDelete);
        Task<ResponseDTO<bool>> DeleteUserFromTheTeamAsync(int teamCreatorId, int userToDeleteId, string teamName);
        Task<ResponseDTO<List<TeamInfoDTO>>> GetMyTeamInvitationsAsync(int userId);
        Task<ResponseDTO<TeamInfoDTO>> GetTeamMainInfoByName(string teamName);
        Task<ResponseDTO<bool>> InvitePersonToTeam(int inviterId, int teamId, string personToInviteUserName);
        Task<ResponseDTO<bool>> RejectTeamInvitationAsync(int userId, string teamName);
        Task<ResponseDTO<bool>> СhangeTeamName(int teamCreatorId, ChangeTeamNameDTO changeTeamNameDTO);
    }
}
