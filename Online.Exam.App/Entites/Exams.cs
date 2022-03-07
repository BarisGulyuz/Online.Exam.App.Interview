using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online.Exam.App.Entites
{
    public class Exams : BaseEntity
    {
        public Exams()
        {
            ExamQuestions = new List<ExamQuestion>();
            Users = new List<User>();
        }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ExamDuration { get; set; }
        public int AchievementScore { get; set; }
        public List<User> Users { get; set; }
        public List<ExamQuestion> ExamQuestions { get; set; }
    }
}
