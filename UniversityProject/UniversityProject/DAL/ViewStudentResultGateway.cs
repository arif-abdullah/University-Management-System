using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using UniversityProject.Models;

namespace UniversityProject.DAL
{
    public class ViewStudentResultGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["UniversityDB"].ConnectionString;

        public List<ViewResult> GetAll()
        {
            string query = "select * from StdResultView121";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<ViewResult> list = new List<ViewResult>();
            while (reader.Read())
            {
                ViewResult view = new ViewResult();
                view.StudentId = (int)reader["StudentId"];
                view.Code = reader["Code"].ToString();
                view.Name = reader["Name"].ToString();
                view.Email = reader["Email"].ToString();
                
                view.CourseCode = reader["CourseCode"].ToString();
                view.CourseName = reader["CourseName"].ToString();
                view.Grade = reader["Grade"].ToString();
                list.Add(view);
            }
            reader.Close();
            connection.Close();
            return list;

        }


    }
}