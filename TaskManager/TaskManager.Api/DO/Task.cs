using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class Task
    {
        public Task()
        {
            InverseParent = new HashSet<Task>();
            TaskAssignments = new HashSet<TaskAssignment>();
        }

        public int Id { get; set; }
        public int TaskPriorityId { get; set; }
        public int TaskStatusId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateScheduled { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }

        public virtual Task? Parent { get; set; }
        public virtual TaskPriority TaskPriority { get; set; } = null!;
        public virtual TaskStatus TaskStatus { get; set; } = null!;
        public virtual ICollection<Task> InverseParent { get; set; }
        public virtual ICollection<TaskAssignment> TaskAssignments { get; set; }
    }
}
