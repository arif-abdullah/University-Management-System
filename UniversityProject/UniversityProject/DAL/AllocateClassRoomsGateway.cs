using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using UniversityProject.Models;

namespace UniversityProject.DAL
{
    public class AllocateClassRoomsGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["UniversityDB"].ConnectionString;
        
        public int Save(ClassRoomAllocate classRoom)
        {
            string query = "INSERT INTO AllocateClassRooms (DepartmentId,CourseId,RoomNoId,Day,StartTime,EndTime) VALUES ('" + classRoom.DepartmentId + "','" + classRoom.CourseId + "','" + classRoom.RoomId + "','" + classRoom.Day + "','" + classRoom.StartTime + "','" + classRoom.EndTime + "')";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            int rawAffected = command.ExecuteNonQuery();
            connection.Close();
            return rawAffected;
        }
        public List<RoomNumbers> GetRooms()
        {
            string query = " Select * from ClassRooms";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<RoomNumbers> list = new List<RoomNumbers>();
            while (reader.Read())
            {
                RoomNumbers room = new RoomNumbers();
                room.Id = (int)reader["Id"];
                room.RoomNumber = reader["RoomNumber"].ToString();


                list.Add(room);
            }
            reader.Close();
            connection.Close();
            return list;
        }

        public bool IsExitsClassroom(int roomId, string clsStrTime,string clsEndTime,string day)
        {
            string query = "select * from AllocateClassRooms where RoomNoId="+roomId+" and Day='"+day+"' and (CAST(StartTime as time) >= '" + clsStrTime + "' And CAST(StartTime as time) <= '" + clsEndTime + "' or CAST(EndTime as time) >= '" + clsStrTime + "')";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            bool isExist = reader.HasRows;
            reader.Close();
            connection.Close();
            return isExist;
        }
        public List<ViewClassRoomAllocation> GetSchedulInfo()
        {
            string query = "select * from ViewClassAllocation";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<ViewClassRoomAllocation> list = new List<ViewClassRoomAllocation>();
            while (reader.Read())
            {
                ViewClassRoomAllocation room = new ViewClassRoomAllocation();
                //room.Id = (int)reader["Id"];
                room.DepartmentId = (int) reader["DepartmentId"];
                room.CourseCode = reader["CourseCode"].ToString();
                room.CourseName = reader["CourseName"].ToString();
                room.ScheduleInfo = reader["ScheduleInfo"].ToString();

                list.Add(room);
            }
            reader.Close();
            connection.Close();
            return list;
        }

        public int UnAllocateRooms()
        {
            string query = "select * into AllocateClass"+DateTime.Now.Millisecond+" from AllocateClassRooms delete AllocateClassRooms";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            int rawAffected = command.ExecuteNonQuery();
            connection.Close();
            return rawAffected;
        }

    }
}