namespace TaskManager.Api.Models.DTOs
{
    public class TaskCreateUpdateSelfUserDTO
    {
        public int Id { get; set; }
        public int TaskPriorityId { get; set; }
        public int TaskStatusId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateScheduled { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
    }
}
