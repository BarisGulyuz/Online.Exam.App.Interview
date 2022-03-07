using Microsoft.EntityFrameworkCore;
using Online.Exam.App.Entites;

namespace Online.Exam.App.DataAccess.Context
{
    public class ExamContext : DbContext
    {
        public ExamContext(DbContextOptions<ExamContext> options) : base(options)
        {

        }
        public DbSet<Exams> Exams { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserExam> UserExams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exams>().HasMany(x => x.Users)
                .WithMany(x => x.Exams)
                .UsingEntity<UserExam>(
                    x => x.HasOne(x => x.User)
                    .WithMany().HasForeignKey(x => x.UserId),
                    x => x.HasOne(x => x.Exam)
                   .WithMany().HasForeignKey(x => x.ExamId));
            base.OnModelCreating(modelBuilder);
        }
    }
}
