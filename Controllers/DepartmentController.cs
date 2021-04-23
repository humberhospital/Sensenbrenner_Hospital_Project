using SensenbrennerHospital.Models;
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

        public ActionResult List()
        {
            string url = "DepartmentData/GetDepartments";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<DepartmentDto> DepartmentList = httpResponse.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                return View(DepartmentList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Department NewDepartment)
        {
            string url = "DepartmentData/AddDepartment";

            Debug.WriteLine(NewDepartment.DepartmentName);
            HttpContent content = new StringContent(jss.Serialize(NewDepartment));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;
            //Debug.WriteLine(jss.Serialize(NewDepartment));
            if (httpResponse.IsSuccessStatusCode)
            {
                int DepartmentID = httpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DepartmentData/GetDepartment/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                DepartmentDto department = new DepartmentDto();
                department = httpResponse.Content.ReadAsAsync<DepartmentDto>().Result;
                return View(department);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "DepartmentData/DeleteDepartment/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;
            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IEnumerable<DepartmentDto> DepartmentList()
        {
            string url = "DepartmentData/GetListOfDepartments";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<DepartmentDto> DepartmentList = httpResponse.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
                return DepartmentList;
            }
            else
            {
                return null;
            }
        }
    }
}