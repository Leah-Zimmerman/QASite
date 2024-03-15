namespace QASite.Data
{
    public class Tag
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public List<QuestionTag> QuestionTags { get; set; }
    }
}