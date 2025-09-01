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


            // --- USERS ---
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                .HasColumnName("Id")
                      .HasColumnType("NUMBER")
                      .ValueGeneratedOnAdd(); // ne koristimo IDENTITY
            });

            // --- QUIZZES ---
            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.ToTable("Quizzes");
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Id)
                .HasColumnName("Id")
                      .HasColumnType("NUMBER")
                      .ValueGeneratedOnAdd();
            });

            // --- QUESTIONS ---
            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Questions");
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Id)
                .HasColumnName("Id")
                      .HasColumnType("NUMBER")
                      .ValueGeneratedOnAdd();

                entity.HasOne(q => q.Quiz)
                      .WithMany(qz => qz.Questions)
                      .HasForeignKey(q => q.QuizId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- ANSWERS ---
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answers");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id)
                .HasColumnName("Id")
                      .HasColumnType("NUMBER")
                      .ValueGeneratedOnAdd();

                entity.Property(a => a.IsCorrect)
                      .HasColumnType("NUMBER") // 0/1 umesto BOOLEAN
                      .IsRequired();

                entity.HasOne(a => a.Question)
                      .WithMany(q => q.Answers)
                      .HasForeignKey(a => a.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
