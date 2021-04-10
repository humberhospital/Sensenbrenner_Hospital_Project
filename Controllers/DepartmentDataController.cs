﻿using SensenbrennerHospital.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SensenbrennerHospital.Controllers
{
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [ResponseType(typeof(IEnumerable<DepartmentDto>))]
        [HttpGet]
        public IHttpActionResult GetDepartments()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> departmentDtos = new List<DepartmentDto>();

            foreach (var department in Departments)
            {
                DepartmentDto NewDepartment = new DepartmentDto
                {
                    DepartmentID = department.DepartmentID,
                    DepartmentName = department.DepartmentName,
                    DepartmentPhoneNumber = department.DepartmentPhoneNumber
                };
                departmentDtos.Add(NewDepartment);
            }

            return Ok(departmentDtos);
        }

        // GET : api/DepartmentData/GetDepartment/7
        [ResponseType(typeof(DepartmentDto))]
        [HttpGet]
        public IHttpActionResult GetDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult AddDepartment([FromBody]Department newDepartment)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Not valid");
                return BadRequest(ModelState);
            }
            Debug.WriteLine(newDepartment);
            db.Departments.Add(newDepartment);
            db.SaveChanges();

            return Ok(newDepartment.DepartmentID);
        }

        [HttpPost]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }
    }
}
