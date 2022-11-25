using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class Team
    {
        public Team()
        {
            TaskAssignments = new HashSet<TaskAssignment>();
            TeamInvitations = new HashSet<TeamInvitation>();
            UserTeams = new HashSet<UserTeam>();
        }

        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string TeamName { get; set; } = null!;
        public DateTime DateCreated { get; set; }

        public virtual User Creator { get; set; } = null!;
        public virtual ICollection<TaskAssignment> TaskAssignments { get; set; }
        public virtual ICollection<TeamInvitation> TeamInvitations { get; set; }
        public virtual ICollection<UserTeam> UserTeams { get; set; }
    }
}
