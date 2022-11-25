using Microsoft.EntityFrameworkCore;

namespace TaskManager.Api.DO
{
    public partial class TaskManagerContext : DbContext
    {
        private readonly string _connectionString;

        public TaskManagerContext()
        {
        }

        public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
            : base(options)
        {
        }

        public TaskManagerContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<Friend> Friends { get; set; } = null!;
        public virtual DbSet<FriendStatus> FriendStatuses { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<TaskAssignment> TaskAssignments { get; set; } = null!;
        public virtual DbSet<TaskPriority> TaskPriorities { get; set; } = null!;
        public virtual DbSet<TaskStatus> TaskStatuses { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<TeamInvitation> TeamInvitations { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserTeam> UserTeams { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>(entity =>
            {
                entity.ToTable("Friend");

                entity.HasOne(d => d.FriendStatus)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.FriendStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Friend__FriendSt__3F466844");

                entity.HasOne(d => d.UserFirst)
                    .WithMany(p => p.FriendUserFirsts)
                    .HasForeignKey(d => d.UserFirstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Friend__UserFirs__3D5E1FD2");

                entity.HasOne(d => d.UserSecond)
                    .WithMany(p => p.FriendUserSeconds)
                    .HasForeignKey(d => d.UserSecondId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Friend__UserSeco__3E52440B");
            });

            modelBuilder.Entity<FriendStatus>(entity =>
            {
                entity.ToTable("FriendStatus");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateScheduled).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK__Task__ParentId__4E88ABD4");

                entity.HasOne(d => d.TaskPriority)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TaskPriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Task__TaskPriori__4CA06362");

                entity.HasOne(d => d.TaskStatus)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TaskStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Task__TaskStatus__4D94879B");
            });

            modelBuilder.Entity<TaskAssignment>(entity =>
            {
                entity.ToTable("TaskAssignment");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskAssignments)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TaskAssig__TaskI__5165187F");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TaskAssignments)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TaskAssig__TeamI__534D60F1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaskAssignments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TaskAssig__UserI__52593CB8");
            });

            modelBuilder.Entity<TaskPriority>(entity =>
            {
                entity.ToTable("TaskPriority");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TaskStatus>(entity =>
            {
                entity.ToTable("TaskStatus");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.TeamName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team__CreatorId__4222D4EF");
            });

            modelBuilder.Entity<TeamInvitation>(entity =>
            {
                entity.ToTable("TeamInvitation");

                entity.HasOne(d => d.Inviter)
                    .WithMany(p => p.TeamInvitationInviters)
                    .HasForeignKey(d => d.InviterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeamInvit__Invit__5629CD9C");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamInvitations)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeamInvit__TeamI__5812160E");

                entity.HasOne(d => d.UserToInvite)
                    .WithMany(p => p.TeamInvitationUserToInvites)
                    .HasForeignKey(d => d.UserToInviteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeamInvit__UserT__571DF1D5");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.PhoneNumber, "UQ__User__85FB4E388C50E09B")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534BA0A36B3")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "UQ__User__C9F28456A17659DD")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.ToTable("UserTeam");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.UserTeams)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserTeam__TeamId__44FF419A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTeams)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserTeam__UserId__45F365D3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
