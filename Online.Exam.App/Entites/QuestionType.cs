using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online.Exam.App.Entites
{
    public class QuestionType : BaseEntity
    {
        public QuestionType()
        {
            ExamQuestions = new List<ExamQuestion>();
        }
        public string Name { get; set; }
        public List<ExamQuestion> ExamQuestions { get; set; }
    }
}
