using TaskManager.Api.Enums;

namespace TaskManager.Api.Models.DTOs
{
    public class UserFriendStatusDTO
    {
        public string UserNameToChangeStatus { get; set; }
        public FriendStatusEnum FriendStatus { get; set; }
    }
}
