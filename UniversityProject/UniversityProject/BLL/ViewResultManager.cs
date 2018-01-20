using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityProject.DAL;
using UniversityProject.Models;

namespace UniversityProject.BLL
{
    public class ViewResultManager
    {
        private ViewStudentResultGateway viewStudentResultGateway=new ViewStudentResultGateway();
        public List<ViewResult> GetAll()
        {
            return viewStudentResultGateway.GetAll();
        }
    }
}