using QASite.Data;

namespace QASite.Web.Models
{
    public class ViewQuestionViewModel
    {
        public Question Question { get; set; }
        public string UserName { get; set; }
        public List<Tag> Tags { get; set; }
        public int Likes { get; set; }
        public List<Answer> Answers { get; set; }
        public string AnswerUserName { get; set; }
        public List<AnswerUser> AnswerUsers { get; set; }
    }
}
