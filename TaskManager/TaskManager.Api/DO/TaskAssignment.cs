using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class TaskAssignment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }

        public virtual Task Task { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
