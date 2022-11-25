using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class TaskStatus
    {
        public TaskStatus()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
