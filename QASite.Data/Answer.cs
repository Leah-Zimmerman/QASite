namespace QASite.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }

        public List<AnswerUser> AnswerUsers { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
    }
}