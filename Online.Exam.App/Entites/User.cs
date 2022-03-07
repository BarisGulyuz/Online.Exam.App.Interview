using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online.Exam.App.Entites
{
    public class User:BaseEntity
    {
        public User()
        {
            Exams = new List<Exams>();
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Exams> Exams { get; set; }
    }
}
