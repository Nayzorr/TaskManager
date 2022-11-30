using TaskManager.Api.Models;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers.Interfaces
{
    public interface IAccountManager
    {
        Task<ResponseDTO<string>> Authentificate(UserLogin userLogin);
        Task<ResponseDTO<UserDTO>> GetUserById(int userId);
        Task<ResponseDTO<bool>> Register(UserDTO userDto);
    }
}
