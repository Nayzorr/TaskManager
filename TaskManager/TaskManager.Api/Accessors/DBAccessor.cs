using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.DO;
using TaskManager.Api.Enums;
using TaskManager.Api.Helpers;
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

        #region Team

        public async Task<bool> AcceptTeamInvitationAsync(int userToAddId, string teamName)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var teamToAddUser = await context.Teams.SingleOrDefaultAsync(o => o.TeamName == teamName);

            if (teamToAddUser is null)
            {
                throw new Exception("Team is not found, cannot to accept user to the team");
            }

            var userInvitation = await context.TeamInvitations.SingleOrDefaultAsync(x => x.UserToInviteId == userToAddId && x.TeamId == teamToAddUser.Id);

            if (userInvitation is null)
            {
                throw new Exception("User has no invtitation to Team, cannot add without invitation");
            }

            using var transaction = context.Database.BeginTransaction();

            try
            {
                //remove from team invitation table + add to team
                context.TeamInvitations.Remove(userInvitation);
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

        public async Task<bool> RejectTeamInvitationAsync(int userId, string teamName)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var teamToReject = await context.Teams.SingleOrDefaultAsync(o => o.TeamName == teamName);

            if (teamToReject is null)
            {
                throw new Exception("Team is not found, cannot to reject team invitation");
            }

            var userInvitation = await context.TeamInvitations.SingleOrDefaultAsync(x => x.UserToInviteId == userId && x.TeamId == teamToReject.Id);

            if (userInvitation is null)
            {
                throw new Exception("User has no invtitation to Team, cannot reject");
            }

            using var transaction = context.Database.BeginTransaction();

            try
            {
                //remove from team invitation table + add to team
                context.TeamInvitations.Remove(userInvitation);
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

        public async Task<bool> DeleteUserFromTheTeamAsync(int teamCreatorId, int userToDeleteId, string teamName)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var teamToDelete = await context.Teams.SingleOrDefaultAsync(o => o.CreatorId == teamCreatorId && o.TeamName == teamName);

            if (teamToDelete is null)
            {
                throw new Exception("Acceptor is not team creator, or team not found, cannot delete accept user from the team");
            }

            var personToDelete = await context.Users.SingleOrDefaultAsync(o => o.Id == userToDeleteId);

            if (personToDelete is null)
            {
                throw new Exception("Person To Delete not found");
            }

            var userTeam = await context.UserTeams.SingleOrDefaultAsync(o => o.TeamId == teamToDelete.Id && o.UserId == userToDeleteId);

            if (userTeam is null)
            {
                throw new Exception("Team and User relationship not found, cannot to delete user from the team");
            }

            using var transaction = context.Database.BeginTransaction();

            try
            {
                context.UserTeams.Remove(userTeam);
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

        public async Task<List<Team>> GetTeamsByTeamIds(List<int> teamIds)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            //TODO verify It after at least one user will be in team
            var teams = await context.Teams
                .Where(o => teamIds.Contains(o.Id)).ToListAsync();

            return teams;
        }

        public async Task<bool> DeleteTeamAsync(int userId, int teamIdToDelete)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var teamToDelete = await context.Teams.SingleOrDefaultAsync(o => o.CreatorId == userId && o.Id == teamIdToDelete);

            if (teamToDelete is null)
            {
                throw new Exception("User is not team creator, or team not found");
            }

            context.Teams.Remove(teamToDelete); //it will remove all tasks assigned to team and team invtitation

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<User>> GetTeamMembersByIdAsync(int teamId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            return await context.Users
                .Where(u => context.UserTeams.Where(ut => ut.TeamId == teamId)
                .Select(e => e.UserId).Contains(u.Id)).ToListAsync();
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

        public async Task<bool> СhangeTeamNameAsync(int teamCreatorId, ChangeTeamNameDTO changeTeamNameDTO)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var team = await context.Teams.AsNoTracking()
               .SingleOrDefaultAsync(o => o.TeamName == changeTeamNameDTO.OldTeamName);

            if (team is null || teamCreatorId != team.CreatorId)
            {
                throw new Exception("Team not found or user is not team creator");
            }

            team.TeamName = changeTeamNameDTO.NewTeamName;

            context.Update(team);
            await context.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Task

        public async Task<bool> UpdateTaskAsync(DO.Task taskToUpdate, int currentUserId, int? teamMemberId, int? teamId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            if (BasicOperationsHelper.CheckIntValueValid(teamMemberId) && BasicOperationsHelper.CheckIntValueValid(teamId))
            {
                var isUserExistsInTeam = await context.UserTeams.AnyAsync(o => o.TeamId == teamId && o.UserId == teamMemberId);

                var teamCreator = await context.Teams.SingleOrDefaultAsync(o => o.CreatorId == currentUserId && o.Id == teamId);

                if (teamCreator is null)
                {
                    throw new Exception($"{currentUserId} is not team creator, or team is not found, cannot to assign the task");
                }

                if (!await context.UserTeams.AnyAsync(o => o.TeamId == teamId && o.UserId == teamMemberId))
                {
                    throw new Exception($"User with Id {teamMemberId} doesn't belongs to the team, cannot assign the task.");
                }

                var taskAssignment = await context.TaskAssignments.SingleOrDefaultAsync(e => e.TaskId == taskToUpdate.Id);

                if (teamCreator is null)
                {
                    await context.TaskAssignments.AddAsync(new TaskAssignment()
                    {
                        TaskId = taskToUpdate.Id,
                        UserId = teamMemberId.Value,
                        TeamId = teamId // For team tasks
                    });
                }
                else
                {
                    taskAssignment.UserId = teamMemberId.Value;
                    context.TaskAssignments.Update(taskAssignment);
                }
            }
            else
            {
                context.Tasks.Update(taskToUpdate);
            }

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<DO.Task> GetTaskByIdAsync(int taskId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var task = await context.Tasks.AsNoTracking()
               .SingleOrDefaultAsync(o => o.Id == taskId);

            return task;
        }

        public async Task<bool> CreateTaskAsync(DO.Task newTask, int currentUserId, int? teamMemberId, int? teamId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            using var transaction = context.Database.BeginTransaction();

            try
            {
                await context.Tasks.AddAsync(newTask);
                await context.SaveChangesAsync();

                int newTaskId = newTask.Id;

                if (BasicOperationsHelper.CheckIntValueValid(teamMemberId) && BasicOperationsHelper.CheckIntValueValid(teamId))
                {
                    var teamCreator = await context.Teams.SingleOrDefaultAsync(o => o.CreatorId == currentUserId && o.Id == teamId);

                    if (teamCreator is null)
                    {
                        throw new Exception($"{currentUserId} is not team creator, or team is not found, cannot to assign the task");
                    }

                    if (!await context.UserTeams.AnyAsync(o => o.TeamId == teamId && o.UserId == teamMemberId))
                    {
                        throw new Exception($"User with Id {teamMemberId} doesn't belongs to the team, cannot assign the task.");
                    }

                    await context.TaskAssignments.AddAsync(new TaskAssignment()
                    {
                        TaskId = newTaskId,
                        UserId = teamMemberId.Value,
                        TeamId = teamId // For team tasks
                    });
                }
                else
                {
                    await context.TaskAssignments.AddAsync(new TaskAssignment()
                    {
                        TaskId = newTaskId,
                        UserId = currentUserId,
                        TeamId = null // For non-team tasks
                    });
                }

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

        public async Task<List<DO.Task>> GetChildTasksByTaskIdAsync(int? taskId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var childTasks = await context.Tasks.Where(e => e.ParentId == taskId).ToListAsync();

            return childTasks;
        }

        public async Task<bool> DeleteTaskAsync(DO.Task existingtask, List<DO.Task> childTasks)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);
            using var transaction = context.Database.BeginTransaction();

            try
            {
                if (childTasks is not null && childTasks.Any())
                {
                    context.Tasks.RemoveRange(childTasks);
                }

                context.Remove(existingtask);
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

        #endregion

        #region Account

        public async Task<bool> ChangeFriendStatusAsync(int currentUserId, int userIdToChangeStatus, FriendStatusEnum friendStatus)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var currentState = await context.Friends
                .SingleOrDefaultAsync(e => (e.UserFirstId == currentUserId && e.UserSecondId == userIdToChangeStatus) ||
                (e.UserSecondId == currentUserId && e.UserFirstId == userIdToChangeStatus));

            if (currentState is null && friendStatus != FriendStatusEnum.Pending)
            {
                return false;
            }

            switch (friendStatus)
            {
                case FriendStatusEnum.Pending:
                    if (currentState is null)
                    {
                        await context.Friends.AddAsync(new Friend
                        {
                            UserFirstId = currentUserId,
                            UserSecondId = userIdToChangeStatus,
                            FriendStatusId = (int)FriendStatusEnum.Pending
                        });
                    }
                    else
                    {
                        return false;
                    }
                    break;

                case FriendStatusEnum.InFriends:
                    if (currentState is null)
                    {
                        return false;
                    }
                    else
                    {
                        currentState.FriendStatusId = (int)FriendStatusEnum.InFriends;
                        context.Friends.Update(currentState);
                    }
                    break;

                case FriendStatusEnum.Rejected:
                    if (currentState is null)
                    {
                        return false;
                    }
                    else
                    {
                        context.Friends.Remove(currentState);
                    }
                    break;
            }

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeUserPasswordAsync(int currentUserId, string password)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var currentUser = await context.Users.SingleOrDefaultAsync(o => o.Id == currentUserId);

            if (currentUser == null)
            {
                throw new Exception("User not found");
            }

            currentUser.PasswordHash = BC.HashPassword(password);
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

        public async Task<List<User>> GetUserFriendsList(int userId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var friendIds = await context.Friends
                .Where(fr => fr.FriendStatusId == (int)FriendStatusEnum.InFriends && (fr.UserFirstId == userId || fr.UserSecondId == userId))
                .Select(fr => fr.UserFirstId == userId ? fr.UserSecondId : fr.UserFirstId)
                .ToListAsync();

            return await context.Users
                    .Where(u => friendIds.Contains(u.Id))
                    .ToListAsync();
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

        public async Task<bool> ChangeUserMainInfo(User mappedUser)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            context.Users.Update(mappedUser);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TeamInvitation>> GetUserTeamInvitationsAsync(int userId)
        {
            using var context = new TaskManagerContext(_rapaportConnectionString);

            var teamInvtitations = await context.TeamInvitations.Where(e => e.UserToInviteId == userId).ToListAsync();

            return teamInvtitations;
        }

        #endregion
    }
}
