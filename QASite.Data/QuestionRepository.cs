namespace QASite.Data
{
    public class QuestionRepository
    {
        private string _connectionString;
        public QuestionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int AddTagAndGetId(string tag)
        {
            var tg = new Tag { Description = tag };
            var context = new StackOverflowContext(_connectionString);
            context.Tags.Add(tg);
            context.SaveChanges();
            return tg.Id;
        }
        public Tag GetTag(string tag)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Tags.FirstOrDefault(t => t.Description == tag);
        }
        public Tag GetTagById(int tagId)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Tags.FirstOrDefault(t => t.Id == tagId);
        }
        public void AddQuestionAndTags(Question question, List<string> tags, User user)
        {
            var context = new StackOverflowContext(_connectionString);
            question.DatePosted = DateTime.Now;
            question.UserId = user.Id;
            question.Likes = 0;
            context.Questions.Add(question);
            context.SaveChanges();
            foreach (var tag in tags)
            {
                int tagId;
                var t = GetTag(tag);
                if (t == null)
                {
                    tagId = AddTagAndGetId(tag);
                }
                else
                {
                    tagId = t.Id;
                }

                context.QuestionTags.Add(new QuestionTag
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });

            }
            context.SaveChanges();

        }

        public List<Question> GetQuestions()
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Questions.OrderByDescending(q => q.DatePosted).ToList();
        }
        public List<Tag> GetTagsForQuestion(Question question)
        {
            var repo = new QuestionRepository(_connectionString);
            var context = new StackOverflowContext(_connectionString);
            var questionTags = context.QuestionTags.Where(q => q.Question == question)
                .Include(q => q.Question)
                .ToList();
            var tagIds = questionTags.Select(qt => qt.TagId);
            var tags = new List<Tag>();
            foreach (var tagId in tagIds)
            {
                var tag = repo.GetTagById(tagId);
                tags.Add(tag);
            }
            return tags;
        }
        public Question GetQuestionById(int questionId)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Questions.FirstOrDefault(q => q.Id == questionId);
        }
        public string GetUserNameById(int userId)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Id == userId).Name;
        }
        public int AddAnswerandGetId(int questionId, string text, User user)
        {
            var context = new StackOverflowContext(_connectionString);
            var answer = new Answer
            {
                DatePosted = DateTime.Now,
                Text = text,
                QuestionId = questionId,
                UserId = user.Id
            };
            context.Answers.Add(answer);
            context.SaveChanges();
            return answer.Id;
        }
        public void SubmitAnswer(int questionId, string text, User user)
        {
            var repo = new QuestionRepository(_connectionString);
            var answerId = repo.AddAnswerandGetId(questionId, text, user);
            var context = new StackOverflowContext(_connectionString);
            context.QuestionAnswers.Add(new QuestionAnswer
            {
                QuestionId = questionId,
                AnswerId = answerId
            });
            context.SaveChanges();
            context.AnswerUsers.Add(new AnswerUser
            {
                AnswerId = answerId,
                UserId = user.Id
            });
            context.SaveChanges();
        }
        public List<Answer> GetAnswersForQuestion(int questionId)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Answers.Where(a => a.QuestionId == questionId).ToList();
        }
        public string GetAnswerUserNameById(int answerUserId)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Id == answerUserId).Name;
        }
        public List<AnswerUser> GetAnswersWithUserName(int questionId)
        {
            var repo = new QuestionRepository(_connectionString);
            var context = new StackOverflowContext(_connectionString);
            var answerUsers = new List<AnswerUser>();
            var answers = repo.GetAnswersForQuestion(questionId);
            foreach (var answer in answers)
            {
                var answerUser = context.AnswerUsers.FirstOrDefault(au => au.AnswerId == answer.Id);
                if (answerUser != null)
                {
                    answerUsers.Add(answerUser);
                }
            }
            return answerUsers;

        }
        public List<QuestionTag> GetQuestionTagsForQuestionId(int questionId)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.QuestionTags.Where(qt => qt.QuestionId == questionId)
                .Include(qt => qt.)
        }
    }
}