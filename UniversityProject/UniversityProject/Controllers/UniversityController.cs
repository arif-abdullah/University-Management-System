using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using C1.C1Rdl.Rdl2008;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UniversityProject.BLL;
using UniversityProject.Models;
using ViewResult = UniversityProject.Models.ViewResult;

namespace UniversityProject.Controllers
{
    public class UniversityController : Controller
    {
        private DepartmentManager departmentManager = new DepartmentManager();
        private SemesterManager semesterManager = new SemesterManager();
        private CourseManager courseManager = new CourseManager();
        private DesignationManager designationManager = new DesignationManager();
        private TeacherManager teacherManager = new TeacherManager();
        private CourseAssignToTeacherManager courseAssignToTeacher=new CourseAssignToTeacherManager();
        private ViewCourseStaticsManager viewCourseStaticsManager=new ViewCourseStaticsManager();
        private StudentManager studentManager=new StudentManager();
        private AllocateClassRoomsManager allocateClass=new AllocateClassRoomsManager();
        private EnrollStudentManager enrollStudentManager=new EnrollStudentManager();
        private StudentResultManager studentResultManager=new StudentResultManager();
        private ViewResultManager viewResultManager=new ViewResultManager();
        //
        // GET: /University/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewDepartments()
        {
            List<Department> departments = departmentManager.GetAllDepartments();
            return View(departments);
        }

        [HttpGet]
        public ActionResult DepartmentSave()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DepartmentSave(Department department)
        {
            string msg = departmentManager.Save(department);
            ViewBag.Message = msg;
            return View();
        }


        [HttpGet]
        public ActionResult CourseSave()
        {
            List<Department> departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            List<Semesters> semesterses = semesterManager.GetAllSemesterses();
            ViewBag.Semesters = semesterses;
            return View();
        }

        [HttpPost]
        public ActionResult CourseSave(Course course)
        {
            List<Department> departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            List<Semesters> semesterses = semesterManager.GetAllSemesterses();
            ViewBag.Semesters = semesterses;
            string msg = courseManager.Save(course);
            ViewBag.Message = msg;
            return View();
        }

        [HttpGet]
        public ActionResult TeachersSave()
        {
            List<Department> departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            List<Designation> designation = designationManager.GetAllDesignations();
            ViewBag.Designation = designation;
            return View();
        }

        [HttpPost]
        public ActionResult TeachersSave(Teacher teacher)
        {
            List<Department> departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            List<Designation> designation = designationManager.GetAllDesignations();
            ViewBag.Designation = designation;
            string msg = teacherManager.Save(teacher);
            ViewBag.Message = msg;
            return View();
        }

        [HttpGet]
        public ActionResult CourseAssignToTeacher()
        {
            List<Department> departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
          
            return View();
        }
        [HttpPost]
        public ActionResult CourseAssignToTeacher(CourseAssignToTeacher courseAssign)
        {
            List<Department> departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            string msg = courseAssignToTeacher.Save(courseAssign);
            ViewBag.Messages = msg;
            return View();
        }


        public JsonResult GetTeachersByDepartmentId(int departmentId)
        {
            List<Teacher> teachers = teacherManager.GetAllTeachers();
            var teacherslist = teachers.Where(a => a.DepartmentId == departmentId).ToList();
            return Json(teacherslist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCouresDptId(int departmentId)
        {
            List<Course> courses = courseManager.GetCourses();
            var courseInfo = courses.Where(a => a.DepartmentId == departmentId).ToList();

            return Json(courseInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCreditLimitForTeacher(int teacherId)
        {

            List<CourseAssignToTeacher> teachers = courseAssignToTeacher.GetAllTeacherss();
            var credits = teachers.Where(a => a.Id == teacherId).ToList();

            return Json(credits, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCoursesById(int courseId)
        {

            List<Course> courses = courseManager.GetCourses();
            var credits = courses.Where(a => a.Id == courseId).ToList();

            return Json(credits, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewCourseStatics()
        {
            var departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;

            var viewCourseStatics = viewCourseStaticsManager.GetAll();

            return View(viewCourseStatics);
        }


        public JsonResult ViewCourseStatics(int departmentId)
        {
            var departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            var viewCourseStatics = viewCourseStaticsManager.GetAll();
            var view= viewCourseStatics.Where(a => a.Id == departmentId).ToList();

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SaveStudent()
        {
            var departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            //var listOfStudents = studentManager.GetStudents();
            return View();
        }

        [HttpPost]
        public ActionResult SaveStudent(Student aStudent)
        {
            string regNo="" ;
            int count = 0;
            string year = DateTime.Now.Year.ToString();
           var departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            var view = departments.Where(a => a.Id == aStudent.DepartmentId).ToList();
            foreach (Department department in view)
            {
                 regNo += department.Code +"-"+year;
            }
 
            var student = studentManager.GetStudents(regNo);
            if (student == null)
            {
                regNo += "-00" + 1;
            }
            else
            {
                foreach (var countStudent in student)
                {
                    count ++;
                }
                count += 1;
                regNo += "-00" + count;
            }
       
            aStudent.RegistrationNumber = regNo;

            string save = studentManager.Save(aStudent);
            ViewBag.Messages = save;
            var listOfStudents = studentManager.GetSaveStudents(aStudent.RegistrationNumber);
            return View(listOfStudents);
        }

        [HttpGet]
        public ActionResult AllocateClassRoom()
        {
            var departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            var rooms = allocateClass.GetRooms();
            ViewBag.Rooms = rooms;
            return View();
        }
        [HttpPost]
        public ActionResult AllocateClassRoom(ClassRoomAllocate classRoom)
        {
            var departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            var rooms = allocateClass.GetRooms();
            ViewBag.Rooms = rooms;
            string msg = allocateClass.Save(classRoom);
            ViewBag.Msg = msg;
            return View();
        }

        public JsonResult GetCourseByDptId(int departmentId)
        {
            //var departments = departmentManager.GetAllDepartments();
            //ViewBag.Departments = departments;
            var courses = courseManager.GetAll();
            var view = courses.Where(a => a.DepartmentId == departmentId).ToList();

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewRoomAllocation()
        {
            var departments = departmentManager.GetAllDepartments();
            ViewBag.Departments = departments;
            return View();
        }
        public JsonResult ViewRoomAllocationByDeptId(int departmentId)
        {
            //var departments = departmentManager.GetAllDepartments();
            //ViewBag.Departments = departments;
            var getAll = allocateClass.GetSchedulInfo();

            
            var view = getAll.Where(a => a.DepartmentId == departmentId).ToList();
            foreach (ViewClassRoomAllocation split in view)
            {
                string text = split.ScheduleInfo;

                split.ScheduleInfo = text.Replace(";", ";<br/>"); 
            }
            return Json(view, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EnrollCourse()
        {
            var students=studentManager.GetRegNo();
            ViewBag.Students = students;
            return View();
        }

        [HttpPost]
        public ActionResult EnrollCourse(EnrollCourses enroll)
        {
            var students = studentManager.GetRegNo();
            ViewBag.Students = students;
            string msg = enrollStudentManager.Save(enroll);
            ViewBag.Messages = msg;
            return View();
        }

        public JsonResult GetStudentsById(int studentId)
        {
           
            var students = studentManager.GetStudentForEnroll();
            var view = students.Where(a => a.StudentId == studentId).ToList();

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult StudentResult()
        {
            var students = studentManager.GetRegNo();
            ViewBag.Students = students;
            return View();
        }

        [HttpPost]
        public ActionResult StudentResult(StudentResult studentResult)
        {
            var students = studentManager.GetRegNo();
            ViewBag.Students = students;
            string msg = studentResultManager.Save(studentResult);
            ViewBag.Messages = msg;
            return View();
        }
        public JsonResult GetStudentsEnrollById(int studentId)
        {

            var students = studentManager.GetStudentsForSaveResult();
            var view = students.Where(a => a.StudentId == studentId).ToList();

            return Json(view, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewResult()
        {
            var students = studentManager.GetRegNo();
            ViewBag.Students = students;
            return View();
        }
        [HttpPost]
        public ActionResult ViewResult(ViewResult view)
        {
            var students = studentManager.GetRegNo();
            ViewBag.Students = students;
            CreatePdf(view.StudentId);
            return View();
        }
        public JsonResult ViewResultByStudentId(int studentId)
        {

            var students = viewResultManager.GetAll();
            var view = students.Where(a => a.StudentId == studentId).ToList();

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewStudentInfoForResult(int studentId)
        {

            var students = studentManager.GetStudents();
            var view = students.Where(a => a.Id == studentId).ToList();

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UnassignCourses()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UnassignCourses(string confirm_value)
        {
            if (confirm_value == "Yes")
            {
                string msg = courseAssignToTeacher.UnAssignAllCourses();
                ViewBag.Unassign = msg;
            }
            else
            {
                ViewBag.Unassign = "Cancle UnAssign Request";
            }
            return View();
        }

        public ActionResult UnallocateRooms()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UnallocateRooms(string confirm_value)
        {
            if (confirm_value == "Yes")
            {
                string msg = allocateClass.UnAllocateRooms();
                ViewBag.Unassign = msg;
            }
            else
            {
                ViewBag.Unassign = "Cancle Unallocate Rooms";
            }
            return View();
        }
        //Code For iTextSharp
        public FileResult CreatePdf(int studentId)  
        {  
            MemoryStream workStream = new MemoryStream();  
            StringBuilder status = new StringBuilder("");  
            
            //file name to be created   
            string strPDFFileName = string.Format("StudentResult"+studentId+".pdf");  
            Document doc = new Document();  
            doc.SetMargins(0f, 0f, 0f, 0f);  
            //Create PDF Table with 5 columns  
            PdfPTable tableLayout = new PdfPTable(3);  
            //view
           // ViewResult view=new ViewResult();
            doc.SetMargins(0f, 0f, 0f, 0f);  
            //Create PDF Table  
  
            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);  
  
  
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;  
            doc.Open();  
  
            //Add Content to PDF   

            doc.Add(Add_Content_To_PDF(tableLayout, studentId));  
  
            // Closing the document  
            doc.Close();  
  
            byte[] byteInfo = workStream.ToArray();  
            workStream.Write(byteInfo, 0, byteInfo.Length);  
            workStream.Position = 0;  
  
  
            return File(workStream, "application/pdf", strPDFFileName);  
  
        }

        private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout,int studentId)
        {
            float[] headers = {50,24,45}; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;  
            //Add Title to the PDF file at the top  
            
            List <ViewResult> students = viewResultManager.GetAll();
            var view = students.Where(a => a.StudentId == studentId).ToList();
            List<Student> std = studentManager.GetStudents();
            var stdLamda = std.Where(a => a.Id == studentId).ToList();
            Font fontH1 = new Font((Font.FontFamily) 16, Font.NORMAL);
            tableLayout.AddCell(new PdfPCell(new Phrase(" ---Student ResultSheet---", new Font(Font.FontFamily.HELVETICA, 20, 1, new iTextSharp.text.BaseColor(1, 2, 1)))) {  
                Colspan = 12,
                Border = 0, 
                PaddingBottom = 5, 
                HorizontalAlignment = Element.ALIGN_CENTER,
                BackgroundColor = BaseColor.GREEN
            });

            tableLayout.AddCell(new PdfPCell(new Phrase("Student Information :", new Font(Font.FontFamily.HELVETICA, 16, 1, new iTextSharp.text.BaseColor(128, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 10,
                HorizontalAlignment = Element.ALIGN_LEFT
                
            });

            foreach (var stdview in stdLamda)
            {
                
                    tableLayout.AddCell(new PdfPCell(new Phrase("Student Name : " + stdview.Name , new Font(Font.FontFamily.TIMES_ROMAN, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                    {
                    
                        Colspan = 12,
                        Border = 0,
                        PaddingBottom = 5,
                        
                        HorizontalAlignment = Element.ALIGN_LEFT
                    });

                    tableLayout.AddCell(new PdfPCell(new Phrase("Email : " + stdview.Email, new Font(Font.FontFamily.TIMES_ROMAN, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                    {

                        Colspan = 12,
                        Border = 0,
                        PaddingBottom = 5,

                        HorizontalAlignment = Element.ALIGN_LEFT
                    });
                tableLayout.AddCell(new PdfPCell(new Phrase("Registration Number : " + stdview.RegistrationNumber, new Font(Font.FontFamily.TIMES_ROMAN, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                {

                    Colspan = 12,
                    Border = 0,
                    PaddingBottom = 5,

                    HorizontalAlignment = Element.ALIGN_LEFT
                });
                tableLayout.AddCell(new PdfPCell(new Phrase("Department : " + stdview.DepartmentName, new Font(Font.FontFamily.TIMES_ROMAN, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                {

                    Colspan = 12,
                    Border = 0,
                    PaddingBottom = 10,

                    HorizontalAlignment = Element.ALIGN_LEFT
                });
            }

            tableLayout.AddCell(new PdfPCell(new Phrase("Academic Result ", new Font(Font.FontFamily.TIMES_ROMAN, 16, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {

                Colspan = 12,
                Border = 1,
                PaddingBottom = 10,
                PaddingTop = 10,
                HorizontalAlignment = Element.ALIGN_CENTER,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                BorderColor = BaseColor.GREEN
            });
            ////Add header  
            AddCellToHeader(tableLayout, "CourseCode");  
            AddCellToHeader(tableLayout, "Name");  
            AddCellToHeader(tableLayout, "Grade");  
            
  
            ////Add body  

            foreach (var emp in view)   
            {  
  
                AddCellToBody(tableLayout, emp.CourseCode);  
                AddCellToBody(tableLayout, emp.CourseName);  
                AddCellToBody(tableLayout, emp.Grade);  
                
  
            }  
  
            return tableLayout;  

        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)  
        {  
  
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 12, 1, iTextSharp.text.BaseColor.YELLOW)))  
            {  
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5, 
                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)  
            });  
        } 

        private static void AddCellToBody(PdfPTable tableLayout, string cellText)  
        {  
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 12, 1, iTextSharp.text.BaseColor.BLACK)))  
            {  
                HorizontalAlignment = Element.ALIGN_LEFT, 
                Padding = 5, 
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)  
            });  
        }  
    }
}