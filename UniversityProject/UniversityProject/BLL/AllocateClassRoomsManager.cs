using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using UniversityProject.DAL;
using UniversityProject.Models;

namespace UniversityProject.BLL
{
    public class AllocateClassRoomsManager
    {
        AllocateClassRoomsGateway allocateClassRoomsGateway=new AllocateClassRoomsGateway();

        public string Save(ClassRoomAllocate classRoom)
        {
           
            var startTime = DateTime.Parse(classRoom.StartTime.ToString("HH:mm"));
            var endTime = DateTime.Parse(classRoom.EndTime.ToString("HH:mm"));
            
            if (endTime<startTime)
            {
                return "You Try To Allocated Next Day";
            }
            if (allocateClassRoomsGateway.IsExitsClassroom(classRoom.RoomId,startTime.ToString(),endTime.ToString(),classRoom.Day))
            {
                return "Already Allocated This Room at this time";
            }
            int rawAffected = allocateClassRoomsGateway.Save(classRoom);
            if (rawAffected>0)
            {
                return "Allocated Successfull";
;            }

            else
            {
                return "Failed to allocated";
            }
        }

        public List<RoomNumbers> GetRooms()
        {
            return allocateClassRoomsGateway.GetRooms();
        }

        public List<ViewClassRoomAllocation> GetSchedulInfo()
        {
            return allocateClassRoomsGateway.GetSchedulInfo();
        }

        public string UnAllocateRooms()
        {
            int rawAffected = allocateClassRoomsGateway.UnAllocateRooms();
            if (rawAffected > 0)
            {
                return "Unallocate All Room Completed";
            }
            return "Failed";
        }
    }
}