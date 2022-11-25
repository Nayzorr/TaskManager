using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class UserTeam
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }

        public virtual Team Team { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
