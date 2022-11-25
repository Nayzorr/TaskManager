using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class User
    {
        public User()
        {
            FriendUserFirsts = new HashSet<Friend>();
            FriendUserSeconds = new HashSet<Friend>();
            TaskAssignments = new HashSet<TaskAssignment>();
            TeamInvitationInviters = new HashSet<TeamInvitation>();
            TeamInvitationUserToInvites = new HashSet<TeamInvitation>();
            Teams = new HashSet<Team>();
            UserTeams = new HashSet<UserTeam>();
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Friend> FriendUserFirsts { get; set; }
        public virtual ICollection<Friend> FriendUserSeconds { get; set; }
        public virtual ICollection<TaskAssignment> TaskAssignments { get; set; }
        public virtual ICollection<TeamInvitation> TeamInvitationInviters { get; set; }
        public virtual ICollection<TeamInvitation> TeamInvitationUserToInvites { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<UserTeam> UserTeams { get; set; }
    }
}
