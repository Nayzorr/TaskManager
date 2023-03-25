﻿using TaskManager.Api.DO;
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
        Task<bool> AddUserToTheTeamAsync(int teamCreatorId, int userToAddId);
        Task<List<User>> GetTeamMembertsByIdAsync(int teamId);
        Task<List<User>> GetUserFriendsList(int userId);
        Task<bool> СhangeTeamNameAsync(int teamCreatorId, ChangeTeamNameDTO changeTeamNameDTO);
    }
}
