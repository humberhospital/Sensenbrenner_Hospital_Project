using SensenbrennerHospital.Models;
using SensenbrennerHospital.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SensenbrennerHospital.Controllers
{
    public class DepartmentController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static DepartmentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44336/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //GET: Department/List
        [HttpGet]
        public ActionResult List()
        {
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DepartmentDto> DepartmentList = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                return View(DepartmentList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Department/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Department NewDepartment)
        {
            string url = "DepartmentData/AddDepartment";

            HttpContent content = new StringContent(jss.Serialize(NewDepartment));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int DepartmentID = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Department/DeleteConfirm/3
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                DepartmentDto department = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(department);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //POST: Department/Delete/3
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "DepartmentData/DeleteDepartment/" + id;
            //Body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Department/Update/1
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(SelectedDepartment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //POST: Department/Update/1
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Department DepartmentInfo)
        {
            string url = "DepartmentData/UpdateDepartment/" + id;

            HttpContent content = new StringContent(jss.Serialize(DepartmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //------------------ Commented until practices controller is finished ---------------------
        //GET: Department/Show/5
        //[HttpGet]
        //public ActionResult Show(int id)
        //{
        //    ShowDepartment ViewModel = new ShowDepartment();
        //    string url = "DepartmentData/FindDepartment/" + id;

        //    HttpResponseMessage response = client.GetAsync(url).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
        //        ViewModel.department = SelectedDepartment;

        //        url = "PracticeData/GetPracticesByDepartmentId/" + SelectedDepartment.DepartmentID;
        //        response = client.GetAsync(url).Result;
        //        IEnumerable<Practice> listOfPractices = response.Content.ReadAsAsync<IEnumerable<Practice>>().Result;
        //        ViewModel.listOfPractices = listOfPractices;

        //        return View(ViewModel);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Error");
        //    }
        //}

        public ActionResult Error()
        {
            return View();
        }
    }
}