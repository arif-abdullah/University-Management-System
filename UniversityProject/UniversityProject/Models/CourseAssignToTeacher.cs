using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityProject.Models
{
    public class CourseAssignToTeacher
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int TeacherId { get; set; }

        public double CreditLimit { get; set; }
        public double RemainingCredit { get; set; }
        public int CourseCodeId { get; set; }

        public string CourseCode { get; set; }
        public double Credit { get; set; }

    }
}