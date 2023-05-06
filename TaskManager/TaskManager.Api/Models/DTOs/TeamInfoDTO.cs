using TaskManager.Api.DO;

namespace TaskManager.Api.Models.DTOs
{
    public class TeamInfoDTO
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string TeamName { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public List<BaseUserDTO> TeamMembers { get; set; }
    }
}
