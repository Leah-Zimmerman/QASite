using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QASite.Data;
using System.Diagnostics;

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
            return Json(repo.GetQuestions());
        }
        public IActionResult GetTagsForQuestion(Question question)
        {
            var repo = new QuestionRepository(_connectionString);
            return Json(repo.GetTagsForQuestion(question));
        }
        public IActionResult GetInfoForQuestion(Question question)
        {
            var repo = new QuestionRepository(_connectionString);
            var tags = repo.GetTagsForQuestion(question);
            var count = tags.Count();
            return Json(new HomePageViewModel
            {
                Tags = tags,
                Likes = question.Likes,
                AnswerCount = tags.Count()
            });
        }

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
                UserName = repo.GetUserNameById(question.UserId),
                Tags = repo.GetTagsForQuestion(question),
                Likes = question.Likes,
                Answers = repo.GetAnswersForQuestion(id)
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


    }
}