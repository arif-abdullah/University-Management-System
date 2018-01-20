using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using UniversityProject.Models;

namespace UniversityProject.DAL
{
    public class CourseAssignToTeacherGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["UniversityDB"].ConnectionString;

        public List<CourseAssignToTeacher> GetAllTeacherss()
        {
           
            string query = "select t.Id as TeacherId,t.CreditLimit,ISnull(co.RemainingCredit,0) as RemainingCredit from Teachers t left join CourseAssignToTeacherView co on co.TeacherId=t.Id";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<CourseAssignToTeacher> listOfTeachers = new List<CourseAssignToTeacher>();
            while (reader.Read())
            {
                CourseAssignToTeacher teacher = new CourseAssignToTeacher();
                teacher.Id = (int)reader["TeacherId"];
                teacher.CreditLimit = Convert.ToDouble(reader["CreditLimit"].ToString());
                teacher.RemainingCredit = teacher.CreditLimit - Convert.ToDouble(reader["RemainingCredit"].ToString());
                
                listOfTeachers.Add(teacher);
            }
            reader.Close();
            connection.Close();
            return listOfTeachers;
        }

        public int Save(CourseAssignToTeacher courseAssign)
        {
            string query = "INSERT INTO CourseAssignToTeacher (DepartmentId,TeacherId,CourseCodeId) VALUES (" + courseAssign.DepartmentId + "," + courseAssign.TeacherId + ","+courseAssign.CourseCodeId+")";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            int rawAffected = command.ExecuteNonQuery();
            connection.Close();
            return rawAffected;
        }

        public bool IsExistsCourse(int  courseCodeId)
        {
            string query = "SELECT * FROM CourseAssignToTeacher where CourseCodeId= " + courseCodeId + " ";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            bool isExists = reader.HasRows;

            reader.Close();
            connection.Close();
            return isExists;
        }

        public int UnAssignAllCourses()
        {
            string query = "select * into CourseAssignTeacher"+DateTime.Now.Millisecond+" from CourseAssignToTeacher delete CourseAssignToTeacher ";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
        
            int rawAffected = command.ExecuteNonQuery();
            connection.Close();
            return rawAffected;
        }
    }
}