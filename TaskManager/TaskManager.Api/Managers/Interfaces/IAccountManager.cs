using TaskManager.Api.Models;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers.Interfaces
{
    public interface IAccountManager
    {
        Task<string> Authentificate(UserLogin userLogin);
        Task<UserDTO> GetUserById(int userId);
    }
}
