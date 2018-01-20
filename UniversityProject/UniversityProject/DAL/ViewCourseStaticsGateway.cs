using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using UniversityProject.Models;

namespace UniversityProject.DAL
{
    public class ViewCourseStaticsGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["UniversityDB"].ConnectionString;
        public List<ViewCourseStatics> GetAll()
        {
            string query = "Select * from ViewCourseStatics";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<ViewCourseStatics> listOfTeachers = new List<ViewCourseStatics>();
            while (reader.Read())
            {
                ViewCourseStatics viewCourseStatics = new ViewCourseStatics();
                viewCourseStatics.Id = (int)reader["Id"];
                viewCourseStatics.CourseCode = reader["CourseCode"].ToString();
                viewCourseStatics.CourseName = reader["CourseName"].ToString();
                viewCourseStatics.Semester = reader["Semester"].ToString();
                viewCourseStatics.AssignTo = reader["AssignTo"].ToString();
                

                listOfTeachers.Add(viewCourseStatics);
            }
            reader.Close();
            connection.Close();
            return listOfTeachers;
        }

        public List<ViewCourseStatics> GetAll(int id)
        {
            string query = "Select * from ViewCourseStatics where Id="+id+"";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<ViewCourseStatics> listOfTeachers = new List<ViewCourseStatics>();
            while (reader.Read())
            {
                ViewCourseStatics viewCourseStatics = new ViewCourseStatics();
                viewCourseStatics.Id = (int)reader["Id"];
                viewCourseStatics.CourseCode = reader["CourseCode"].ToString();
                viewCourseStatics.CourseName = reader["CourseName"].ToString();
                viewCourseStatics.Semester = reader["Semester"].ToString();
                viewCourseStatics.AssignTo = reader["AssignTo"].ToString();


                listOfTeachers.Add(viewCourseStatics);
            }
            reader.Close();
            connection.Close();
            return listOfTeachers;
        }
    }
}