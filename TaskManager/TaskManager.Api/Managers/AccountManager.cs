using AutoMapper;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.DO;
using TaskManager.Api.Helpers;
using TaskManager.Api.Managers.Interfaces;
using TaskManager.Api.Models;
using TaskManager.Api.Models.DTOs;
using BC = BCrypt.Net.BCrypt;

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

        public async Task<ResponseDTO<string>> Authentificate(UserLogin userLogin)
        {
            var currentUser = await _dbAccessor.GetUserByCredentionalsAsync(userLogin);

            var token = AccountHelper.GenerateToken(currentUser);

            return ResponseFormater.OK(token);
        }

        public async Task<ResponseDTO<UserDTO>> GetUserById(int userId)
        {
            var dbUser = await _dbAccessor.GetUserById(userId);

            if (dbUser == null)
            {
                throw new Exception("User not found");
            }

            var mappedUser = _mapper.Map<UserDTO>(dbUser);

            return ResponseFormater.OK(mappedUser);
        }

        public async Task<ResponseDTO<bool>> Register(UserDTO userDto)
        {
            var mappedUser = _mapper.Map<User>(userDto);

            mappedUser.PasswordHash = BC.HashPassword(userDto.Password);

            var result = await _dbAccessor.Register(mappedUser);

            return ResponseFormater.OK(result);
        }
    }
}
