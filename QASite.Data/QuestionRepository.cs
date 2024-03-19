using Microsoft.EntityFrameworkCore;

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
            context.QuestionUsers.Add(new QuestionUser
            {
                QuestionId = question.Id,
                UserId = user.Id
            });
            context.SaveChanges();

        }
        public List<Question> GetQuestions()
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Questions.OrderByDescending(q => q.DatePosted).ToList();
        }
        public List<QuestionTag> GetQuestionTags()
        {
            var context = new StackOverflowContext(_connectionString);
            return context.QuestionTags.OrderByDescending(qt => qt.Question.DatePosted)
                .Include(q => q.Question)
                .Include(q => q.Tag)
                .ToList();
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
        public List<AnswerUser> GetAnswerUsersByQuestionId(int questionId)
        {
            var repo = new QuestionRepository(_connectionString);
            var context = new StackOverflowContext(_connectionString);
            return (List<AnswerUser>)context.AnswerUsers.Where(au => au.Answer.QuestionId == questionId)
                .Include(au => au.User)
                .Include(au => au.Answer)
                //.ThenInclude(a => a.QuestionId)
                .ToList();
        }

        public List<QuestionTag> GetQuestionTagsByQuestionId(int questionId)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.QuestionTags.Where(qt => qt.Question.Id == questionId)
                .Include(qt => qt.Question)
                .Include(qt => qt.Tag)
                .ToList();
        }
        //public List<QuestionUser> GetQuestionUsersByQuestionId(int questionId)
        //{
        //    var context = new StackOverflowContext(_connectionString);
        //    return context.QuestionUsers.Where(qu => qu.Question.Id == questionId)
        //        .Include(qu => qu.Question)
        //        .Include(qu => qu.User)
        //        .ToList();
        //}
        public QuestionUser GetQuestionUsersByQuestionId(int questionId)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.QuestionUsers.Include(qu => qu.Question).Include(qu => qu.User).FirstOrDefault(qu => qu.Question.Id == questionId);

        }
        public void IncreaseLikes(int id)
        {
            var repo = new QuestionRepository(_connectionString);
            var context = new StackOverflowContext(_connectionString);
            //var question = repo.GetQuestionById(id);
            //question.Likes++;
            context.Questions.FirstOrDefault(q => q.Id == id).Likes++;
            context.SaveChanges();

        }
        public List<Like> GetUserLikes(int userId)
        {
            var context = new StackOverflowContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Id == userId).Likes.ToList();
        }
        public void AddLike(int userId, int id)
        {
            var context = new StackOverflowContext(_connectionString);
            context.Users.FirstOrDefault(u => u.Id == userId).Likes.Add(new Like
            {
                Value = id,
                UserId = userId
            });
            context.SaveChanges();
        }
        public void NewItUp(int userId)
        {
            var context = new StackOverflowContext(_connectionString);
            context.Users.FirstOrDefault(u => u.Id == userId).Likes = new List<Like>();
            context.SaveChanges();
        }
        public bool HasThisValue(int userId, int id)
        {
            var context = new StackOverflowContext(_connectionString);
            var users = context.Users.FirstOrDefault(u => u.Id == userId);
            return users.Likes.Any(l => l.Value == id);
        }
    }
}