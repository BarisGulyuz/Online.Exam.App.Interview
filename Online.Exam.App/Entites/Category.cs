using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online.Exam.App.Entites
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Exams = new List<Exams>();
        }
        public string Name { get; set; }
        public List<Exams> Exams { get; set; }
    }
}
