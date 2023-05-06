using TaskManager.Api.DO;
using TaskManager.Api.Enums;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Accessors.Interfaces
{
    public interface IDBAccessor
    {
        Task<User> GetUserByCredentionalsAsync(UserLogin userLogin);
        Task<User> GetUserByIdAsync(int userId);
        Task<bool> RegisterAsync(User mappedUser);
        Task<bool> ChangeUserPasswordAsync(int currentUserId, string password);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<bool> ChangeFriendStatusAsync(int currentUserId, int userIdToChangeStatus, FriendStatusEnum friendStatus);
        Task<bool> CreateTeamAsync(Team teamToCreate);
        Task<bool> CheckIfTeamNameUniqueAsync(CreateTeamDTO teamDto);
        Task<bool> InvitePersonToTeamAsync(int inviterId, int teamId, string personToIviteUserName);
        Task<Team> GetTeamMainInfoByNameAsync(string teamName);
        Task<bool> AcceptTeamInvitationAsync(int userToAddId, string teamName);
        Task<List<User>> GetTeamMembersByIdAsync(int teamId);
        Task<List<User>> GetUserFriendsList(int userId);
        Task<bool> СhangeTeamNameAsync(int teamCreatorId, ChangeTeamNameDTO changeTeamNameDTO);
        Task<bool> DeleteUserFromTheTeamAsync(int teamCreatorId, int userToDeleteId, string teamName);
        Task<bool> ChangeUserMainInfo(User mappedUser);
        Task<bool> CreateTaskAsync(DO.Task newTask, int currentUserId, int? teamMemberId, int? teamId);
        Task<DO.Task> GetTaskByIdAsync(int taskId);
        Task<bool> UpdateTaskAsync(DO.Task taskToUpdate, int currentUserId, int? teamMemberId, int? teamId);
        Task<List<TeamInvitation>> GetUserTeamInvitationsAsync(int userId);
        Task<List<Team>> GetTeamsByTeamIds(List<int> teamIds);
        Task<bool> RejectTeamInvitationAsync(int userId, string teamName);
        Task<bool> DeleteTeamAsync(int userId, int teamIdToDelete);
        Task<List<DO.Task>> GetChildTasksByTaskIdAsync(int? taskId);
        Task<bool> DeleteTaskAsync(DO.Task existingtask, List<DO.Task> childTasks);
    }
}
