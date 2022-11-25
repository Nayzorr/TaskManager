using AutoMapper;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.Helpers;
using TaskManager.Api.Managers.Interfaces;
using TaskManager.Api.Models;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly IDBAccessor _dbAccessor;
        private readonly IMapper _mapper;

        public AccountManager(IDBAccessor dBAccessor, IMapper mapper)
        {
            _dbAccessor = dBAccessor;
            _mapper = mapper;
        }

        public async Task<string> Authentificate(UserLogin userLogin)
        {
            var currentUser = await _dbAccessor.GetUserByCredentionalsAsync(userLogin);

            if (currentUser == null)
            {
                throw new Exception("User not found");
            }

            var token = AccountHelper.GenerateToken(currentUser);

            return token;
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            var dbUser = await _dbAccessor.GetUserById(userId);

            if (dbUser == null)
            {
                throw new Exception("User not found");
            }

            var mappedUser = _mapper.Map<UserDTO>(dbUser);

            return mappedUser;
        }
    }
}
