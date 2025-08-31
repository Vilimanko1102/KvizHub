using KvizHubBack.Models;
using Microsoft.EntityFrameworkCore;

namespace KvizHubBack.Data
{
    public class KvizHubContext : DbContext
    {
        public KvizHubContext(DbContextOptions<KvizHubContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quiz>()
                .Property(q => q.Id)
                .UseIdentityColumn();  

            modelBuilder.Entity<Question>()
                .Property(q => q.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Answer>()
                .Property(a => a.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .UseIdentityColumn();
        }
    }
}
