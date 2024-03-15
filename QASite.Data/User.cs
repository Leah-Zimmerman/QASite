namespace QASite.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }


        public List<AnswerUser> AnswerUsers { get; set; }
        public List<Question> Questions { get; set; }
        public List<QuestionUser> QuestionUsers { get; set; }

    }
}