using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Online.Exam.App.DataAccess.Repository;
using Online.Exam.App.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online.Exam.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController : Controller
    {
        private readonly IRepository<ExamQuestion> _examQuestionRepository;
        private readonly IRepository<QuestionType> _examQuestionTypeRepository;
        private readonly IRepository<Exams> _examRepository;

        public QuestionController(IRepository<ExamQuestion> examQuestionRepository, IRepository<QuestionType> examQuestionTypeRepository, IRepository<Exams> examRepository)
        {
            _examQuestionRepository = examQuestionRepository;
            _examQuestionTypeRepository = examQuestionTypeRepository;
            _examRepository = examRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int examId)
        {
            var exam = await _examRepository.GetAsync(x => x.Id == examId);

            ViewBag.ExamId = examId;
            var types = await _examQuestionTypeRepository.GetAllAsync();
            ViewBag.Types = new SelectList(types, "Id", "Name");
            ViewBag.ExamName = exam.Name.ToString();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(int id)
        {
            var questions = await _examQuestionRepository.GetAllAsync(x => x.ExamId == id);
            return Json(questions);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ExamQuestion examQuestion)
        {
            if (ModelState.IsValid)
            {
                examQuestion.Status = true;
                await _examQuestionRepository.AddAsync(examQuestion);
                return Json(examQuestion);
            }
            else
            {
                return Json("Validation Error");
            }
        }


    }
}
