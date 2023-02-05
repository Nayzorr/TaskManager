using TaskManager.Api.DO;
using TaskManager.Api.Enums;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Accessors.Interfaces
{
    public interface IDBAccessor
    {
        Task<User> GetUserByCredentionalsAsync(UserLogin userLogin);
        Task<User> GetUserById(int userId);
        Task<bool> Register(User mappedUser);
        Task<bool> ChangeUserPassword(UserLogin userChangePasswordDto);
        Task<User> GetUserByUserName(string userName);
        Task<bool> ChangeFriendStatus(int currentUserId, int userIdToChangeStatus, FriendStatusEnum friendStatus);
    }
}
