using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online.Exam.App.Entites
{
    public class UserExam : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ExamId { get; set; }
        public Exams Exam { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
