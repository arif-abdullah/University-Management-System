using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityProject.Models
{
    public class ViewCourseStatics
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }

        public string Semester { get; set; }
        public string AssignTo { get; set; }


    }
}