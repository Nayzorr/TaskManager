using AutoMapper;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.DO;
using TaskManager.Api.Helpers;
using TaskManager.Api.Managers.Interfaces;
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

        public async Task<ResponseDTO<bool>> ChangeFriendStatus(int currentUserId, UserFriendStatusDTO userFriendDTO)
        {
            var userToToChangeStatus = await _dbAccessor.GetUserByUserNameAsync(userFriendDTO.UserNameToChangeStatus);

            if (userToToChangeStatus == null)
            {
                throw new NullReferenceException($"User {userToToChangeStatus} not found");
            }

            if (userToToChangeStatus.Id == currentUserId)
            {
                throw new ArgumentException($"It's not possible to add to friends yourself");
            }

            var result = await _dbAccessor.ChangeFriendStatusAsync(currentUserId, userToToChangeStatus.Id, userFriendDTO.FriendStatus);

            return ResponseFormater.OK(result);
        }

        public async Task<ResponseDTO<string>> Authentificate(UserLogin userLogin)
        {
            var currentUser = await _dbAccessor.GetUserByCredentionalsAsync(userLogin);

            var token = AccountHelper.GenerateToken(currentUser);

            return ResponseFormater.OK(token);
        }

        public async Task<ResponseDTO<bool>> ChangeUserPassword(int currentUserId, string password)
        {
            var result = await _dbAccessor.ChangeUserPasswordAsync(currentUserId, password);
            return ResponseFormater.OK(result);
        }

        public async Task<ResponseDTO<BaseUserDTO>> GetUserById(int userId)
        {
            var dbUser = await _dbAccessor.GetUserByIdAsync(userId);

            if (dbUser == null)
            {
                throw new Exception("User not found");
            }

            var mappedUser = _mapper.Map<BaseUserDTO>(dbUser);

            return ResponseFormater.OK(mappedUser);
        }

        public async Task<ResponseDTO<bool>> Register(RegisterUserDTO userDto)
        {
            var mappedUser = _mapper.Map<User>(userDto);

            mappedUser.PasswordHash = BC.HashPassword(userDto.Password);

            var result = await _dbAccessor.RegisterAsync(mappedUser);

            return ResponseFormater.OK(result);
        }

        public async Task<ResponseDTO<List<BaseUserDTO>>> GetUserFriendsList(int userId)
        {
            var dbUserFriendsList = await _dbAccessor.GetUserFriendsList(userId);

            var mappedUserFriendsList = _mapper.Map<List<BaseUserDTO>>(dbUserFriendsList);

            return ResponseFormater.OK(mappedUserFriendsList);
        }

        public async Task<ResponseDTO<bool>> ChangeUserMainInfo(BaseUserDTO userDto)
        {
            var userToChangeInfo = await _dbAccessor.GetUserByIdAsync(userDto.Id);

            if (userToChangeInfo == null)
            {
                throw new Exception("User not found");
            }

            var mappedUser = _mapper.Map(userDto, userToChangeInfo);

            var result = await _dbAccessor.ChangeUserMainInfo(userToChangeInfo);

            return ResponseFormater.OK(result);
        }
    }
}
