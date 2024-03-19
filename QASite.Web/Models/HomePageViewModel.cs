using QASite.Data;

namespace QASite.Web.Models
{
    public class HomePageViewModel
    {
        public List<Tag>? Tags { get; set; }
        public int Likes { get; set; }
        public List<Answer>? Answers { get; set; }
        public int AnswerCount { get; set; }
        public List<QuestionTag> QuestionTags { get; set; }
        public Question Question { get; set; }
        
    }
}
