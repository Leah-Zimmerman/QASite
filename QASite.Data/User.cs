using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace QASite.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<Like> Likes { get; set; } = new List<Like>();

        public List<AnswerUser> AnswerUsers { get; set; }
        //public List<Question> Questions { get; set; }
        public List<QuestionUser> QuestionUsers { get; set; }

    }
    public class Like
    {
        public int Id { get; set; }
        public int Value { get; set; } // Represents the value of the like
        public int UserId { get; set; } // Foreign key to the User
        public User User { get; set; } // Navigation property to the User
    }
}