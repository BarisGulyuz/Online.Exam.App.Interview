using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online.Exam.App.DataAccess.Repository;
using Online.Exam.App.Entites;
using Online.Exam.App.Entites.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Online.Exam.App.Controllers
{

    [Authorize]
    public class ExamController : Controller
    {
        private readonly IRepository<Exams> _examRepository;
        private readonly IRepository<UserExam> _userExamRepository;

        public ExamController(IRepository<Exams> examRepository, IRepository<UserExam> userExamRepository)
        {
            _examRepository = examRepository;
            _userExamRepository = userExamRepository;
        }
        public int UserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = UserId();
            var userExams = await _userExamRepository.GetAllAsync(x => x.UserId == userId);

            List<UserExamVM> userExamVMs = new List<UserExamVM>();
            foreach (var item in userExams)
            {
                var exam = await _examRepository.GetAsync(filter: x => x.Id == item.ExamId,
                    x => x.Category,
                    x => x.ExamQuestions);
                userExamVMs.Add(new UserExamVM
                {
                    Id = exam.Id,
                    CategoryName = exam.Category.Name,
                    Name = exam.Name,
                    Description = exam.Description,
                    ExamDuration = exam.ExamDuration,
                    AchievementScore = exam.AchievementScore,
                    QuestionCount = exam.ExamQuestions.Count(),
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Status = item.Status

                });

            }

            return View(userExamVMs.Where(x => x.Status).ToList());
        }
        [HttpGet]
        public async Task<IActionResult> GetExamQuestions(int examId, int questionNumber = 0)
        {
            ViewBag.ExamId = examId;

            var exam = await _examRepository.GetAsync(x => x.Id == examId, x => x.ExamQuestions);
            var questions = exam.ExamQuestions.ToList();
            ViewBag.ExamQuestionCount = questions.Count();
            return View(questions[questionNumber]);
        }

        [HttpPost]
        public async Task<IActionResult> FinishExam(int examId, List<QuestionAnswersVM> questionAnswers)
        {
            float points = 0;
            bool status;

            var exam = await _examRepository.GetAsync(x => x.Id == examId, x => x.ExamQuestions);
            var questions = exam.ExamQuestions.ToList();

            int questionCount = questions.Count();
            float pointPerQuestion = 100 / (float)questionCount;

            for (int i = 0; i < questions.Count(); i++)
            {
                if (questions[i].Answer == questionAnswers[i].Answer) points += pointPerQuestion;
            }

            if (points >= exam.AchievementScore) status = true;
            else status = false;

            var userExam = await _userExamRepository.GetAsync(x => x.ExamId == examId && x.UserId == UserId());
            userExam.Status = false;
            await _userExamRepository.UpdateAsync(userExam);

            return Json(new ResultPageVM
            {
                Point = points,
                SuccessStatus = status
            });

        }

        [HttpGet]
        public IActionResult ResultPage(int point, bool status)
        {

            string message = "";
            if (status)
            {
                message = "Tebrikler Sınavı Başarıyla Tamamladınız. Anasayfadan Aktif Sınavları ve Geçmiş Sınavılarınız Puanlarını Görüntüleyebilirsiniz";
            }
            if (!status)
            {
                message = "Ne Yazık Ki Sınav Sonucunuz Başarısız. Anasayfadan Aktif Sınavları ve Geçmiş Sınavılarınız Puanlarını Görüntüleyebilirsiniz";
            }
            return View(new ResultPageVM
            {
                Point = point,
                SuccessStatus = status,
                Message = message
            });
        }
    }
}
