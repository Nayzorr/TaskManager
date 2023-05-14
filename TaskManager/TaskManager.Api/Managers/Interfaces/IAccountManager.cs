using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers.Interfaces
{
    public interface IAccountManager
    {
        Task<ResponseDTO<string>> Authentificate(UserLogin userLogin);
        Task<ResponseDTO<BaseUserDTO>> GetUserById(int userId);
        Task<ResponseDTO<bool>> Register(RegisterUserDTO userDto);
        Task<ResponseDTO<bool>> ChangeUserPassword(int currentUserId, string password);
        Task<ResponseDTO<bool>> ChangeFriendStatus(int currentUserId, UserFriendStatusDTO userFriendDTO);
        Task<ResponseDTO<List<BaseUserDTO>>> GetUserFriendsList(int userId);
        Task<ResponseDTO<bool>> ChangeUserMainInfo(BaseUserDTO userDTO);
        Task<ResponseDTO<bool>> LogOut();
    }
}
