namespace QASite.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
        public int UserId { get; set; }
        public int Likes { get; set; }

        public User User { get; set; }

        public List<QuestionTag> QuestionTags { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
        public List<QuestionUser> QuestionUsers { get; set; }
    }
}