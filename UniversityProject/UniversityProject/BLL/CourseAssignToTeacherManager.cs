using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityProject.DAL;
using UniversityProject.Models;

namespace UniversityProject.BLL
{
    public class CourseAssignToTeacherManager
    {
        private CourseAssignToTeacherGateway courseAssignToTeacherGateway=new CourseAssignToTeacherGateway();
        public List<CourseAssignToTeacher> GetAllTeacherss()
        {
            return courseAssignToTeacherGateway.GetAllTeacherss();
        }

        public string Save(CourseAssignToTeacher courseAssign)
        {
            if(courseAssignToTeacherGateway.IsExistsCourse(courseAssign.CourseCodeId))
            {
                return "This Course Already assign to a Teacher";
            }
            int rawAffected = courseAssignToTeacherGateway.Save(courseAssign);
            if (rawAffected > 0)
            {
                return "Course Assign To Teacher SuccessFull";

            }

            return "Failed to Save";
        }
        public string UnAssignAllCourses()
        {
            int rawAffected = courseAssignToTeacherGateway.UnAssignAllCourses();
            if (rawAffected>0)
            {
                return "UnAssign Completed";
            }
            return "Failed";
        }
    }
}