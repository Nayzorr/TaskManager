using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class TaskPriority
    {
        public TaskPriority()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
