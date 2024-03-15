namespace QASite.Data
{
    public class AnswerUser
    {
        public int AnswerId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public Answer Answer { get; set; }
    }
}