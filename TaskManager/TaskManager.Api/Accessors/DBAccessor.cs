using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.DO;
using TaskManager.Api.Models;
using BC = BCrypt.Net.BCrypt;

namespace TaskManager.Api.Accessors
{
    public class DBAccessor : IDBAccessor
    {
        private readonly string _rapaportConnectionString;
        public DBAccessor(string rapaportConnectionString)
        {
            _rapaportConnectionString = rapaportConnectionString;
        }

        public async Task<bool> ChangeUserPassword(UserLogin userChangePasswordDto)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var currentUser = await context.Users.SingleOrDefaultAsync(o => o.UserName == userChangePasswordDto.UserName);

            if (currentUser == null)
            {
                throw new Exception("Username '" + userChangePasswordDto.UserName + "' not found");
            }

            currentUser.PasswordHash = BC.HashPassword(userChangePasswordDto.Password);
            context.Users.Update(currentUser);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetUserByCredentionalsAsync(UserLogin userLogin)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var currentUser = await context.Users.SingleOrDefaultAsync(o => o.UserName == userLogin.UserName);

            if (currentUser is null || !BC.Verify(userLogin.Password, currentUser.PasswordHash))
            {
                throw new Exception("Username or password is incorrect");
            }

            return currentUser;
        }

        public async Task<User> GetUserById(int userId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            //TOOD: add password hashing
            var currentUser = await context.Users.SingleOrDefaultAsync(o => o.Id == userId);

            return currentUser;
        }

        public async Task<bool> Register(User mappedUser)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            if (context.Users.Any(x => x.UserName == mappedUser.UserName))
            {
                throw new Exception("Username '" + mappedUser.UserName + "' is already taken");
            }

            await context.Users.AddAsync(mappedUser);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
