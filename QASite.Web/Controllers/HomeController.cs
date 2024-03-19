using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QASite.Data;
using QASite.Web.Models;
using System.Diagnostics;
using System.Text.Json;

namespace StackOverflow.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        public HomeController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConStr");
            _connectionString = connectionString;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetQuestions()
        {
            var repo = new QuestionRepository(_connectionString);
            var q = repo.GetQuestions();
            return Json(repo.GetQuestions());
        }
        public IActionResult GetQuestionTags()
        {
            var repo = new QuestionRepository(_connectionString);
            return Json(repo.GetQuestions());
        }
        //public IActionResult GetTagsForQuestion(Question question)
        //{
        //    var repo = new QuestionRepository(_connectionString);
        //    return Json(repo.GetTagsForQuestion(question));
        //}

        [Authorize]
        public IActionResult AskAQuestion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitQuestion(Question question, List<string> tags)
        {
            var qrepo = new QuestionRepository(_connectionString);
            var urepo = new UserRepository(_connectionString);
            var currentUserEmail = User.Identity.Name;
            var user = urepo.GetUserByEmail(currentUserEmail);
            qrepo.AddQuestionAndTags(question, tags, user);
            return Redirect("/");
        }
        public IActionResult ViewQuestion(int id)
        {
            var repo = new QuestionRepository(_connectionString);
            var question = repo.GetQuestionById(id);

            return View(new ViewQuestionViewModel
            {
                Question = question,
                AnswerUsers = repo.GetAnswerUsersByQuestionId(id),
                QuestionTags = repo.GetQuestionTagsByQuestionId(id),
                QuestionUser = repo.GetQuestionUsersByQuestionId(id)
            });
        }
        [Authorize]
        public IActionResult SubmitAnswer(int questionId, string text)
        {
            var qrepo = new QuestionRepository(_connectionString);
            var urepo = new UserRepository(_connectionString);
            var currentUserEmail = User.Identity.Name;
            var user = urepo.GetUserByEmail(currentUserEmail);
            qrepo.SubmitAnswer(questionId, text, user);
            return Redirect($"/home/viewquestion?id={questionId}");
        }
        public IActionResult GetInfoForQuestion(Question question)
        {
            var repo = new QuestionRepository(_connectionString);
            var tags = repo.GetTagsForQuestion(question);
            var count = tags.Count();
            var answerCount = repo.GetAnswersForQuestion(question.Id).Count();
            return Json(new HomePageViewModel
            {
                Tags = tags,
                Likes = question.Likes,
                AnswerCount = answerCount
            });
        }
        public IActionResult IncreaseLikes(int id)
        {
            var urepo = new UserRepository(_connectionString);
            var currentUserEmail = User.Identity.Name;
            var user = urepo.GetUserByEmail(currentUserEmail);
            var repo = new QuestionRepository(_connectionString);
            repo.IncreaseLikes(id);
            return Redirect($"/home/viewquestion?id={id}");
        }
        [HttpPost]
        public IActionResult AddToSession(int id)
        {
            var urepo = new UserRepository(_connectionString);
            var qrepo = new QuestionRepository(_connectionString);
            var currentUserEmail = User.Identity.Name;
            var user = urepo.GetUserByEmail(currentUserEmail);
            var userLikes = qrepo.GetUserLikes(user.Id);
            if (userLikes == null)
            {
                qrepo.NewItUp(user.Id);
            }
            else if(qrepo.HasThisValue(user.Id,id))
            {
                return Redirect($"/home/viewimage?id={id}");
            }
            qrepo.AddLike(user.Id, id);
            //HttpContext.Session.Set("Session", sessionLikes);
            return Redirect($"/home/increaselikes?id={id}");
        }


    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }

}


