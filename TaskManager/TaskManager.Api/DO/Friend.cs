using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class Friend
    {
        public int Id { get; set; }
        public int UserFirstId { get; set; }
        public int UserSecondId { get; set; }
        public int FriendStatusId { get; set; }

        public virtual FriendStatus FriendStatus { get; set; } = null!;
        public virtual User UserFirst { get; set; } = null!;
        public virtual User UserSecond { get; set; } = null!;
    }
}
