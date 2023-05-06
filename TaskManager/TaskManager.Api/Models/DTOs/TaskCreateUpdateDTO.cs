namespace TaskManager.Api.Models.DTOs
{
    public class TaskCreateUpdateDTO
    {
        public int Id { get; set; }
        public int TaskPriorityId { get; set; }
        public int TaskStatusId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateScheduled { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public int? TeamMemberUserId { get;set; }
        public int? TeamId { get;set; } //if null - assign to current User
    }
}
