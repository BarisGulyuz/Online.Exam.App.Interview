using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online.Exam.App.Entites
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool Status { get; set; } = true;
    }
}
