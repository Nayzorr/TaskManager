using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.DO;
using TaskManager.Api.Models;

namespace TaskManager.Api.Accessors
{
    public class DBAccessor : IDBAccessor
    {
        private readonly string _rapaportConnectionString;
        public DBAccessor(string rapaportConnectionString)
        {
            _rapaportConnectionString = rapaportConnectionString;
        }

        public async Task<User> GetUserByCredentionalsAsync(UserLogin userLogin)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            //TOOD: add password hashing
            var currentUser = await context.Users.FirstOrDefaultAsync(o => o.UserName.ToLower() == userLogin.UserName.ToLower() && o.PasswordHash == userLogin.Password);

            return currentUser;
        }

        public async Task<User> GetUserById(int userId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            //TOOD: add password hashing
            var currentUser = await context.Users.FirstOrDefaultAsync(o => o.Id == userId);

            return currentUser;
        }
    }
}
