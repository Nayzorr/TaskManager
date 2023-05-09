namespace TaskManager.Api.Models.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public int TaskPriorityId { get; set; }
        public string TaskPriority { get; set; }
        public int TaskStatusId { get; set; }
        public string TaskStatus { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateScheduled { get; set; }
        public List<int> AssignedToUserIds { get; set; }
        public int? TeamId { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public List<TaskDTO> SubTasks { get; set; }
    }
}
