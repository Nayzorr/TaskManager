using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers.Interfaces
{
    public interface IAccountManager
    {
        Task<ResponseDTO<string>> Authentificate(UserLogin userLogin);
        Task<ResponseDTO<UserDTO>> GetUserById(int userId);
        Task<ResponseDTO<bool>> Register(UserDTO userDto);
        Task<ResponseDTO<bool>> ChangeUserPassword(UserLogin userLogin);
        Task<ResponseDTO<bool>> ChangeFriendStatus(int currentUserId, UserFriendDTO userFriendDTO);
    }
}
