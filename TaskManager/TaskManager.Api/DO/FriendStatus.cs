using System;
using System.Collections.Generic;

namespace TaskManager.Api.DO
{
    public partial class FriendStatus
    {
        public FriendStatus()
        {
            Friends = new HashSet<Friend>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Friend> Friends { get; set; }
    }
}
