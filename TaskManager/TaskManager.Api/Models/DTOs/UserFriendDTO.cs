using TaskManager.Api.Enums;

namespace TaskManager.Api.Models.DTOs
{
    public class UserFriendDTO
    {
        public string UserNameToChangeStatus { get; set; }
        public FriendStatusEnum FriendStatus { get; set; }
    }
}
