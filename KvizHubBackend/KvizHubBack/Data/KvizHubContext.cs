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
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

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
                      .ValueGeneratedOnAdd();
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

            // --- QUIZ ATTEMPTS ---
            modelBuilder.Entity<QuizAttempt>(entity =>
            {
                entity.ToTable("QuizAttempts");
                entity.HasKey(qa => qa.Id);
                entity.Property(qa => qa.Id)
                      .HasColumnName("Id")
                      .HasColumnType("NUMBER")
                      .ValueGeneratedOnAdd();

                entity.HasOne(qa => qa.Quiz)
                      .WithMany(q => q.QuizAttempts)
                      .HasForeignKey(qa => qa.QuizId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(qa => qa.User)
                      .WithMany(u => u.QuizAttempts)
                      .HasForeignKey(qa => qa.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- USER ANSWERS ---
            modelBuilder.Entity<UserAnswer>(entity =>
            {
                entity.ToTable("UserAnswers");
                entity.HasKey(ua => ua.Id);
                entity.Property(ua => ua.Id)
                      .HasColumnName("Id")
                      .HasColumnType("NUMBER")
                      .ValueGeneratedOnAdd();

                entity.HasOne(ua => ua.QuizAttempt)
                      .WithMany(qa => qa.UserAnswers)
                      .HasForeignKey(ua => ua.QuizAttemptId)
                      .OnDelete(DeleteBehavior.Cascade);


                // SelectedAnswerIds je lista, u bazi može biti JSON ili drugi tip
                entity.Ignore(ua => ua.SelectedAnswerIds);
            });
        }
    }
}
