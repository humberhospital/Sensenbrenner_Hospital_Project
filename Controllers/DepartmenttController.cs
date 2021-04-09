using SensenbrennerHospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SensenbrennerHospital.Controllers
{
    public class DepartmenttController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Department
        public ActionResult Index()
        {
            return View();
        }

        public IEnumerable<DepartmentDto> GetDepartments()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            foreach (var department in Departments)
            {
                DepartmentDto NewDepartment = new DepartmentDto
                {
                    DepartmentID = department.DepartmentID,
                    DepartmentName = department.DepartmentName,
                    DepartmentPhoneNumber = department.DepartmentPhoneNumber
                };
                DepartmentDtos.Add(NewDepartment);
            }

            return DepartmentDtos;
        }
    }
}