using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Online.Exam.App.DataAccess.Repository;
using Online.Exam.App.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online.Exam.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExamController : Controller
    {
        private readonly IRepository<Exams> _examRepository;
        private readonly IRepository<Category> _categorRrepository;
        private readonly IRepository<User> _userRrepository;
        private readonly IRepository<UserExam> _userExamRepository;


        public ExamController(IRepository<Exams> examRepository, IRepository<Category> categorRrepository, IRepository<User> userRrepository, IRepository<UserExam> userExamRepository)
        {
            _examRepository = examRepository;
            _categorRrepository = categorRrepository;
            _userRrepository = userRrepository;
            _userExamRepository = userExamRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoires = await _categorRrepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categoires, "Id", "Name");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetExams()
        {
            var exams = await _examRepository.GetAllAsync(filter: x => x.Status, includes: x => x.Category);
            return Json(exams);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Exams exam)
        {
            if (ModelState.IsValid)
            {
                exam.Status = true;
                await _examRepository.AddAsync(exam);
                return Json(exam);
            }
            else
            {
                return Json("Validation Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _examRepository.GetAsync(x => x.Id == id);
            if (exam != null)
            {
                await _examRepository.DeleteAsync(exam);
                return Json(exam);
            }
            else
            {
                return Json("Exam Does Not Exist");
            }

        }

        [HttpGet]
        public async Task<IActionResult> AssignExamToUser(int id)
        {
            var users = await _userRrepository.GetAllAsync();
            var dropdownUserList = new List<User>();
            foreach (var user in users)
            {
                var checkUserExam = await _userExamRepository.GetAsync(x => x.ExamId == id && x.UserId == user.Id);
                if (checkUserExam == null) dropdownUserList.Add(user);
            }

            ViewBag.Users = new SelectList(dropdownUserList, "Id", "UserName");
            ViewBag.ExamId = id;

            return PartialView("_AssignExamToUser");
        }

        [HttpPost]
        public async Task<IActionResult> AssignExamToUser(UserExam userExam)
        {
            if (ModelState.IsValid)
            {
                await _userExamRepository.AddAsync(userExam);
                return Json("");
            }
            return Json("Validasyon Hatası");

        }
    }
}
