using Microsoft.EntityFrameworkCore;

namespace QASite.Data
{
    public class StackOverflowContext : DbContext
    {
        private string _connectionString;
        public StackOverflowContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<QuestionTag>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            modelBuilder.Entity<QuestionAnswer>()
               .HasKey(qt => new { qt.QuestionId, qt.AnswerId });
            modelBuilder.Entity<QuestionUser>()
                .HasKey(qt => new { qt.QuestionId, qt.UserId });
            modelBuilder.Entity<AnswerUser>()
                .HasKey(qt => new { qt.AnswerId, qt.UserId });
       //     modelBuilder.Entity<Like>()
       //.HasOne(l => l.User)
       //.WithMany(u => u.Likes) // Assuming Likes is the navigation property in the User entity
       //.HasForeignKey(l => l.UserId)
       //.IsRequired();


            modelBuilder.Entity<QuestionUser>()
        .HasOne(qu => qu.User)  // Each QuestionUser has one User
        .WithMany(u => u.QuestionUsers)  // Each User can have many QuestionUsers
        .HasForeignKey(qu => qu.UserId); // Foreign key for User in QuestionUser

            modelBuilder.Entity<QuestionUser>()
                .HasOne(qu => qu.Question)  // Each QuestionUser has one Question
                .WithMany(q => q.QuestionUsers)  // Each Question can have many QuestionUsers
                .HasForeignKey(qu => qu.QuestionId);

        }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AnswerUser> AnswerUsers { get; set; }
        public DbSet<QuestionUser> QuestionUsers { get; set; }
    }
}