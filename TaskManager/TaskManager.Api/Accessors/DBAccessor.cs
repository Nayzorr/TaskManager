using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.DO;
using TaskManager.Api.Enums;
using TaskManager.Api.Models.DTOs;
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

        public async Task<bool> AddUserToTheTeamAsync(int teamCreatorId, int userToAddId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var IsAcceptorTeamCreator = await context.Teams.SingleOrDefaultAsync(o => o.CreatorId == teamCreatorId);

            if (IsAcceptorTeamCreator is null)
            {
                throw new Exception("Acceptor is not team creator, cannot to accept user to the team");
            }

            var userInvitation = await context.TeamInvitations.SingleOrDefaultAsync(x => x.UserToInviteId == userToAddId);

            if (userInvitation is null)
            {
                throw new Exception("User doesn't exists Has no invtitation to Team, cannot add without invitation");
            }

            using var transaction = context.Database.BeginTransaction();

            try
            {
                //remove from team invitation table + add to team
                context.Remove(userInvitation);
                await context.UserTeams.AddAsync(new UserTeam() { TeamId = userInvitation.TeamId, UserId = userInvitation.UserToInviteId });
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> ChangeFriendStatusAsync(int currentUserId, int userIdToChangeStatus, FriendStatusEnum friendStatus)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            await context.Friends.AddAsync(new Friend()
            {
                UserFirstId = currentUserId,
                UserSecondId = userIdToChangeStatus,
                FriendStatusId = (int)friendStatus
            });

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeUserPasswordAsync(UserLogin userChangePasswordDto)
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

        public async Task<bool> CheckIfTeamNameUniqueAsync(CreateTeamDTO teamDto)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var result = await context.Teams.AnyAsync(x => x.TeamName == teamDto.TeamName);

            return !result;
        }

        public async Task<bool> CreateTeamAsync(Team teamToCreate)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            if (context.Teams.Any(x => x.TeamName == teamToCreate.TeamName))
            {
                throw new Exception("Team Name '" + teamToCreate.TeamName + "' is already taken");
            }

            await context.Teams.AddAsync(teamToCreate);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Team> GetTeamMainInfoByNameAsync(string teamName)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            //TODO verify It after at least one user will be in team
            var team = await context.Teams.AsNoTracking()
                .SingleOrDefaultAsync(o => o.TeamName == teamName);

            if (team is null)
            {
                throw new Exception("Team not found");
            }

            return team;
        }

        public async Task<List<User>> GetTeamMembertsByIdAsync(int teamId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            return await context.Users
                .Where(u => context.UserTeams.Where(ut => ut.TeamId == teamId)
                .Select(e => e.UserId).Contains(u.Id)).ToListAsync();
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

        public async Task<User> GetUserByIdAsync(int userId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var currentUser = await context.Users.SingleOrDefaultAsync(o => o.Id == userId);

            return currentUser;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var currentUser = await context.Users.SingleOrDefaultAsync(o => o.UserName == userName);

            return currentUser;
        }

        public async Task<bool> InvitePersonToTeamAsync(int inviterId, int teamId, string personToInviteUserName)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var IsInviterTeamCreator = await context.Teams.SingleOrDefaultAsync(o => o.CreatorId == inviterId);

            if (IsInviterTeamCreator is null)
            {
                throw new Exception("Inviter is not team creator, cannot to invite user");
            }

            var personToInvite = await context.Users.SingleOrDefaultAsync(o => o.UserName == personToInviteUserName);

            if (personToInvite is null)
            {
                throw new Exception("Person To Invite not found");
            }

            if (context.UserTeams.Any(x => x.TeamId == teamId && x.UserId == personToInvite.Id))
            {
                throw new Exception("User already in this team '");
            }

            await context.TeamInvitations.AddAsync(new TeamInvitation
            {
                InviterId = inviterId,
                TeamId = teamId,
                UserToInviteId = personToInvite.Id
            });

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegisterAsync(User mappedUser)
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
