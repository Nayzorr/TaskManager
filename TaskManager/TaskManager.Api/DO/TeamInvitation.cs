using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class TeamInvitation
    {
        public int Id { get; set; }
        public int InviterId { get; set; }
        public int UserToInviteId { get; set; }
        public int TeamId { get; set; }

        public virtual User Inviter { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
        public virtual User UserToInvite { get; set; } = null!;
    }
}
