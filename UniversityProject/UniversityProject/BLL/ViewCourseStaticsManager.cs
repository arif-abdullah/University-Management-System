using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityProject.DAL;
using UniversityProject.Models;

namespace UniversityProject.BLL
{
    public class ViewCourseStaticsManager
    {
        private ViewCourseStaticsGateway viewCourseStaticsGateway=new ViewCourseStaticsGateway();

        public List<ViewCourseStatics> GetAll()
        {
            return viewCourseStaticsGateway.GetAll();
        }

        public List<ViewCourseStatics> GetAll(int id)
        {
            return viewCourseStaticsGateway.GetAll(id);
        }
    }
}